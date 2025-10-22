# push-to-repo.ps1
Write-Host "`nVerificando informações do repositório..."

# Obtém a branch atual
$branch = git rev-parse --abbrev-ref HEAD

# Obtém o usuário configurado
$usuario = git config user.name
$email = git config user.email

# Obtém a URL do repositório remoto
$remoteUrl = git remote get-url origin

Write-Host "Repositório: $remoteUrl"
Write-Host "Branch atual: $branch"
Write-Host "Usuário Git: $usuario <$email>"

# Sugere tipos de commit
Write-Host "`nTipos de commit (Conventional Commits):"
Write-Host "  feat     → nova funcionalidade"
Write-Host "  fix      → correção de bug"
Write-Host "  docs     → documentação"
Write-Host "  style    → formatação (sem mudança de lógica)"
Write-Host "  refactor → refatoração de código"
Write-Host "  test     → testes adicionados ou modificados"
Write-Host "  chore    → tarefas de manutenção"
Write-Host "  ci       → mudanças no pipeline CI/CD"
Write-Host "  build    → mudanças em dependências ou build"
Write-Host "  revert   → reversão de commit"

# Solicita tipo e descrição
$tipo = Read-Host "`nDigite o tipo de commit (ex: feat, fix, chore)"
$descricao = Read-Host "Digite a descrição do commit"

# Monta mensagem final
$mensagem = "${tipo}: ${descricao}"

# Confirma antes de enviar
Write-Host "`nMensagem de commit: $mensagem"
$confirm = Read-Host "Deseja continuar com o push? (s/n)"
if ($confirm -ne "s") {
    Write-Host "Operação cancelada."
    exit
}

# Verifica se há mudanças
$changes = git status --porcelain

if ($changes) {
    git add .
    git commit -m "$mensagem"
    git push origin $branch

    Write-Host "`nArquivos enviados com sucesso para a branch $branch." -ForegroundColor Green
} else {
    Write-Host "`nNenhuma alteração detectada. Nada para enviar." -ForegroundColor Yellow
}