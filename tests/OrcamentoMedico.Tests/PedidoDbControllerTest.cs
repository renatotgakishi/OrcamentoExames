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
    public class PedidoDbControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PedidoDbControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        private async Task<string> GerarTokenAsync()
        {
            var login = new
            {
                Usuario = "admin",
                Senha = "123456"
            };

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Auth/login", content);

            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao autenticar: {response.StatusCode}\n{json}");
            }

            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            string token = obj?.data?.token;
            Assert.False(string.IsNullOrEmpty(token), "Token JWT não foi retornado.");
            return token;
        }

        private async Task<string> ObterOuCriarUsuarioAsync()
        {
            var email = $"kishi_{Guid.NewGuid()}@example.com";

            // Tenta buscar o usuário
            var response = await _client.GetAsync($"/api/usuarios/email/{email}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                string id = obj?.data?.idUsuario;
                Assert.False(string.IsNullOrEmpty(id), "ID do usuário existente não foi retornado.");
                return id;
            }

            // Cria novo usuário
            var usuario = new
            {
                nome = "kishi",
                email = email,
                senha = "123456"
            };

            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/usuarios", content);
            var jsonNovo = await createResponse.Content.ReadAsStringAsync();

            if (!createResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao criar usuário: {createResponse.StatusCode}\n{jsonNovo}");
            }

            var objNovo = JsonConvert.DeserializeObject<dynamic>(jsonNovo);
            string idNovo = objNovo?.data?.idUsuario;
            Assert.False(string.IsNullOrEmpty(idNovo), "ID do novo usuário não foi retornado.");
            return idNovo;
        }

        [Fact]
        public async Task PostPedidoDb_ComUsuarioCriado_DeveRetornar201()
        {
            var token = await GerarTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var idUsuario = await ObterOuCriarUsuarioAsync();

            var pedidoDb = new
            {
                idUsuario,
                imagemS3 = "string",
                emailUsuario = "renato@example.com"
            };

            var content = new StringContent(JsonConvert.SerializeObject(pedidoDb), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/pedidos-db", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao criar pedido: {response.StatusCode}\n{responseBody}");
            }

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(responseBody), "Resposta do pedido está vazia.");
        }
    }
}