dotnet new sln -n OrcamentoMedico

cd src
dotnet new classlib -n OrcamentoMedico.Domain
dotnet new classlib -n OrcamentoMedico.Application  
dotnet new classlib -n OrcamentoMedico.Infrastructure
dotnet new webapi -n OrcamentoMedico.API
dotnet new classlib -n OrcamentoMedico.Worker
dotnet new classlib -n OrcamentoMedico.Auth

cd ..
dotnet sln add src/OrcamentoMedico.Domain
dotnet sln add src/OrcamentoMedico.Application
dotnet sln add src/OrcamentoMedico.Infrastructure
dotnet sln add src/OrcamentoMedico.API
dotnet sln add src/OrcamentoMedico.Worker
dotnet sln add src/OrcamentoMedico.Auth

cd tests
dotnet new xunit -n OrcamentoMedico.Tests
dotnet sln add tests/OrcamentoMedico.Tests
