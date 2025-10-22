# generate-report.ps1
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