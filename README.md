# CarePlus Mindfulness API 🧘

**API de Saúde Mental & Mindfulness** — Challenge Care Plus FIAP 2025  
Sprint 3 — Arquitetura Orientada a Serviços e Web Services

## 📋 Descrição do Projeto

API RESTful desenvolvida em **ASP.NET Core 8** com **Entity Framework Core** e **PostgreSQL** para gerenciar serviços digitais de saúde mental e mindfulness no aplicativo da Care Plus.

A solução permite que beneficiários registrem sessões de meditação, acompanhem seu humor diário e acessem conteúdos de bem-estar, promovendo prevenção e qualidade de vida de forma inovadora — sem utilizar abordagens tradicionais como telemedicina ou diagnósticos clínicos.

### Propósito (alinhado à Care Plus)
> "Ajudar as pessoas a viverem vidas mais longas, saudáveis e felizes, e criar um mundo melhor."

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Versão | Finalidade |
|---|---|---|
| .NET | 8.0 | Framework principal |
| ASP.NET Core Web API | 8.0 | Framework web REST |
| Entity Framework Core | 8.0.8 | ORM para acesso a dados |
| Npgsql | 8.0.4 | Provider PostgreSQL para EF Core |
| PostgreSQL | 16+ | Banco de dados relacional |
| Swagger / Swashbuckle | 6.5.0 | Documentação interativa da API |

---

## 📁 Estrutura do Projeto

```
CarePlus.MindfulnessAPI/
├── Controllers/                    # Camada de apresentação (endpoints)
│   ├── UsersController.cs
│   ├── MeditationSessionsController.cs
│   ├── MoodEntriesController.cs
│   └── WellnessContentsController.cs
├── Services/                       # Camada de lógica de negócio
│   ├── Interfaces/
│   │   └── IServices.cs
│   ├── UserService.cs
│   ├── MeditationSessionService.cs
│   ├── MoodEntryService.cs
│   └── WellnessContentService.cs
├── Repositories/                   # Camada de acesso a dados
│   ├── Interfaces/
│   │   └── IRepositories.cs
│   ├── UserRepository.cs
│   ├── MeditationSessionRepository.cs
│   ├── MoodEntryRepository.cs
│   └── WellnessContentRepository.cs
├── Models/
│   ├── Entities/                   # Entidades do domínio
│   │   ├── User.cs
│   │   ├── MeditationSession.cs
│   │   ├── MoodEntry.cs
│   │   └── WellnessContent.cs
│   ├── DTOs/                       # Data Transfer Objects
│   │   └── DTOs.cs
│   └── Enums/
│       └── Enums.cs
├── Data/
│   └── AppDbContext.cs             # Contexto do EF Core + Seed Data
├── Middleware/
│   └── GlobalExceptionMiddleware.cs # Tratamento global de erros
├── Program.cs                      # Configuração e DI
├── appsettings.json                # Connection string e configs
└── CarePlus.MindfulnessAPI.csproj  # Dependências NuGet
```

---

## ⚙️ Passos de Configuração e Execução

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/)

### 1. Clonar o repositório
```bash
git clone https://github.com/SEU_USUARIO/CarePlus.MindfulnessAPI.git
cd CarePlus.MindfulnessAPI
```

### 2. Configurar o banco de dados
Edite o arquivo `appsettings.json` com suas credenciais do PostgreSQL:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=careplus_mindfulness;Username=postgres;Password=SUA_SENHA"
  }
}
```

### 3. Instalar dependências e criar o banco
```bash
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Executar a aplicação
```bash
dotnet run
```

A API estará disponível em: `https://localhost:5001` (ou `http://localhost:5000`)

### 5. Acessar o Swagger
Abra no navegador: `https://localhost:5001/swagger`

---

## 📡 Endpoints da API

### Users (Usuários)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/users` | Lista todos os usuários |
| GET | `/api/users/{id}` | Busca usuário por ID |
| POST | `/api/users` | Cadastra novo usuário |
| PUT | `/api/users/{id}` | Atualiza um usuário |
| DELETE | `/api/users/{id}` | Remove um usuário |

### MeditationSessions (Sessões de Meditação)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/meditationsessions` | Lista todas as sessões |
| GET | `/api/meditationsessions/{id}` | Busca sessão por ID |
| GET | `/api/meditationsessions/user/{userId}` | Sessões de um usuário |
| POST | `/api/meditationsessions` | Registra nova sessão |
| PUT | `/api/meditationsessions/{id}` | Atualiza uma sessão |
| DELETE | `/api/meditationsessions/{id}` | Remove uma sessão |

