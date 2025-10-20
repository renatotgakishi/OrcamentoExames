using OrcamentoMedico.Domain.Events;

namespace OrcamentoMedico.Application.Interfaces
{
    public interface ISqsService
    {
        Task PublishAsync<T>(T evento);
    }
}