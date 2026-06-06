# MedSchedulerUZ — Backend

> A hospital/clinic schedule management system built with ASP.NET Core and PostgreSQL.

---

## 📋 About the Project

**MedSchedulerUZ** is a web-based scheduling platform designed for hospitals and clinics in Uzbekistan. It enables administrators to manage doctor schedules, track shift statuses, and publish timetables — all through a clean REST API.

This repository contains the **backend** (API) of the project. The frontend is available here: [MedSchedulerUZ-Client](https://github.com/esanboyevjavohir/MedSchedulerUZ-Client)

---

## ✨ Features

- 🔐 JWT-based authentication and role-based authorization
- 👨‍⚕️ Doctor and department management
- 📅 Schedule creation, publishing, archiving
- 🔍 Filter schedules by status (Draft, Published, Archived)
- 📄 RESTful API with full CRUD operations
- 🗄️ PostgreSQL database with Entity Framework Core

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 8) |
| Language | C# |
| Database | PostgreSQL |
| ORM | Entity Framework Core |
| Authentication | JWT (JSON Web Tokens) |
| API Docs | Swagger / OpenAPI |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/)

### Installation

```bash
# Clone the repository
git clone https://github.com/esanboyevjavohir/MedSchedulerUZ.git
cd MedSchedulerUZ

# Restore dependencies
dotnet restore

# Update database connection string in appsettings.json
# "ConnectionStrings": { "DefaultConnection": "Host=...;Database=...;Username=...;Password=..." }

# Apply migrations
dotnet ef database update

# Run the project
dotnet run
```

The API will be available at `https://localhost:5001` and Swagger UI at `https://localhost:5001/swagger`.

---

## 📁 Project Structure

```
MedSchedulerUZ/
├── src/
│   ├── Controllers/       # API endpoints
│   ├── Models/            # Entity models
│   ├── DTOs/              # Data transfer objects
│   ├── Services/          # Business logic
│   ├── Repositories/      # Data access layer
│   └── Migrations/        # EF Core migrations
├── MedSchedulerUZ.sln
└── README.md
```

---

## 📸 Screenshots

<!-- Add your screenshots here -->
<!-- Example:
![Dashboard](assets/dashboard.png)
![Schedules](assets/schedules.png)
-->

*Screenshots coming soon*

---

## 🔗 Related

- 🖥️ Frontend Repository: [MedSchedulerUZ-Client](https://github.com/esanboyevjavohir/MedSchedulerUZ-Client)

---

## 👤 Author

**Esanboyev Javohir**  
GitHub: [@esanboyevjavohir](https://github.com/esanboyevjavohir)