### MoodEntries (Registro de Humor)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/moodentries` | Lista todos os registros |
| GET | `/api/moodentries/{id}` | Busca registro por ID |
| GET | `/api/moodentries/user/{userId}` | Registros de um usuário |
| POST | `/api/moodentries` | Registra novo humor |
| PUT | `/api/moodentries/{id}` | Atualiza um registro |
| DELETE | `/api/moodentries/{id}` | Remove um registro |

### WellnessContents (Conteúdos de Bem-Estar)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/wellnesscontents` | Lista todos os conteúdos |
| GET | `/api/wellnesscontents/active` | Lista conteúdos ativos |
| GET | `/api/wellnesscontents/{id}` | Busca conteúdo por ID |
| POST | `/api/wellnesscontents` | Cadastra novo conteúdo |
| PUT | `/api/wellnesscontents/{id}` | Atualiza um conteúdo |
| DELETE | `/api/wellnesscontents/{id}` | Remove um conteúdo |

---

## 📝 Exemplos de Requisições e Respostas

### POST /api/users — Criar usuário
**Request:**
```json
{
  "nome": "Ana Costa",
  "email": "ana.costa@email.com",
  "dataNascimento": "1995-03-20"
}
```
**Response (201 Created):**
```json
{
  "sucesso": true,
  "mensagem": "Usuário criado com sucesso.",
  "dados": {
    "id": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
    "nome": "Ana Costa",
    "email": "ana.costa@email.com",
    "dataNascimento": "1995-03-20",
    "criadoEm": "2025-08-15T14:30:00Z",
    "totalSessions": 0,
    "totalMoodEntries": 0
  }
}
```

### POST /api/meditationsessions — Registrar sessão
**Request:**
```json
{
  "userId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "tipo": "Meditacao",
  "titulo": "Mindfulness Matinal",
  "duracaoMinutos": 15,
  "observacoes": "Sessão focada em respiração consciente."
}
```
**Response (201 Created):**
```json
{
  "sucesso": true,
  "mensagem": "Sessão criada com sucesso.",
  "dados": {
    "id": "...",
    "userId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "nomeUsuario": "Maria Silva",
    "tipo": "Meditacao",
    "titulo": "Mindfulness Matinal",
    "duracaoMinutos": 15,
    "concluida": false,
    "observacoes": "Sessão focada em respiração consciente.",
    "realizadaEm": "2025-08-15T14:35:00Z"
  }
}
```

### POST /api/moodentries — Registrar humor
**Request:**
```json
{
  "userId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "nivelHumor": "Bom",
  "notas": "Dia produtivo, me senti mais calmo após a meditação."
}
```

### Resposta de Erro (400 Bad Request)
```json
{
  "sucesso": false,
  "mensagem": "Usuário com ID 'xxx' não encontrado.",
  "erros": null
}
```

---

## 🏗️ Arquitetura

O projeto segue a arquitetura em camadas com separação clara de responsabilidades:

```
[Client/Swagger] → [Controllers] → [Services] → [Repositories] → [PostgreSQL]
```

- **Controllers**: Recebem requisições HTTP, validam parâmetros, retornam `ResponseEntity` padronizado
- **Services**: Contêm a lógica de negócio, validações e mapeamento Entity ↔ DTO
- **Repositories**: Abstração do acesso a dados via EF Core
- **Entities**: Modelos de domínio mapeados para tabelas
- **DTOs**: Objetos de transferência (Create, Update, Response) que isolam a API do modelo de dados

### Padrões e Boas Práticas
- Injeção de Dependência (DI) via `Program.cs`
- Interfaces para todas as camadas (inversão de dependência)
- DTOs com `record` types (imutáveis e concisos)
- Tratamento de erros global via `Middleware`
- Enums armazenados como `string` no banco (legibilidade)
- Seed data para desenvolvimento/testes
- `AsNoTracking()` em consultas de leitura (performance)

---

## 👤 Autor

Diana Letícia de Souza Inocencio -  RM553562
João Viktor Carvalho de Souza - RM552613
Lucas Reis Diniz - RM552838
Thiago Araújo Vieira - RM553477
Victor Augusto Pereira dos Santos -  RM553518
Vitor de Moura Nascimento - RM553806  

Turma: Engenharia de Software — 3º Ano  
FIAP — 2025/ago

---

## 📄 Licença

Este projeto é parte do Challenge acadêmico Care Plus — FIAP 2025.
