using Microsoft.EntityFrameworkCore;
using OrcamentoMedico.API.Extensions;
using OrcamentoMedico.Infrastructure.Persistence;
using OrcamentoMedico.API.Middlewares;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers()
            .AddApplicationPart(typeof(OrcamentoMedico.Auth.Controllers.AuthController).Assembly);

        builder.Services
            .AddApplicationServices()
            .AddJwtAuthentication(builder.Configuration)
            .AddSwaggerDocumentation();

        var app = builder.Build();

        // 🔹 Middleware de resposta customizado

        //app.UseApiResponseMiddleware(); // Deve vir antes de autenticação

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrcamentoMedico.API v1");
            });
        }
        app.UseMiddleware<ApiResponseMiddleware>();
        
        app.MapControllers();
        app.Run();

    }
}