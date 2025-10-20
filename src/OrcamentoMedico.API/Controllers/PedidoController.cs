using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Domain.DTO;

namespace OrcamentoMedico.API.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CriarPedido([FromBody] PedidoRequest request)
        {
            var pedidoId = await _pedidoService.CriarPedidoAsync(request);
            return CreatedAtAction(nameof(CriarPedido), new { id = pedidoId }, new { pedidoId });
        }
    }
}