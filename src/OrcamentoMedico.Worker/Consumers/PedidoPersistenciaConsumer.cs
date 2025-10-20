using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using OrcamentoMedico.Domain.Events;
using OrcamentoMedico.Application.Interfaces;

namespace OrcamentoMedico.Worker.Consumers
{
    public class PedidoPersistenciaConsumer : BackgroundService
    {
        private readonly IAmazonSQS _sqs;
        private readonly IPedidoRepository _repository;
        private readonly string _queueUrl = "http://localhost:4566/000000000000/pedido-criado";


        public PedidoPersistenciaConsumer(IAmazonSQS sqs, IPedidoRepository repository)
        {
            _sqs = sqs;
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 1,
                    WaitTimeSeconds = 10
                });

                foreach (var message in response.Messages)
                {
                    var evento = JsonSerializer.Deserialize<PedidoCriadoEvent>(message.Body);
                    if (evento != null)
                    {
                        await _repository.SalvarAsync(evento);
                    }

                   // await _sqs.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                }
            }
        }
    }
}