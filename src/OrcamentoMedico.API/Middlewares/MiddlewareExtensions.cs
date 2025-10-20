using Microsoft.AspNetCore.Builder;

namespace OrcamentoMedico.API.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiResponseMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiResponseMiddleware>();
        }
    }
}