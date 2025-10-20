using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrcamentoMedico.Application.DTO;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Domain.Events;

namespace OrcamentoMedico.API.Controllers;

[ApiController]
[Route("api/pedidos-db")]
public class PedidoDbController : ControllerBase
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoDbController(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarPedidoDb([FromBody] PedidoRequest request)
    {
        var evento = new PedidoCriadoEvent
        {
            PedidoId = Guid.NewGuid(),
            IdUsuario = request.IdUsuario,
            EmailUsuario = request.EmailUsuario,
            ImagemS3 = request.ImagemS3,
            DataCriacao = DateTime.UtcNow
        };

        await _pedidoRepository.SalvarAsync(evento);

        return CreatedAtAction(nameof(CriarPedidoDb), new { id = evento.PedidoId }, new { evento.PedidoId });
    }
}