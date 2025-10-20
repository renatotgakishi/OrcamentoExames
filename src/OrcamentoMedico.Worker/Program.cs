using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrcamentoMedico.Worker;
using OrcamentoMedico.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<PedidoEmailConsumer>();
builder.Services.AddHostedService<PedidoPersistenciaConsumer>();
builder.Services.AddHostedService<OrcamentoDistribuidorConsumer>();

var host = builder.Build();
host.Run();