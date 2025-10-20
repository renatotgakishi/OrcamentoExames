using System;
using System.Threading.Tasks;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Domain.DTO;
using OrcamentoMedico.Domain.Events;

namespace OrcamentoMedico.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly ISqsService _sqs;

        public PedidoService(ISqsService sqs)
        {
            _sqs = sqs;
        }

        public async Task<Guid> CriarPedidoAsync(PedidoRequest request)
        {
            var pedidoId = Guid.NewGuid();

            var evento = new PedidoCriadoEvent
            {
                PedidoId = pedidoId,
                IdUsuario = request.IdUsuario,
                EmailUsuario = request.EmailUsuario,
                ImagemS3 = request.ImagemS3,
                DataCriacao = DateTime.UtcNow
            };

            await _sqs.PublishAsync(evento);
            return pedidoId;
        }
    }
}