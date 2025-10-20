using OrcamentoMedico.Domain.Entities;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Infrastructure.Persistence;

namespace OrcamentoMedico.Infrastructure.Repositories
{
    using OrcamentoMedico.Application.Interfaces;
    using OrcamentoMedico.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public Guid Criar(Usuario usuario)
        {
            usuario.Id = Guid.NewGuid();
            usuario.CriadoEm = DateTime.UtcNow;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return usuario.Id;
        }

        public bool Existe(Guid id)
        {
            return _context.Usuarios.Any(u => u.Id == id);
        }

        public Usuario? ObterPorId(Guid id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }
        public Usuario? ObterPorEmail(string email)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<Usuario> ListarTodos()
        {
            return _context.Usuarios.AsNoTracking().ToList();
        }
        public bool Remover(Guid id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return true;
        }
    }

}