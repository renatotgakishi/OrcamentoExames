using System;
using System.ComponentModel.DataAnnotations;

namespace OrcamentoMedico.Application.DTO
{
    public class PedidoRequest
    {
        [Required]
        public Guid IdUsuario { get; set; }

        [Required]
        public string ImagemS3 { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailUsuario { get; set; } = string.Empty;
    }
}
