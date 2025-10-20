using System.Threading.Tasks;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Domain.Entities;
using OrcamentoMedico.Domain.Events;
using OrcamentoMedico.Infrastructure.Persistence;

namespace OrcamentoMedico.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    
    private readonly AppDbContext _context;
    private readonly IUsuarioRepository _usuarioRepository;
    public PedidoRepository(AppDbContext context, IUsuarioRepository usuarioRepository)
    {
        _context = context;
        _usuarioRepository = usuarioRepository;
    }

    public async Task SalvarAsync(PedidoCriadoEvent evento)
    {
        var usuario = _usuarioRepository.ObterPorId(evento.IdUsuario);
        if (usuario == null)
        {
            throw new InvalidOperationException($"Usuário com ID {evento.IdUsuario} não encontrado.");
        }

        var pedido = new Pedido
        {
            Id = evento.PedidoId,
            IdUsuario = evento.IdUsuario,
            ImagemS3 = evento.ImagemS3,
            CriadoEm = evento.DataCriacao,
            Status = "PENDENTE",
            EmAtendimento = false
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
    }
}

