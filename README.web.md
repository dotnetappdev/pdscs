# 🌐 Web API & Angular Frontend

This project contains the ASP.NET Core Web API and Angular frontend for the CRUD app.

## Web API
- **Endpoints:**
  - `/api/department` — CRUD for departments
  - `/api/person` — CRUD for persons
- **Validation:**
  - Uses FluentValidation for all POST/PUT requests
  - Returns field-level errors for display in the frontend
- **InMemory DB:**
  - All data is stored in EF Core's InMemory provider
  - Seed data is loaded on startup

## Angular Frontend
- **Features:**
  - Responsive, accessible SPA
  - Full CRUD for departments and persons
  - Displays validation errors from backend
  - Keyboard and screen reader accessible

## Accessibility
- Semantic HTML, ARIA roles, color contrast
- All forms and tables are accessible

---

See the main [README](./README.md) for setup and usage.
