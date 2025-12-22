# 📽️ **Cinema Reservation System - Clean Architecture JWT Auth**

[![.NET](https://github.com/yourusername/CinemaReservation.CleanArch/actions/workflows/dotnet.yml/badge.svg)](https://github.com/yourusername/CinemaReservation.CleanArch/actions/workflows/dotnet.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Docker](https://img.shields.io/badge/Docker-%2300D1FF.svg?&logo=docker&logoColor=white)](https://hub.docker.com/)

## ✨ **ویژگی‌های کلیدی**

| ✅ **امنیت کامل** | ✅ **Clean Architecture** | ✅ **Scope + Role Auth** |
|---|---|---|
| 🔐 JWT + BCrypt Password | 🏗️ 5 Layer Architecture | 🎭 Role: User/Admin |
| 🔑 Secure Key (appsettings) | 📊 Domain-Driven Design | 📋 Scopes: `ticket:reserve`, `screening:create` |
| ✅ Issuer/Audience Validation | 🔌 Dependency Injection | 🛡️ Custom Policy Handler |
| ⏱️ Token Lifetime: 30min | 🧪 100% Testable | 🧪 Swagger + JWT Support |

## 🏗️ **معماری پروژه (Clean Architecture)**
CinemaReservation.CleanArch/

├── src/

│ ├── Domain/ # Entities + Interfaces (1%)

│ ├── Application/ # UseCases + DTOs (15%)

│ ├── Infrastructure/ # EF + JWT + BCrypt (30%)

│ └── API/ # Controllers + Policies (54%)

├── tests/

└── docker-compose.yml

🚀 راه‌اندازی سریع
پیش‌نیازها
bash

.NET 8.0+ SDK

Docker (اختیاری)

SQLite (توسعه)

1. Clone & Restore
bash

git clone https://github.com/yourusername/CinemaReservation.CleanArch.git

cd CinemaReservation.CleanArch

dotnet restore

2. تنظیمات (appsettings.json)
json

{

“Jwt”: {

“Secret”: “your-super-secure-jwt-key-minimum-32-chars-long-very-secure!!!”,

“Issuer”: “CinemaReservationAPI”,

“Audience”: “CinemaReservationClient”

},

“ConnectionStrings”: {

“DefaultConnection”: “Data Source=cinema.db”

}

}

3. Migration & Run
bash

Database Migration
dotnet ef migrations add InitialCreate

dotnet ef database update

Run API
dotnet run --project src/CinemaReservation.WebHost

یا Docker
docker-compose up --build

🔐 احراز هویت و مجوزدهی
کاربران پیش‌فرض
Username	Password	Role	Scopes
admin	123456	Admin	ticket:reserve, screening:create
Login
bash

curl -X POST http://localhost:5000/api/auth/login \

-H “Content-Type: application/json” \

-d ‘{“username”:“admin”,“password”:“123456”}’

پاسخ موفق:

json

{

“accessToken”: “eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9…”,

“expiresAt”: “2025-01-01T20:30:00Z”

}

🧪 تست API ها
1. رزرو بلیط (User/Admin)
bash

curl -X POST http://localhost:5000/api/tickets/reserve \

-H “Authorization: Bearer YOUR_JWT_TOKEN” \

-H “Content-Type: application/json” \

-d ‘{“screeningId”:“3fa85f64-5717-4562-b3fc-2c963f66afa6”,“seatNumber”:5}’

2. ایجاد اکران (فقط Admin)
bash

curl -X POST http://localhost:5000/api/screenings \

-H “Authorization: Bearer ADMIN_JWT_TOKEN” \

-H “Content-Type: application/json” \

-d '{

“movieTitle”: “Oppenheimer”,

“showTime”: “2025-01-01T20:00:00Z”,

“totalSeats”: 100

}’

3. تست عدم مجوز (403)
bash

User عادی → Create Screening → 403 Forbidden ✅
curl -X POST http://localhost:5000/api/screenings \

-H “Authorization: Bearer USER_JWT_TOKEN” \

-H “Content-Type: application/json” \

-d {…}

📊 Swagger Documentation
http://localhost:5000/swagger

Authorize → Bearer YOUR_JWT_TOKEN
🔍 JWT Token Structure
json

{

“sub”: “11111111-1111-1111-1111-111111111111”,

“name”: “admin”,

“role”: “Admin”,

“scope”: [“ticket:reserve”, “screening:create”],

“iss”: “CinemaReservationAPI”,

“aud”: “CinemaReservationClient”,

“exp”: 1735807800

}

🛡️ امنیت پیاده‌سازی شده
ویژگی	وضعیت	توضیح
✅ Password Hashing	BCrypt	BCrypt.Net.BCrypt.HashPassword()
✅ JWT Secret	appsettings.json	نه Hardcode
✅ Token Validation	کامل	Issuer + Audience + Signature + Lifetime
✅ Scope Auth	Custom Handler	RequireScopeHandler
✅ Role Auth	[Authorize(Roles = "Admin")]	Policy + Role
✅ HTTPS	Redirect	app.UseHttpsRedirection()
🏗️ لایه‌های Clean Architecture
mermaid

graph TB

Presentation[API Layer<br/>Controllers] --> Application[Application<br/>UseCases + DTOs]

Application --> Domain[Domain<br/>Entities + Interfaces]

Application --> Infrastructure[Infrastructure<br/>EF + JWT + BCrypt]

Infrastructure -.->|Implements| Domain

🧪 Unit Tests
bash

dotnet test --project tests/CinemaReservation.Tests

📦 Docker Deployment
Build & Run
bash

docker-compose up --build

Production Docker
dockerfile

Dockerfile.prod
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet publish -c Release -o /app/publish

FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [“dotnet”, “CinemaReservation.WebHost.dll”]

🔧 Environment Variables
متغیر	پیش‌فرض	توضیح
ASPNETCORE_ENVIRONMENT	Development	Production / Staging
Jwt__Secret	your-super-secure-key...	تغییر دهید
ConnectionStrings__DefaultConnection	cinema.db	SQLite/PostgreSQL