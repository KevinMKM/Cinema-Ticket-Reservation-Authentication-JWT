# Cinema Ticket Reservation Authentication (JWT)

[![Difficulty](https://img.shields.io/badge/difficulty-medium-brightgreen)]()
[![Languages](https://img.shields.io/badge/languages-C%23-informational)]()
[![Deadline](https://img.shields.io/badge/deadline-2025--12--21-critical)]()

> Implement JWT-based authentication and authorization for a cinema ticket reservation system. Users must receive a JWT containing identity, role, and scopes. Reserving tickets is available to all authenticated users, while creating screenings is **Admin-only**. Apply security best practices.

---

## Table of Contents

* [Cinema Ticket Reservation Authentication (JWT)](#cinema-ticket-reservation-authentication-jwt)

  * [Table of Contents](#table-of-contents)
  * [Requirements](#requirements)
  * [Problem Description](#problem-description)
  * [Rules and Constraints](#rules-and-constraints)
  * [Core Scenarios](#core-scenarios)
  * [Suggested API Contract](#suggested-api-contract)
  * [Input/Output and Examples](#inputoutput-and-examples)
  * [How to Run and Test](#how-to-run-and-test)
  * [How to Submit (PR)](#how-to-submit-pr)
  * [Evaluation Criteria](#evaluation-criteria)
  * [Timeline](#timeline)
  * [Contact](#contact)

---

## Requirements

* Allowed Language: **C#**
* Recommended Versions: `.NET 6+`
* API Style: REST (ASP.NET Core Web API recommended)
* Token Type: **JWT Access Token**
* Authorization Model:

  * Roles: `User`, `Admin`
  * Scopes: required in JWT (e.g., `tickets:reserve`, `screenings:write`)
* Storage: any (InMemory / SQLite / SQL Server / PostgreSQL). Choose what’s easiest to run.

---

## Problem Description

You are building an **Authentication & Authorization layer** for a cinema ticket reservation system.

The system supports two key operations:

1. **Reserve Ticket** – available to **all authenticated users** (`User` and `Admin`).
2. **Add Screening** – available **only to Admins**.

After login, a user must receive a **JWT** that includes:

* Identity information (e.g., `sub` / `userId`)
* Role (e.g., `role: Admin` or `role: User`)
* Scopes (e.g., `scope: tickets:reserve screenings:write`)

Your implementation must enforce both role and scope-based access (best effort; at minimum, add-screening must require Admin).

---

## Rules and Constraints

### General

* Every protected endpoint must validate JWT properly (`exp`, `nbf`, signature, issuer, audience).
* The API must return correct HTTP status codes:

  * `401 Unauthorized` for missing/invalid token
  * `403 Forbidden` for valid token but insufficient permissions

### Authorization Rules

* **Reserve Ticket**

  * Requires authentication
  * Allowed roles: `User`, `Admin`
  * Must require scope: `tickets:reserve`

* **Add Screening**

  * Requires authentication
  * Allowed roles: `Admin` only
  * Must require scope: `screenings:write`

### JWT Claims (Minimum)

Your JWT must include at least:

* `sub` (or `userId`)
* `role`
* `scope` (space-separated) **or** `scp` (array) — document your choice
* `iat`
* `exp`
* `iss`
* `aud`

### Security Best Practices (Expected)

Implement as many as reasonable (at least the starred ones):

* ⭐ **Password hashing** (e.g., BCrypt) — never store plaintext passwords
* ⭐ **Short-lived access token** (e.g., 15–60 minutes)
* ⭐ **Strong signing key** (for HS256: long random secret; RS256 is optional)
* ⭐ **Issuer/Audience validation**
* ⭐ **No sensitive data in JWT** (no passwords, no secrets)
* ⭐ **Consistent error responses** (avoid leaking whether a username exists)
* Optional (bonus):

  * Refresh tokens (rotation + revocation)
  * Rate limiting / lockout on login
  * CORS configuration, security headers
  * Audit logging for sensitive actions

---

## Core Scenarios

### 1) User Login and JWT Issue

* User submits credentials.
* If valid, the system issues a JWT containing identity + role + scopes.

### 2) Reserve a Ticket (Authenticated Users)

* A user with a valid token can reserve a seat for a screening.
* The endpoint must reject:

  * missing/invalid token (`401`)
  * token without required scope (`403`)

### 3) Add a Screening (Admin Only)

* Only `Admin` can create a new screening.
* The endpoint must reject:

  * valid non-admin token (`403`)
  * admin token missing `screenings:write` scope (`403`)

### 4) Negative Tests

You should demonstrate at least:

* invalid/expired token → `401`
* valid user token calling admin endpoint → `403`
* token missing required scope → `403`

---

## Suggested API Contract

> You can change endpoint names if you want, but keep the same behavior.

### Auth

* `POST /api/auth/register` (optional)
* `POST /api/auth/login`

### Screenings

* `POST /api/screenings` (Admin only)
* `GET /api/screenings` (public or authenticated — your choice, document it)

### Tickets

* `POST /api/tickets/reserve` (authenticated)

---

## Input/Output and Examples

### Example: Login Request

```http
POST /api/auth/login HTTP/1.1
Content-Type: application/json

{
  "username": "alice",
  "password": "P@ssw0rd!"
}
```

### Example: Login Response

```json
{
  "accessToken": "<JWT>",
  "tokenType": "Bearer",
  "expiresInSeconds": 3600
}
```

### Example: JWT Payload (illustrative)

```json
{
  "sub": "b7b1b2a2-6a6a-4d2a-9f49-3d41e3b6d7f1",
  "role": "User",
  "scope": "tickets:reserve",
  "iss": "cinema-auth",
  "aud": "cinema-api",
  "iat": 1765670400,
  "exp": 1765674000
}
```

### Example: Reserve Ticket

```http
POST /api/tickets/reserve HTTP/1.1
Authorization: Bearer <JWT>
Content-Type: application/json

{
  "screeningId": 101,
  "seatNumber": "E12"
}
```

Response:

```json
{
  "reservationId": "c93a1d9c-3e9c-4c5b-8a59-57f0f7f9c5a2",
  "status": "Reserved"
}
```

### Example: Add Screening (Admin)

```http
POST /api/screenings HTTP/1.1
Authorization: Bearer <ADMIN_JWT>
Content-Type: application/json

{
  "movieTitle": "Interstellar",
  "startsAt": "2025-12-15T20:30:00Z",
  "hall": "Hall 1"
}
```

---

## How to Run and Test

### 1) Clone

```bash
git clone https://github.com/dotin-challenge/Cinema-Ticket-Reservation-Authentication-JWT.git
cd <repo>
```

### 2) Configure

Document your configuration in your solution folder. Typical settings:

* JWT settings: `Issuer`, `Audience`, `SigningKey`, `AccessTokenLifetimeMinutes`
* Storage settings (optional)

### 3) Build & Run

```bash
dotnet build
dotnet run
```

### 4) Tests

If you include tests:

```bash
dotnet test
```

---

## How to Submit (PR)

1. Fork the repository.

2. Create a new branch:

   ```bash
   git checkout -b solution/<username>
   ```

3. Add your solution:

   ```text
   solutions/C#/<username>/
     ├─ source code files
     └─ README.md   # optional notes about setup and design
   ```

4. Open a Pull Request with the title:

   ```text
   [Solution] Cinema Ticket JWT Auth - <username>
   ```

---

## Evaluation Criteria

| Criteria                            | Weight |
| ----------------------------------- | -----: |
| Correctness (role + scope enforced) |    35% |
| Security Best Practices             |    25% |
| Code Quality & Readability          |    20% |
| Tests / Negative Scenarios          |    10% |
| Submission Speed (PR creation time) |     5% |
| Documentation                       |     5% |

> **Submission Speed** is only evaluated if correctness and security requirements are met. Earlier valid PRs receive higher scores for this criterion.

---

## Acceptance Criteria

A submission is considered **acceptable** only if all of the following are met:

* Login endpoint successfully issues a valid JWT.
* JWT validation includes **signature**, **expiration**, **issuer**, and **audience** checks.
* JWT contains identity, role, and scopes.
* Accessing protected endpoints without a token returns `401 Unauthorized`.
* Accessing Admin-only endpoints with a non-admin token returns `403 Forbidden`.
* Missing required scopes results in `403 Forbidden`.
* Passwords are never stored or logged in plaintext.
* Token signing key is not hardcoded in source code.
* Error responses do not leak sensitive authentication details.

---

## Timeline

* **Start:** `2025-12-14`
* **PR Submission Deadline:** `2025-12-21`

---

## Contact

* .NET / Backend Community Group
* Or open an **Issue** in this repository for questions.
