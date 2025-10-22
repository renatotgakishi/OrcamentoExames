using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

public class PedidoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PedidoControllerTests(WebApplicationFactory<Program> factory)
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

        var loginJson = JsonConvert.SerializeObject(login);
        var content = new StringContent(loginJson, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/Auth/login", content);

        var json = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var obj = JsonConvert.DeserializeObject<dynamic>(json);
        string token = obj.data?.token;

        Assert.False(string.IsNullOrEmpty(token)); // Garante que o token foi gerado

        return token;
    }

    [Fact]
    public async Task PostPedido_ComLogin_DeveRetornar201()
    {
        var token = await GerarTokenAsync();

        var pedido = new
        {
            idUsuario = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            imagemS3 = "string",
            emailUsuario = "user@example.com"
        };

        var pedidoJson = JsonConvert.SerializeObject(pedido);
        var content = new StringContent(pedidoJson, Encoding.UTF8, "application/json");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsync("/api/pedidos", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(responseBody)); // Garante que há resposta
    }

    [Fact]
    public async Task PostPedido_SemToken_DeveRetornar401()
    {
        var pedido = new
        {
            idUsuario = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            imagemS3 = "string",
            emailUsuario = "user@example.com"
        };

        var content = new StringContent(JsonConvert.SerializeObject(pedido), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/pedidos", content);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}