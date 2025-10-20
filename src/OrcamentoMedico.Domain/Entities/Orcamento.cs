namespace OrcamentoMedico.Domain.Entities;

public class Orcamento
{
    public Guid Id { get; set; }
    public Guid PedidoId { get; set; }
    public Guid? AtententeId { get; set; }
    public decimal Valor { get; set; }
    public int PrazoDias { get; set; }
    public bool Aprovado { get; set; }
    public DateTime CriadoEm { get; set; }

    public Pedido Pedido { get; set; } = null!;
    public Atendente? Atendente { get; set; }
}