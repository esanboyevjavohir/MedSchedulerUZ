# MedSchedulerUZ — Backend

> A comprehensive hospital staff scheduling and workforce management system built with ASP.NET Core (.NET 8) and PostgreSQL, following Clean Architecture principles.

---

## 📋 About the Project

**MedSchedulerUZ** is a production-ready REST API designed for hospitals and clinics in Uzbekistan. The platform enables multi-level management of doctor schedules, shift assignments, attendance tracking via QR codes, leave requests, and staff certifications — all under a role-based access control system.

This repository contains the **backend (API)** of the project. The frontend is available here: [MedSchedulerUZ-Client](https://github.com/esanboyevjavohir/MedSchedulerUZ-Client)

---

## ✨ Key Features

- 🔐 **JWT Authentication** with Refresh Token support and role-based authorization (4 roles)
- 🏥 **Multi-level Hierarchy** — SuperAdmin → HospitalAdmin → DeptHead → Employee
- 📅 **Schedule Management** — create weekly schedules with Draft / Published / Archived lifecycle
- ⚡ **Auto-generate Shifts** — automatically assign shifts for an entire week by department
- 📲 **QR Code Attendance** — each shift has a unique QR token; employees clock in/out by scanning
- 🔄 **Shift Swap Requests** — employees can request shift swaps, approved by management
- 🏖️ **Leave Request System** — submit and approve/reject leave requests (Sick, Vacation, etc.)
- 🎓 **Certification Tracking** — track staff certifications with automatic expiry notifications
- 🔔 **Notification System** — in-app notifications for key events
- 🔒 **MustChangePassword Middleware** — forces first-login password change
- ⏰ **Background Service** — daily automated check for expiring certifications
- 📧 **Email Service** — OTP codes and password reset via email
- ✅ **FluentValidation** — request validation on all user inputs

---

## 🏗️ Architecture

The project follows **Clean Architecture** with 4 layers:

```
MedSchedulerUZ/
├── MedSchedulerUZ.API/            # Presentation layer
│   ├── Controllers/               # REST API endpoints (12 controllers)
│   ├── Middlewares/               # ExceptionHandler, MustChangePassword
│   └── Program.cs
│
├── MedSchedulerUZ.Application/   # Business logic layer
│   ├── Services/Implement/        # Service implementations (12 services)
│   ├── Services/Interface/        # Service interfaces
│   ├── Services/Background/       # CertificationExpiryBackgroundService
│   ├── Models/                    # Request/Response DTOs
│   ├── MappingProfiles/           # AutoMapper profiles
│   ├── Validators/                # FluentValidation validators
│   ├── Helpers/GenerateJWT/       # JWT token generation & password hashing
│   └── Email/                     # Email service configuration
│
├── MedSchedulerUZ.Core/           # Domain layer
│   ├── Entities/                  # Domain entities (13 entities)
│   ├── Enums/                     # Domain enumerations
│   └── Common/                    # BaseEntity, IAuditedEntity
│
└── MedSchedulerUZ.DataAccess/     # Infrastructure layer
    ├── Persistence/               # DatabaseContext, AutomatedMigration
    ├── Configurations/            # EF Core entity configurations
    └── Migrations/                # Database migrations
```

---

## 👥 Role System

| Role | Access |
|---|---|
| **SuperAdmin** | Manages all hospitals and their data |
| **HospitalAdmin** | Manages their own hospital, departments, and staff |
| **DeptHead** | Manages their department's schedules and shifts |
| **Employee** | Views own schedule, submits leave requests, clocks in/out via QR |

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core (.NET 8) |
| Language | C# |
| Database | PostgreSQL |
| ORM | Entity Framework Core |
| Authentication | JWT + Refresh Tokens |
| Mapping | AutoMapper |
| Validation | FluentValidation |
| API Docs | Swagger / OpenAPI |
| Email | SMTP Email Service |
| Background Jobs | IHostedService (BackgroundService) |

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

# Configure appsettings.json
# Set your PostgreSQL connection string:
# "ConnectionStrings": {
#   "DefaultConnection": "Host=localhost;Database=MedSchedulerUZ;Username=...;Password=..."
# }

# Apply migrations (auto-runs on startup via AutomatedMigration)
dotnet ef database update --project src/MedSchedulerUZ.DataAccess

# Run the project
dotnet run --project src/MedSchedulerUZ.API
```

Swagger UI will be available at: `https://localhost:5001/swagger`

---

## 📸 Screenshots

### 📋 Backend Swagger API Documentation
<img width="1034" height="912" alt="image" src="https://github.com/user-attachments/assets/529caf40-d379-4a95-9b0e-bf510d85496a" />

<img width="901" height="912" alt="image" src="https://github.com/user-attachments/assets/43230eae-ba2e-4277-ba56-bea14979aef3" />

<img width="994" height="882" alt="image" src="https://github.com/user-attachments/assets/189a9720-d68a-4510-815d-fb50ab1d7d71" />

<img width="1450" height="898" alt="image" src="https://github.com/user-attachments/assets/a3fb7004-2837-437b-a534-f81a72392c2c" />

---

## 🔗 Related

- 🖥️ Frontend Repository: [MedSchedulerUZ-Client](https://github.com/esanboyevjavohir/MedSchedulerUZ-Client)

---

## 👤 Author

**Esanboyev Javohir**  
GitHub: [@esanboyevjavohir](https://github.com/esanboyevjavohir)
