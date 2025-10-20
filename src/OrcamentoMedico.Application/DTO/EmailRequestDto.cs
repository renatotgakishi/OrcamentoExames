namespace OrcamentoMedico.Application.DTO
{
    public class EmailRequestDto
    {
        public string Para { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Corpo { get; set; } = string.Empty;
    }
}