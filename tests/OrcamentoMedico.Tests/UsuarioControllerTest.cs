using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace OrcamentoMedico.Tests
{
    // 1. DTOs/Modelos (Melhora a tipagem e clareza do que está sendo enviado/recebido)
    public class CriarUsuarioRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class CriarUsuarioResponse
    {
        // O tipo está correto como Guid para corresponder à entidade.
        // Verifique se o nome da propriedade no JSON da API é "idUsuario" ou apenas "id".
        [JsonProperty("idUsuario")]
        public Guid IdUsuario { get; set; }
    }

    public class UsuariosControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        // Base URL para a Controller de Usuários
        private const string BaseUrl = "/api/usuarios";

        public UsuariosControllerTests(WebApplicationFactory<Program> factory)
        {
            // O cliente é criado e gerenciado pelo WebApplicationFactory
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CriarUsuario_DeveRetornar201ComIdValido_EDeletar()
        {
            // ARRANGE: Preparação dos dados
            var emailUnico = $"renato_{Guid.NewGuid()}@example.com";
            var usuarioNovo = new CriarUsuarioRequest
            {
                Nome = "Renato Teste",
                Email = emailUnico,
                Senha = "123456"
            };

            string? idUsuarioCriado = null; // Inicializa a variável para garantir o cleanup (Tipo string é necessário para a URL)

            try
            {
                // ACT: 1. Criação do Usuário
                var response = await PostJsonAsync(_client, BaseUrl, usuarioNovo);

                // ASSERT: Verifica o sucesso da criação
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);

                // ACT: 2. Extração do ID do Usuário Criado
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // 🔍 DEBUG: Imprime o JSON retornado para verificar o nome da propriedade (idUsuario vs id)
                Console.WriteLine($"🔍 Resposta JSON da API: {jsonResponse}");

                var usuarioCriado = JsonConvert.DeserializeObject<CriarUsuarioResponse>(jsonResponse);

                // ✅ CORREÇÃO APLICADA: Usa ?. para navegação segura e .ToString() para conversão explícita de Guid para string
                idUsuarioCriado = usuarioCriado?.IdUsuario.ToString();

                // ASSERT: Verifica se o ID foi retornado (e convertido para string)
                Assert.False(string.IsNullOrEmpty(idUsuarioCriado), "O ID do usuário não deve ser nulo ou vazio na resposta de criação.");
                Console.WriteLine($"✅ Usuário criado com sucesso. ID: {idUsuarioCriado}");

                // ACT: 3. Deletar o usuário criado
                var deleteResponse = await _client.DeleteAsync($"{BaseUrl}/{idUsuarioCriado}");

                // ASSERT: Verifica se a deleção foi bem-sucedida (204 NoContent ou 404 NotFound se já tiver sido removido)
                Assert.True(deleteResponse.StatusCode == HttpStatusCode.NoContent ||
                deleteResponse.StatusCode == HttpStatusCode.NotFound,
                $"Esperado NoContent ou NotFound, mas foi {deleteResponse.StatusCode}"
                );
            }
            finally
            {
                // CLEANUP (Garante que o recurso seja excluído mesmo se a criação falhar parcialmente)
                if (!string.IsNullOrEmpty(idUsuarioCriado))
                {
                    await _client.DeleteAsync($"{BaseUrl}/{idUsuarioCriado}");
                }
            }
        }

        // 2. Método Auxiliar para Envio de JSON (Melhora a clareza e reutilização)
        private static async Task<HttpResponseMessage> PostJsonAsync<T>(HttpClient client, string url, T data)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );
            return await client.PostAsync(url, content);
        }
    }
}