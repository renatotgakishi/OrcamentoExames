namespace OrcamentoMedico.Application.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadAsync(Stream fileStream, string fileName);
    }
}