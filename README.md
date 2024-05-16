<p dir="auto"><img src="https://github.com/g12-4soat/tech-lanches/blob/main/src/TechLanches/Adapter/Driver/TechLanches.Adapter.API/wwwroot/SwaggerUI/images/android-chrome-192x192.png" alt="TECHLANCHES" title="TECHLANCHES" align="right" height="60" style="max-width: 100%;"></p>

# Tech Lanches Auth
Projeto Tech Challenge Fase 4

Repositório dedicado ao projeto de autenticação do TechChallenge da FIAP - Turma 4SOAT.

# Descrição

Este projeto faz parte do curso de pós-graduação em Arquitetura de Software oferecido pela FIAP. Nosso objetivo é demonstrar como implementamos os recursos AWS que são necessários para o Sign In e Sign Up do usuário na aplicação Tech Lanches.

# Documentação

<h4 tabindex="-1" dir="auto" data-react-autofocus="true">Stack</h4>

<p>
  <a target="_blank" rel="noopener noreferrer nofollow" href="https://camo.githubusercontent.com/71ae40a5c68bd66e1cb3813f84a5b71dd3c270c8f2506143d33be1c23f0b0783/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f2e4e45542d3531324244343f7374796c653d666f722d7468652d6261646765266c6f676f3d646f746e6574266c6f676f436f6c6f723d7768697465"><img src="https://camo.githubusercontent.com/71ae40a5c68bd66e1cb3813f84a5b71dd3c270c8f2506143d33be1c23f0b0783/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f2e4e45542d3531324244343f7374796c653d666f722d7468652d6261646765266c6f676f3d646f746e6574266c6f676f436f6c6f723d7768697465" data-canonical-src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&amp;logo=dotnet&amp;logoColor=white" style="max-width: 100%;"></a>
  <a target="_blank" rel="noopener noreferrer nofollow" href="https://camo.githubusercontent.com/ffd9b9f100120fd49ebdbe8064adec834a0927f7be93551d12804c85fb92a298/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f432532332d3233393132303f7374796c653d666f722d7468652d6261646765266c6f676f3d637368617270266c6f676f436f6c6f723d7768697465"><img src="https://camo.githubusercontent.com/ffd9b9f100120fd49ebdbe8064adec834a0927f7be93551d12804c85fb92a298/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f432532332d3233393132303f7374796c653d666f722d7468652d6261646765266c6f676f3d637368617270266c6f676f436f6c6f723d7768697465" data-canonical-src="https://img.shields.io/badge/CSHARP-6A5ACD.svg?style=for-the-badge&amp;logo=csharp&amp;logoColor=white" style="max-width: 100%;"></a>
</p>

<details>
  <summary>Como executar o terraform local?</summary>
  
## Executando o Projeto
O procedimento para executar o Terraform local é simples e leva poucos passos: 

1. Clone o repositório: _[https://github.com/g12-4soat/techlanches-auth](https://github.com/g12-4soat/techlanches-auth.git)_
 
1. Abra a pasta via linha de comando no diretório escolhido no **passo 1**. _Ex.: c:\> cd “c:/techlanches-auth”_

## Gerando zip das functions .NET
Da raiz do repositório, execute os seguintes comandos no terminal:

### Instalando Tools .NET AWS Lambda 
> c:\techlanches-auth> dotnet tool install -g Amazon.Lambda.Tools

### Gerando zip das functions .NET
> c:\techlanches-auth> dotnet lambda package --project-location  src/Serverless/LambdaAuth/ --output-package src/Serverless/auth_lambda.zip --configuration Release --framework net8.0


## Rodando Terraform

1. Clone o repositório: _[https://github.com/g12-4soat/techlanches-iac](https://github.com/g12-4soat/techlanches-iac.git)_
 
1. Da raiz do repositório, entre no diretório ./src/terraform-api-gateway (onde se encontram todos os scripts Terraform necessários para este projeto), e execute os seguintes comandos no terminal:

### Iniciando o Terraform 
> c:\techlanches-iac/src/terraform-api-gateway> terraform init

### Validando script Terraform
> c:\techlanches-iac/src/terraform-api-gateway> terraform validate

### Verificando plano de implantação do script 
> c:\techlanches-iac/src/terraform-api-gateway> terraform plan

### Aplicando plano de implantação do script 
> c:\techlanches-iac/src/terraform-api-gateway> terraform apply

## Postman 
Para importar as collections do postman, basta acessar os links a seguir:
- Collection: https://github.com/g12-4soat/techlanches-auth/blob/main/docs/fase3/LambdaTechLanches.postman_collection.json
- Local Environment: https://github.com/g12-4soat/techlanches-auth/blob/main/docs/fase3/TechLanchesGateway.postman_environment.json

> Quando uma nova instância do API Gateway é criada, uma nova URL é gerada, exigindo a atualização manual da URL na Enviroment do Postman.
  ---


</details>

<details>
  <summary>Versões</summary>

## Software
- C-Sharp - 10.0
- .NET - 8.0
</details>

---
# Dependências
- [AWS Tools .NET](https://aws.amazon.com/pt/sdk-for-net/)

## Métricas Sonar

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=g12-4soat_techlanches-auth&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=g12-4soat_techlanches-auth)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=g12-4soat_techlanches-auth&metric=coverage)](https://sonarcloud.io/summary/new_code?id=g12-4soat_techlanches-auth)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=g12-4soat_techlanches-auth&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=g12-4soat_techlanches-auth)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=g12-4soat_techlanches-auth&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=g12-4soat_techlanches-auth)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=g12-4soat_techlanches-auth&metric=bugs)](https://sonarcloud.io/summary/new_code?id=g12-4soat_techlanches-auth)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=g12-4soat_techlanches-auth&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=g12-4soat_techlanches-auth)

---

## Pipeline Status
| Pipeline | Status |
| --- | --- | 
| Pipeline techlanches-auth | [![TechLanches API](https://github.com/g12-4soat/techlanches-auth/actions/workflows/pipeline.yml/badge.svg)](https://github.com/g12-4soat/techlanches-auth/actions/workflows/pipeline.yml)

---

