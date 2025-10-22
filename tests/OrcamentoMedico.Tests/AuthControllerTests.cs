using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace OrcamentoMedico.Tests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        
        public async Task Login_ComCredenciaisValidas_DeveRetornarToken()
        {
            var login = new
            {
                Usuario = "admin",
                Senha = "123456"
            };

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Auth/login", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);

            string token = obj.data?.token;
            Assert.False(string.IsNullOrEmpty(token));
        }
        [Fact]
        public async Task Login_ComCredenciaisInvalidas_DeveRetornar401()
        {
            var login = new
            {
                Usuario = "admin",
                Senha = "senhaErrada"
            };

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Auth/login", content);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


    }
}