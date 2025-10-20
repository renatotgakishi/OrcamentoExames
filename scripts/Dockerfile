# Etapa 1: build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia todos os arquivos do projeto
COPY .. ./

# Restaura dependências apenas da API
RUN dotnet restore src/OrcamentoMedico.API/OrcamentoMedico.API.csproj

# Publica apenas a API
RUN dotnet publish src/OrcamentoMedico.API/OrcamentoMedico.API.csproj -c Release -o /app/out

# Etapa 2: imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "OrcamentoMedico.API.dll"]