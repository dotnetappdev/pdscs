# Release Notes

## v1.0.0 (Initial Release)

### Features & Improvements
- Modern CRUD web application for managing Departments and Persons.
- Built with:
  - **.NET 9** (ASP.NET Core Web API)
  - **Angular 17+** frontend
  - **Entity Framework Core** with InMemory database for development/testing
  - **xUnit** for backend unit testing
  - **FluentValidation** for robust backend validation
- Clean OOP design and Web API best practices.
- Full CRUD for Departments and Persons (add, edit, delete, list).
- End-to-end validation with error display (frontend and backend).
- Accessibility and semantic HTML throughout (WCAG 2.1 AA compliant, ARIA roles, keyboard navigation).
- Departments and Persons management UI:
  - Filters above tables for both Persons and Departments, always visible and accessible.
  - "Add Person" and "Add Department" buttons above their respective tables for improved usability.
  - Action buttons in Departments table are compact, responsive, and accessible.
  - Pagination for Departments table matches Persons table (controls below table, responsive, accessible, default page size 5).
  - Persistent filters and error messages for both tables.
  - Close (×) button for forms and modals.
  - Markup and structure fixes for valid HTML and accessibility.
  - Responsive layout for desktop, tablet, and mobile.
- Toast notifications for user feedback.
- Comprehensive test suite (xUnit for backend, Jasmine/Karma for frontend).

### Bug Fixes
- Fixed table and form markup errors in Departments component.
- Fixed disappearing filters and tables when no data is present.
- Fixed alignment and structure of filter and action rows.
- Fixed HTML structure and accessibility issues.

### Notes
- See README.md for usage, accessibility, and test instructions.
- All changes tested for accessibility and responsive layout.
- For test details, see README.tests.md.

---
