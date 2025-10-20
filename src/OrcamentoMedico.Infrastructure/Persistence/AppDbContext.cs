using Microsoft.EntityFrameworkCore;
using OrcamentoMedico.Domain.Entities;

namespace OrcamentoMedico.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Atendente> Atendentes => Set<Atendente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Orcamento> Orcamentos => Set<Orcamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Pedidos)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.IdUsuario);

        modelBuilder.Entity<Atendente>()
            .HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.IdUsuario);

        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.Orcamentos)
            .WithOne(o => o.Pedido)
            .HasForeignKey(o => o.PedidoId);

        modelBuilder.Entity<Orcamento>()
            .HasOne(o => o.Atendente)
            .WithMany()
            .HasForeignKey(o => o.AtententeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}