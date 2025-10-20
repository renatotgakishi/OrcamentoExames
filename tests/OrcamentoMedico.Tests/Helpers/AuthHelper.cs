using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrcamentoMedico.Tests.Helpers
{
    public static class AuthHelper
    {
        public static async Task<string> GerarTokenAsync(HttpClient client)
        {
            var login = new
            {
                Usuario = "admin",
                Senha = "123456"
            };

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/Auth/login", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            return (string)obj.token;
        }
    }
}