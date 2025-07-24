[![Frontend Tests](https://github.com/<OWNER>/<REPO>/actions/workflows/frontend-tests.yml/badge.svg)](https://github.com/<OWNER>/<REPO>/actions/workflows/frontend-tests.yml)
[![Backend Tests](https://github.com/<OWNER>/<REPO>/actions/workflows/backend-tests.yml/badge.svg)](https://github.com/<OWNER>/<REPO>/actions/workflows/backend-tests.yml)

<p align="center">
  <img src="https://img.shields.io/badge/.NET-9.0-purple?logo=dotnet&logoColor=white" alt=".NET 9" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-9.0-blue?logo=dotnet&logoColor=white" alt="ASP.NET Core 9" />
  <img src="https://img.shields.io/badge/Angular-17+-red?logo=angular&logoColor=white" alt="Angular 17+" />
  <!-- xUnit badge below is static. For live test status, see the Backend Tests badge above. -->
  <img src="https://img.shields.io/badge/xUnit-2.4+-orange?logo=xunit&logoColor=white" alt="xUnit 2.4+" />
  <img src="https://img.shields.io/badge/EF%20Core-InMemory-green?logo=database&logoColor=white" alt="EF Core InMemory" />
  <img src="https://img.shields.io/badge/FluentValidation-11+-blueviolet" alt="FluentValidation 11+" />
</p>

---

## 📑 Table of Contents
| Section | Link |
|---------|------|
| 🏠 Main Overview | [README.md](./README.md) |
| 🌐 Web API & Frontend | [README.web.md](./README.web.md) |
| 🧪 Test Suite | [README.tests.md](./README.tests.md) |

---

## Overview

This project is a modern CRUD web application built with:
- **.NET 9** (ASP.NET Core Web API)
- **Angular** (frontend)
- **Entity Framework Core** with InMemory database for development/testing
- **xUnit** for unit testing
- **FluentValidation** for robust backend validation

The app demonstrates:
- Clean OOP design
- Web API best practices
- Full CRUD for Departments and Persons
- End-to-end validation with error display
- Accessibility and semantic HTML
- Compliance with accessibility standards (WCAG/ARIA)

---

## 🚀 Web Services

### Department API
- `GET /api/department` — List all departments
- `POST /api/department` — Add a department (validates name)
- `PUT /api/department/{id}` — Update a department
- `DELETE /api/department/{id}` — Delete a department

### Person API
- `GET /api/person` — List all persons
- `POST /api/person` — Add a person (validates fields)
- `PUT /api/person/{id}` — Update a person
- `DELETE /api/person/{id}` — Delete a person

---

## 🧑‍💻 Main Features & OOP
- **OOP**: All business logic is encapsulated in services and repositories.
- **Validation**: FluentValidation is used for both persons and departments. Errors are returned as field-level errors and displayed in the UI.
- **InMemory DB**: The app uses EF Core's InMemory provider for easy local development and testing. Seed data is loaded on startup.
- **Accessibility**: The Angular frontend uses semantic HTML, ARIA roles, and color contrast for accessibility. Forms and tables are keyboard-friendly.
- **SPA**: The Angular app is a single-page application with responsive design.

---

## 🧪 Tests
- **xUnit** unit tests for repositories and validation logic
- **Angular Frontend Unit Tests**: Jasmine/Karma tests for components and services
- Tests cover:
  - Adding, updating, and deleting departments/persons
  - Validation failures (e.g., missing department name)
  - InMemory DB context setup
  - Angular component rendering and service logic

See [`README.tests.md`](./README.tests.md) for backend test details.

### ▶️ Running Frontend Tests
To run Angular (frontend) unit tests with Jasmine/Karma:
```sh
cd UKParliament.CodeTest.Web/ClientApp
ng test
```
This will launch the test runner and execute all `.spec.ts` files using Jasmine.

---

## ⚙️ Versions
- **.NET**: 9.0
- **ASP.NET Core**: 9.0
- **Angular**: 17+
- **EF Core**: 9.0 (InMemory)
- **xUnit**: 2.4+
- **FluentValidation**: 11+

---

## ♿ Accessibility
- All forms and tables use semantic HTML
- Color contrast and focus indicators
- ARIA labels and roles where appropriate
- Keyboard navigation supported

---

## 📂 Project Structure
- `Web/` — ASP.NET Core Web API + Angular frontend
- `Data/` — EF Core models, context, seed data
- `Services/` — Business logic, validation, repositories
- `Tests/` — xUnit unit tests

---

## 🔗 Related Docs
- [Test Suite Details](./README.tests.md)
- [Web API & Frontend Details](./README.web.md)

---

## 📝 How to Run
1. **Restore & Build:**
   ```sh
   dotnet restore
   dotnet build
   ```
2. **Run the API & Angular app:**
   ```sh
   dotnet run --project UKParliament.CodeTest.Web
   ```
3. **Run backend tests:**
   ```sh
   dotnet test
   ```
4. **Run frontend (Angular) tests:**
   ```sh
   cd UKParliament.CodeTest.Web/ClientApp
   ng test
   ```

---

## 🗄️ InMemory Database
- The app uses EF Core's InMemory provider for all CRUD operations and tests.
- Seed data is loaded on startup for both departments and persons.
- No external database setup required.

---

## 📢 Notes
- No real data is used. All data is for demonstration/testing only.

