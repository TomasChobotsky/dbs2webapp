# 🧠 DBS2WebApp – Online Learning System

A modern web application for managing online courses, chapters, and tests – built using **ASP.NET Core Web API** & **Blazor WebAssembly**, with full Docker support and clean architectural patterns.

> 🎓 Originally developed as part of the *Databázové systémy 2* course (University of Hradec Králové), now ready for real-world use.

---

## 🚀 Features

- ✅ **Three user roles:** Admin, Teacher, Student
- 📚 **Course & chapter management** (CRUD)
- 📝 **Test system** with question bank & scoring
- 🔐 **JWT authentication** via ASP.NET Identity
- ⚡ **REST API** with Swagger docs
- 🧱 **Clean Architecture** & Generic Repository pattern
- 🐳 **Dockerized** full-stack deployment
- 📈 Built with Entity Framework Core (code-first)

---

## 🗭 Project Structure

| Project                   | Description                                   |
|---------------------------|-----------------------------------------------|
| `dbs2webapp.Api`          | ASP.NET Core Web API, authentication, routing |
| `dbs2webapp.Client`       | Blazor WebAssembly frontend (SPA)             |
| `dbs2webapp.Application`  | DTOs, repository interfaces, service contracts|
| `dbs2webapp.Domain`       | EF Core entities & domain models              |
| `dbs2webapp.Infrastructure` | EF configuration & generic repositories     |
| `dbs2webapp.Tests`        | Basic unit tests (controller logic)           |

---

## 🐳 Getting Started (with Docker)

### 1️⃣ Prerequisites

- Docker Desktop (Windows/macOS) or Docker Engine + Compose (Linux)

### 2️⃣ Launch the app

```bash
docker-compose up --build
```

Then visit:

- Frontend: [http://localhost:3000](http://localhost:3000)
- Backend API: [http://localhost:5000](http://localhost:5000)

### 3️⃣ Default login

You can register new users via `/api/auth/register`, or seed roles manually via `UserManager`.

---

## 📷 Screenshots

| Student View | Teacher Panel |
|--------------|---------------|
| *(insert screenshots here)* | *(insert screenshots here)* |

---

## 🔧 Tech Stack

- ASP.NET Core 9, Blazor WebAssembly
- Entity Framework Core (Code-First)
- SQL Server 2022 (Docker)
- AutoMapper, JWT Bearer Auth
- Swagger / OpenAPI
- Bootstrap 5

---

## 🧪 Testing

Basic unit tests are located in `dbs2webapp.Tests`. To run:

```bash
dotnet test
```

---

## 🤛‍♂️ Author

Developed by [Tomas Chobotsky](https://github.com/TomasChobotsky)  
For academic & professional growth – feel free to fork, use, and expand 🙌

---

## 📄 License

This project is open source under the [MIT License](LICENSE).