using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using OrcamentoMedico.Application.Interfaces;

namespace OrcamentoMedico.Infrastructure.Aws
{
    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueName;
        private string _queueUrl;

        public SqsService(IConfiguration config)
        {
            var sqsConfig = new AmazonSQSConfig
            {
                ServiceURL = config["AWS:ServiceURL"]
            };

            _sqs = new AmazonSQSClient(
                config["AWS:AccessKey"],
                config["AWS:SecretKey"],
                sqsConfig
            );

            _queueName = config["AWS:SqsQueue"]
                         ?? throw new InvalidOperationException("SqsQueue config is missing.");
        }

        public async Task PublishAsync<T>(T evento)
        {
            await EnsureQueueExistsAsync();

            var message = JsonSerializer.Serialize(evento);

            var request = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = message
            };

            await _sqs.SendMessageAsync(request);
        }

        private async Task EnsureQueueExistsAsync()
        {
            if (!string.IsNullOrEmpty(_queueUrl)) return;

            try
            {
                var response = await _sqs.GetQueueUrlAsync(_queueName);
                _queueUrl = response.QueueUrl;
            }
            catch (QueueDoesNotExistException)
            {
                var createResponse = await _sqs.CreateQueueAsync(new CreateQueueRequest
                {
                    QueueName = _queueName
                });
                _queueUrl = createResponse.QueueUrl;
            }
        }

        // (Opcional) para inicializar no startup
        public async Task InitializeAsync()
        {
            await EnsureQueueExistsAsync();
        }
    }
}