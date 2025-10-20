namespace OrcamentoMedico.Domain.Entities;

public class Pedido
{
    public Guid Id { get; set; }
    public Guid IdUsuario { get; set; }
    public DateTime CriadoEm { get; set; }
    public string Status { get; set; } = "PENDENTE";
    public string ImagemS3 { get; set; } = string.Empty;
    public bool EmAtendimento { get; set; }

    // 🔧 Propriedades de navegação
    public Usuario Usuario { get; set; } = null!;
    public ICollection<Orcamento> Orcamentos { get; set; } = new List<Orcamento>();
}