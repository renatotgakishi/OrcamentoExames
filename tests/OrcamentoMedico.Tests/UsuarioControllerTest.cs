using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace OrcamentoMedico.Tests
{
    public class UsuariosControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsuariosControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task CriarUsuario_DeveRetornar201ComIdValido_EDeletar()
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

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<dynamic>(json);
            var idUsuario = (string)obj.idUsuario;

            Console.WriteLine("🆔 IdUsuario criado: " + idUsuario);

            Assert.False(string.IsNullOrEmpty(idUsuario));

            // 🔄 Deletar o usuário criado
            var deleteResponse = await _client.DeleteAsync($"/api/usuarios/{idUsuario}");
            Console.WriteLine("🗑️ Status da exclusão: " + deleteResponse.StatusCode);
            Console.WriteLine("🗑️ Corpo da resposta: " + await deleteResponse.Content.ReadAsStringAsync());
            Console.WriteLine("🗑️ IdUsuario deletado: " + idUsuario);
            

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

    }
}