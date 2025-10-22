namespace OrcamentoMedico.Domain.Entities;
public class Atendente
{
    public Guid Id { get; set; }
    public Guid IdUsuario { get; set; }
    public DateTime CriadoEm { get; set; }
    public Usuario Usuario { get; set; } = null!;
}