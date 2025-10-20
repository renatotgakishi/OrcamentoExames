using OrcamentoMedico.Domain.Events;

namespace OrcamentoMedico.Application.Interfaces
{
    public interface IPedidoRepository
    {
        Task SalvarAsync(PedidoCriadoEvent evento);
    }
}