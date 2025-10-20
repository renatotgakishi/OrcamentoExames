using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OrcamentoMedico.Application.DTO
{
    public class UploadExameRequest
    {
        [Required]
        public int IdUsuario { get; set; }

        [Required]
        public IFormFile? Imagem { get; set; }
    }
}