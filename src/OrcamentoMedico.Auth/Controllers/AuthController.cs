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
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Usuario == "admin" && request.Senha == "123456")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Usuario)
                };

                var jwtSection = _configuration.GetSection("JWT");
                var secret = jwtSection.GetValue<string>("Secret") ?? throw new InvalidOperationException("JWT Secret is not configured.");
                var issuer = jwtSection.GetValue<string>("Issuer");
                var audience = jwtSection.GetValue<string>("Audience");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }
    }
}