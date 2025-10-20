using System;

namespace OrcamentoMedico.Domain.Events
{
    public class PedidoCriadoEvent
    {
        public Guid PedidoId { get; set; }
        public Guid IdUsuario { get; set; }
        public string EmailUsuario { get; set; } = string.Empty;
        public string ImagemS3 { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}