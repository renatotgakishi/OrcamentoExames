namespace OrcamentoMedico.Domain.Entities;
public class Usuario
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;    
    public DateTimeOffset CriadoEm { get; set; } // ✅ Compatível com DATETIMEOFFSET no SQL Server

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}

