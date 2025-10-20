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
    public class PedidoEmailConsumer : BackgroundService
    {
        private readonly IAmazonSQS _sqs;
        private readonly IEmailService _emailService;
        private readonly string _queueUrl = "http://localhost:4566/000000000000/pedido-criado";

        public PedidoEmailConsumer(IAmazonSQS sqs, IEmailService emailService)
        {
            _sqs = sqs;
            _emailService = emailService;
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
                        await _emailService.SendAsync(
                            evento.EmailUsuario,
                            "Seu pedido foi recebido!",
                            $"Pedido ID: {evento.PedidoId}"
                        );
                    }

                    //await _sqs.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                }
            }
        }
    }
}