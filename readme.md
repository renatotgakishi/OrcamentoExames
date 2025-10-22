

# 🩺 Orçamento Médico

**Orçamento Médico** é uma solução cloud desenvolvida em .NET 9.0 para gestão de pedidos médicos, orçamentos, uploads de exames e autenticação de usuários. A aplicação é distribuída em múltiplos serviços e integra com **AWS S3** e **AWS SQS**, além de possuir cobertura de testes automatizados e pipeline CI/CD.

---

## 🧱 Estrutura do Projeto


orcamento-medico/ 
├── src/ 
│   ├── OrcamentoMedico.API/           
# API principal 
│   ├── OrcamentoMedico.Application/   
# Regras de negócio e interfaces 
│   ├── OrcamentoMedico.Auth/          
# Autenticação │   
├── OrcamentoMedico.Domain/        
# Entidades e eventos 
│   ├── OrcamentoMedico.Infrastructure/
# Persistência e AWS 
│   └── OrcamentoMedico.Worker/        
# Consumers de fila (SQS) 
├── tests/ │   
└── OrcamentoMedico.Tests/         
# Testes unitários e de integração 
├── scripts/ 
│   └── docker-compose.dev.yml         
# Infraestrutura local 
├── .github/workflows/ 
│   └── ci.yml                         
# Pipeline CI/CD 
├── run-tests.ps1                      
# Executa testes com cobertura 
├── generate-report.ps1               
# Gera relatório HTML de cobertura 
├── runAll-tests.ps1                  
# Executa testes + gera relatório


---

## 🚀 Como rodar localmente

### 1. Subir infraestrutura com Docker

```bash
docker-compose -f scripts/docker-compose.dev.yml up -d

Isso sobe banco de dados, serviços auxiliares e dependências locais.

2. Executar testes com cobertura

.\runAll-tests.ps1

Roda os testes e gera o relatório em coverage-report/index.html.

🧪 Testes e Cobertura

Framework: xUnit

Cobertura: coverlet.collector + ReportGenerator

Relatório gerado em HTML com métricas por classe, método e linha

☁️ Integração com AWS

Serviços utilizados:

S3: armazenamento de exames e arquivos

SQS: filas para processamento assíncrono de pedidos e e-mails

Comandos úteis:

# Criar bucket S3
aws s3 mb s3://exames-medicos

# Criar fila SQS
aws sqs create-queue --queue-name pedidos-criados

As credenciais e configurações AWS devem estar definidas via AWS CLI ou variáveis de ambiente.

🧠 Análise de Qualidade com SonarQube

Este projeto utiliza SonarQube para análise contínua de qualidade de código, incluindo:

Cobertura de testes

Duplicação de código

Complexidade ciclomática

Bugs e vulnerabilidades

Code

🔄 CI/CD

Pipeline definido em .github/workflows/ci.yml:

Build e testes automatizados

Coleta de cobertura com XPlat Code Coverage

Geração de artefatos e validação de qualidade

📬 Endpoints principais

POST /api/pedidos-db — Criação de pedidos

POST /api/upload — Upload de exames

POST /api/email — Envio de e-mails

POST /api/auth/login — Autenticação de usuários

🧠 Boas práticas

Separação clara entre camadas: API, Application, Domain, Infrastructure

Testes automatizados com cobertura acima de 70%

Uso de middlewares para padronização de respostas

Consumers desacoplados via SQS


---