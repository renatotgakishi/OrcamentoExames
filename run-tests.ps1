# run-tests.ps1
Write-Host "Executando testes unitários..." -ForegroundColor Cyan

# Caminho do projeto de testes
$testProject = ".\tests\OrcamentoMedico.Tests\OrcamentoMedico.Tests.csproj"

# Executa os testes com saída limpa e coleta cobertura
dotnet test $testProject `
    --logger "console;verbosity=minimal" `
    --collect:"Code Coverage" `
    --results-directory ".\TestResults" `
    --no-build

Write-Host ""
Write-Host "Testes finalizados. Verifique os resultados acima." -ForegroundColor Green