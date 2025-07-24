# 🟣 .NET 9 | 🟦 ASP.NET Core | 🟩 Angular | 🧪 xUnit | 🗄️ InMemory DB

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
- Tests cover:
  - Adding departments/persons
  - Validation failures (e.g., missing department name)
  - InMemory DB context setup

See [`README.tests.md`](./README.tests.md) for test details.

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
3. **Run tests:**
   ```sh
   dotnet test
   ```

---

## 🗄️ InMemory Database
- The app uses EF Core's InMemory provider for all CRUD operations and tests.
- Seed data is loaded on startup for both departments and persons.
- No external database setup required.

---

## 📢 Notes
- No real data is used. All data is for demonstration/testing only.
- No references to any real-world organizations or abbreviations in code or CSS.

