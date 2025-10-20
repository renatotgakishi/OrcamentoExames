namespace OrcamentoMedico.Domain.Events
{
    public class UploadCriadoEvent
    {
        public int IdUsuario { get; set; }
        public string ImagemS3 { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}