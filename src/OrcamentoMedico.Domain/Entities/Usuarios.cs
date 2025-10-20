namespace OrcamentoMedico.Domain.Entities;
public class Usuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}

