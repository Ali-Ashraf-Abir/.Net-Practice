# GameStore API

A RESTful Web API built with **ASP.NET Core (.NET 9)** for managing a game catalog. This project documents a learning journey through modern .NET backend development.

---

## Concepts Covered So Far

### 1. DTOs (Data Transfer Objects)
- Separating **read models** (`GameDto`) from **write models** (`CreateGameDto`)
- Using C# **record types** for immutable, value-based DTOs
- `with` expressions to clone records with modifications (e.g. `updatedGame with { Id = id }`)

### 2. Minimal APIs & Routing
- `WebApplication.MapGet/Post/Put/Delete` for defining HTTP endpoints
- **Route groups** with `app.MapGroup("/games")` to avoid repeating prefixes
- Named routes with `.WithName(...)` for generating location URIs
- Route parameters (`/{id}`) and how they bind to handler arguments

### 3. HTTP Result Helpers
| Method | HTTP Status | Use Case |
|---|---|---|
| `Results.Ok(data)` | 200 | Return resource |
| `Results.Created(uri, data)` | 201 | New resource created |
| `Results.NotFound()` | 404 | Resource doesn't exist |
| `Results.NoContent()` | 204 | Delete succeeded |

### 4. Validation
- Registered `builder.Services.AddValidation()` for input validation support

### 5. In-Memory Data Store
- Static `List<GameDto>` as a temporary data store
- `List.Find`, `FirstOrDefault`, `IndexOf`, `Remove` for CRUD operations

---

## API Endpoints

| Method | Route | Description |
|---|---|---|
| `GET` | `/games` | Get all games |
| `GET` | `/games/{id}` | Get game by ID |
| `POST` | `/games` | Create a new game |
| `PUT` | `/games/{id}` | Update an existing game |
| `DELETE` | `/games/{id}` | Delete a game |

---
## Entity Frame work
- Explored a bit of Entity Framework DbContext and Migrations as well
---

## Migrations and Database Seeding
- Did proper database migrations, automatic database migrations on startup and database seeding
---
## Next Steps

- [ done ] **Entity Framework Core** with SQLite
- [ done ] Migrations & database seeding
- [ ] Repository pattern
- [ ] Error handling middleware
- [ ] OpenAPI / Swagger docs

---

## Tech Stack

- [.NET 10](https://dotnet.microsoft.com/)
- ASP.NET Core Minimal APIs
- C# Records & Pattern Matching
- *(Coming soon)* Entity Framework Core + SQLite