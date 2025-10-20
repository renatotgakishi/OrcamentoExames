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
    public async Task PostPedido_ComLogin_DeveRetornar200()
    {

        var token = await GerarTokenAsync();

        var pedido = new
        {
            idUsuario = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            imagemS3 = "string",
            emailUsuario = "user@example.com"
        };

        var pedidoJson = JsonConvert.SerializeObject(pedido);
        Console.WriteLine("📦 Enviando para /api/pedidos:");
        Console.WriteLine(pedidoJson);

        var content = new StringContent(pedidoJson, Encoding.UTF8, "application/json");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsync("/api/pedidos", content);
        Console.WriteLine($"📦 Status da resposta: {(int)response.StatusCode} {response.StatusCode}");

        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("📦 Corpo da resposta:");
        Console.WriteLine(responseBody);

        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
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