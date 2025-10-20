using Microsoft.AspNetCore.Mvc;
using OrcamentoMedico.Application.DTO;
using OrcamentoMedico.Application.Interfaces;

namespace OrcamentoMedico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> EnviarEmail([FromBody] EmailRequestDto dto)
        {
            await _emailService.SendAsync(dto.Para, dto.Assunto, dto.Corpo);
            return Ok(new { mensagem = "E-mail enviado com sucesso!" });
        }
    }
}