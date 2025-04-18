# dbs2webapp
 
## Struktura
Dbs2WebApp.sln
│
├── Dbs2WebApp.Api           <-- ASP.NET Core Web API (entrypoint)
├── Dbs2WebApp.Application   <-- Application logic (use cases, interfaces)
├── Dbs2WebApp.Domain        <-- Domain models + enums
├── Dbs2WebApp.Infrastructure<-- Database context, EF Core, file storage, services


## Úvod (Stručný popis aplikace)

Aplikace představuje webový vzdělávací systém určený pro správu kurzů, kapitol a testů. Je vyvíjena jako moderní klient-server architektura s využitím REST API (ASP.NET Core) a Blazor WebAssembly frontendem.

---

Uživatelé systému jsou rozděleni do tří hlavních rolí: Student, Teacher, Admin

- Studenti si mohou prohlížet kurzy, studovat obsah kapitol a vyplňovat testy.

- Učitelé vytváří kurzy, přidávají kapitoly a sestavují testy s otázkami a možnostmi.

- Administrátoři spravují uživatele a jejich role.

Aplikace obsahuje evidenci výsledků testů a přiřazení studentů ke kurzům. Veškerá data jsou uložena v relační databázi (MsSQL) prostřednictvím Entity Framework Core (Code-First přístup).

### Zadání (Formulace problému a cílové prostředí)

Systém je navržen jako veřejný vzdělávací portál, do kterého se může jakýkoliv student zaregistrovat a zadarmo studovat. Učitelé mohou být přidáni pouze administrátorem, a jsou zaměstnanci firmy poskytující tento portál. Cílem je zefektivnit distribuci výukových materiálů a testování studentů v rámci jednotlivých kurzů.

#### Požadavky na systém:

- Uživatelé se přihlašují pomocí systému ASP.NET Core Identity.

- Každý kurz je spravován jedním učitelem a obsahuje více kapitol.

- Každá kapitola může obsahovat jeden nebo více testů.

- Test obsahuje sadu otázek, každá otázka má více odpovědí, z nichž je právě jedna správná.

- Studenti mohou absolvovat test a systém zaznamená výsledek (počet bodů, datum).

- Učitelé a administrátoři mohou spravovat kurzy, kapitoly, testy a otázky. Administrátoři mohou spravovat i ostatní uživatele. Učitelé mohou spravovat pouze svoje kurzy, kapitoly, testy, otázky a možnosti.

#### Použité technologie:

- ASP.NET Core (REST API)

- Blazor WebAssembly

- Entity Framework Core (Code-First, Microsoft SQL Server)

- AutoMapper

- ASP.NET Core Identity (Role: Student, Teacher, Admin)


## Instalace aplikace

Aplikace je postavená na technologii ASP.NET Core (REST API) a Blazor WebAssembly (frontend). Pro persistenci dat využívá EF Core s databází SQLite nebo SQL Server. Lze spustit lokálně nebo v Docker kontejneru.

---

### Lokální instalace (vývojářský režim)

#### Požadavky

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
    
- Visual Studio 2022
    
- Volitelně: Docker (pro snadný deployment)

#### Kroky pro spuštění:

```
# 1. Klonuj projekt
git clone https://github.com/TomasChobotsky/dbs2webapp.git
cd dbs2webapp

# 2. Restore NuGet balíčků
dotnet restore

# 3. Aplikuj EF Core migrace a vytvoř databázi
dotnet ef database update --project Dbs2WebApp.Infrastructure --startup-project Dbs2WebApp.Api

# 4. Spusť backend API
cd Dbs2WebApp.Api
dotnet run

# 5. Spusť frontend (pokud je oddělený)
cd ../Dbs2WebApp.Blazor
dotnet run
```

---

### Docker varianta (produkční nebo testovací prostředí)

Pokud projekt obsahuje `Dockerfile`, lze jej spustit jednoduše pomocí Docker Compose:

`docker-compose up --build`

📁 **Poznámka**: Docker image obsahuje jak API, tak frontend. Po spuštění bude dostupné na `http://localhost:5000`.

---

### Přístupové údaje pro testování

V databázi jsou při prvním spuštění (Seed Data) vytvořeni následující uživatelé:

| Role    | Email              | Heslo     |
| ------- | ------------------ | --------- |
| Admin   | admin@example.cz   | Asdf1234* |
| Učitel  | teacher@example.cz | Asdf1234* |
| Student | student@example.cz | Asdf1234* |

Po přihlášení můžeš testovat různé úrovně přístupu.