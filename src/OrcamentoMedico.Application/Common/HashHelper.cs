using System.Text;

namespace OrcamentoMedico.Application.Common
{
    public static class HashHelper
    {
        public static string GerarHash(string senha)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}