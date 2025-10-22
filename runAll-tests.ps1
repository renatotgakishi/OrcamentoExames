# runAll-tests.ps1
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

Write-Host "Gerando relatorio de cobertura..." -ForegroundColor Cyan

# Localiza o arquivo mais recente de cobertura
$coverageFile = Get-ChildItem -Recurse -Filter coverage.cobertura.xml |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if ($coverageFile -eq $null) {
    Write-Host "Nenhum arquivo de cobertura encontrado." -ForegroundColor Red
    exit 1
}

# Pasta de saída do relatório
$outputDir = ".\coverage-report"

# Executa o ReportGenerator
reportgenerator `
    -reports:$coverageFile.FullName `
    -targetdir:$outputDir `
    -reporttypes:Html

Write-Host ""
Write-Host "Relatorio gerado em: $outputDir\index.html" -ForegroundColor Green