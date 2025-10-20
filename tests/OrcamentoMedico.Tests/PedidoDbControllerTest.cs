using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using OrcamentoMedico.Tests.Helpers;

namespace OrcamentoMedico.Tests
{
    public class PedidoControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PedidoControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        /*private async Task<string> CriarUsuarioAsync()
        {
            var emailUnico = $"renato_{Guid.NewGuid()}@example.com";

            var usuario = new
            {
                nome = "Renato",
                email = emailUnico,
                senha = "123456"
            };

            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/usuarios", content);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.EnsureSuccessStatusCode());
            Console.WriteLine("🔹 StatusCode do usuario retornado: " + response.StatusCode);


            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            return (string)obj.idUsuario;
        }*/
        private async Task<string> ObterOuCriarUsuarioAsync()
        {
            var email = "kishi@example.com";

            // Tenta buscar o usuário existente
            var response = await _client.GetAsync($"/api/usuarios/email/{email}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return (string)obj.idUsuario;
            }

            // Se não existir, cria um novo
            var usuario = new
            {
                nome = "kishi",
                email = email,
                senha = "123456"
            };

            var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/usuarios", content);
            createResponse.EnsureSuccessStatusCode();

            var jsonNovo = await createResponse.Content.ReadAsStringAsync();
            var objNovo = JsonConvert.DeserializeObject<dynamic>(jsonNovo);
            return (string)objNovo.idUsuario;
        }

        private async Task<string> GerarTokenAsync()
        {
            var login = new
            {
                Usuario = "admin",
                Senha = "123456"
            };

            var loginJson = JsonConvert.SerializeObject(login);
            Console.WriteLine("🔐 Enviando para /api/Auth/login:");
            Console.WriteLine(loginJson);

            var content = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Auth/login", content);

            Console.WriteLine($"🔐 Status da resposta: {(int)response.StatusCode} {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("🔐 Corpo da resposta:");
            Console.WriteLine(json);

            response.EnsureSuccessStatusCode();

            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            var token = (string)obj.token;
            Console.WriteLine("🔐 Token gerado:");
            Console.WriteLine(token);

            return token;
        }


        [Fact]
        public async Task PostPedidoDb_ComUsuarioCriado_DeveRetornar201()
        {
            var token = await GerarTokenAsync();
            //var token = await AuthHelper.GerarTokenAsync(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var idUsuario = await ObterOuCriarUsuarioAsync();

            var pedidoDb = new
            {
                idUsuario = idUsuario, //"4C5D311D-C385-4274-892B-2CD0783DB9C1",
                imagemS3 = "string",
                emailUsuario = "renato@example.com"
            };

            var jsonPedido = JsonConvert.SerializeObject(pedidoDb);
            Console.WriteLine("🔹 JSON enviado para /api/pedidos-db:");
            Console.WriteLine(jsonPedido);

            var content = new StringContent(jsonPedido, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/pedidos-db", content);

            Console.WriteLine("🔹 StatusCode retornado: " + response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("🔹 Corpo da resposta:");
            Console.WriteLine(responseBody);



            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}