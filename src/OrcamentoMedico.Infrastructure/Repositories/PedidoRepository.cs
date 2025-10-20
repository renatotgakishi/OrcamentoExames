using System.Threading.Tasks;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Domain.Entities;
using OrcamentoMedico.Domain.Events;
using OrcamentoMedico.Infrastructure.Persistence;

namespace OrcamentoMedico.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task SalvarAsync(PedidoCriadoEvent evento)
    {
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