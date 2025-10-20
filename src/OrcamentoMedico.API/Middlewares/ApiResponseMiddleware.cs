using System.Net;
using System.Text;
using System.Text.Json;

public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiResponseMiddleware> _logger;

    public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Captura o corpo original da resposta para podermos modificá-lo
        var originalBodyStream = context.Response.Body;
        
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
            
            // Só formatamos se for uma resposta JSON bem-sucedida (200-299)
            if (IsSuccessfulJsonResponse(context))
            {
                var body = await FormatResponseBody(context.Response);
                await WriteFormattedResponse(context, originalBodyStream, body);
            }
            else
            {
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro não tratado");
            await HandleExceptionAsync(context, originalBodyStream, ex);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private bool IsSuccessfulJsonResponse(HttpContext context)
    {
        return context.Response.ContentType?.Contains("application/json") == true &&
               context.Response.StatusCode >= 200 && 
               context.Response.StatusCode < 300;
    }

    private async Task<string> FormatResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        var result = JsonSerializer.Deserialize<object>(bodyAsText);
        
        // Padrão de resposta consistente
        var formattedResponse = new
        {
            Success = true,
            Data = result,
            Timestamp = DateTime.UtcNow,
            StatusCode = response.StatusCode
        };

        return JsonSerializer.Serialize(formattedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });
    }

    private async Task WriteFormattedResponse(HttpContext context, Stream originalBody, string body)
    {
        var buffer = Encoding.UTF8.GetBytes(body);
        context.Response.ContentLength = buffer.Length;
        await originalBody.WriteAsync(buffer, 0, buffer.Length);
    }

    private async Task HandleExceptionAsync(HttpContext context, Stream originalBody, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            Success = false,
            Error = new
            {
                Message = "Ocorreu um erro ao processar sua requisição",
                Details = exception.Message,
                ExceptionType = exception.GetType().Name
            },
            Timestamp = DateTime.UtcNow,
            StatusCode = context.Response.StatusCode
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var buffer = Encoding.UTF8.GetBytes(jsonResponse);
        await originalBody.WriteAsync(buffer, 0, buffer.Length);
    }
}