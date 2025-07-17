# 🏋️‍♂️ Controle de Acesso - Clube

Este projeto tem como objetivo demonstrar na prática a aplicação de conceitos de **C#**, **.NET Core**, **Entity Framework Core (Code-First)**, boas práticas de **arquitetura limpa**, separação de responsabilidades e testes automatizados.

---

## 🧪 Desafio

Construção de uma **API REST** para controle de acesso a áreas de um clube (piscina, academia, quadras etc.). A API permite o gerenciamento de sócios, planos e áreas, além de registrar tentativas de acesso.

---

## ✅ Funcionalidades Implementadas

- Cadastro e consulta de:
  - Sócios
  - Planos de acesso
  - Áreas do clube
- Registro de tentativas de acesso
- Regras de negócio aplicadas:
  - Um sócio só pode acessar as áreas permitidas no seu plano
  - Cada tentativa de acesso registra:
    - ID do sócio
    - ID da área
    - Data e hora
    - Resultado (Autorizado ou Negado)

---

## 🧱 Arquitetura e Boas Práticas

- Separação por **camadas (API / Aplicação / Domínio / Infraestrutura)**
- Injeção de dependência (DI)
- Repositórios com interfaces
- `UseCases` separados da API (seguindo Clean Architecture)
- DTOs e AutoMapper para desacoplamento
- Validações com FluentValidation
- Entity Framework Core com **Code First**

---

## 🔍 Testes Automatizados

- Utilizado **XUnit** e **Moq**
- Testes unitários para validar a **regra de acesso**:
  - Leitura de cenários via arquivos JSON
  - Simulação de repositórios com mocks
  - Verificação de exceções e respostas autorizadas/negadas
- Geração de um log com as tentativas testadas (`tentativas_acesso.json`)

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- (Opcional) Visual Studio 2022 ou VS Code

### Passo a passo

1. Clone o repositório:

   ```bash
   git clone https://github.com/seu-usuario/controle-acesso-clube.git
   cd controle-acesso-clube
