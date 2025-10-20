using System.ComponentModel.DataAnnotations;

namespace OrcamentoMedico.Auth.Models
{
    public class LoginRequest
    {
        [Required]
        public required string Usuario { get; set; }

        [Required]
        public required string Senha { get; set; }
    }
}