using System;
using System.Threading.Tasks;
using OrcamentoMedico.Domain.DTO;

namespace OrcamentoMedico.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<Guid> CriarPedidoAsync(PedidoRequest request);
    }
}