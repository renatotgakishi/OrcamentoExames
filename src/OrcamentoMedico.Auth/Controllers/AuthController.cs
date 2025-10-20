using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrcamentoMedico.Auth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrcamentoMedico.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Usuario == "admin" && request.Senha == "123456")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Usuario)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minha-chave-super-secreta-de-32-caracteres"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "orcamento-medico",
                    audience: "orcamento-medico-client",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }
    }
}