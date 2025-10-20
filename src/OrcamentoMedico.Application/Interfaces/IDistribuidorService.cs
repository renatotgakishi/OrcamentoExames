using OrcamentoMedico.Domain.Events;

namespace OrcamentoMedico.Application.Interfaces
{
    public interface IDistribuidorService
    {
        Task DistribuirAsync(PedidoCriadoEvent evento);
    }
}