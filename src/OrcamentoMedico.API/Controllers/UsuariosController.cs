using Microsoft.AspNetCore.Mvc;
using OrcamentoMedico.Application.Common;
using OrcamentoMedico.Application.DTOs;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Domain.Entities;

namespace OrcamentoMedico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuariosController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult CriarUsuario([FromBody] UsuarioRequest request)
        {
            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = HashHelper.GerarHash(request.Senha),
                CriadoEm = DateTime.UtcNow
            };

            var id = _repository.Criar(usuario);
            return Created("", new { idUsuario = id });
        }
        [HttpGet("email/{email}")]
        public IActionResult ObterPorEmail(string email)
        {
            var usuario = _repository.ObterPorEmail(email);
            if (usuario == null)
                return NotFound();

            return Ok(new { idUsuario = usuario.Id });
        }

        
        [HttpDelete("{id}")]
        public IActionResult Remover(Guid id)
        {
            
            if (_repository.Remover(id))
            {
                return NoContent(); // Retorna 204
            }

            // Se _repository.Remover(id) retornar false, significa que não encontrou.
            return NotFound(); // Retorna 404
        }

        // ...

    }
}