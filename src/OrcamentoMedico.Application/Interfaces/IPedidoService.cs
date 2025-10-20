using System;
using System.Threading.Tasks;
using OrcamentoMedico.Application.DTO;

namespace OrcamentoMedico.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<Guid> CriarPedidoAsync(PedidoRequest request);
    }
}