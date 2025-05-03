# AppRommanel.Teste

Projeto desenvolvido como teste técnico para a empresa **Rommanel**. O sistema realiza o **CRUD de Clientes** utilizando Blazor no front-end e arquitetura moderna no back-end.

## 🛠️ Tecnologias Utilizadas

- **Front-end**: Blazor WebAssembly
- **Back-end**: ASP.NET Core 8.0 (API)
- **Banco de dados**: SQL Server (externo)
- **Arquitetura**: DDD, CQRS, Event Sourcing (mínimo viável)
- **Validações**: FluentValidation
- **Testes unitários**: xUnit + Moq
- **Containerização**: Docker e Docker Compose

---

## 🔧 Rodando com Docker

### Pré-requisitos
- Docker instalado na máquina

### Passos:

```bash
git clone https://github.com/diogocorte19932607/AppRommanel.Teste.git
cd AppRommanel.Teste
docker-compose up --build
```

Acesse a API em: [http://localhost:5000](http://localhost:5000)

---

## ⚙️ Funcionalidades

- Cadastro de cliente com:
  - Nome / Razão Social
  - CPF / CNPJ
  - Data de nascimento
  - Telefone
  - E-mail
  - Endereço completo
  - Tipo de Pessoa (Física/Jurídica)
  - Inscrição Estadual (ou isenção)

---

## 🔍 Regras de Negócio

- CPF/CNPJ e e-mail únicos
- Pessoa Física: mínimo 18 anos
- Pessoa Jurídica: IE obrigatório ou isento
- Validações no front e no back

---

## 🧪 Testes

- Executar os testes com:

```bash
dotnet test Rommanel.Infra.Tests
```

---

## 📝 Observações

- Blazor foi utilizado no lugar de Angular por decisão técnica.
- Projeto com foco em legibilidade, arquitetura limpa e boas práticas.