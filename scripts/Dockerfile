# Etapa 1: build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia os arquivos do projeto
COPY . ./

# Restaura dependências e compila
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa 2: imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados da etapa anterior
COPY --from=build /app/out .

# Expõe a porta padrão do container
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "OrcamentoMedico.dll"]