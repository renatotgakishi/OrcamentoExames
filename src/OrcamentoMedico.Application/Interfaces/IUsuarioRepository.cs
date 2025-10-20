using OrcamentoMedico.Domain.Entities;

namespace OrcamentoMedico.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Guid Criar(Usuario usuario);
        bool Existe(Guid id);
        Usuario? ObterPorId(Guid id);
        Usuario? ObterPorEmail(string email);
        IEnumerable<Usuario> ListarTodos();
        bool Remover(Guid id);
    }
}