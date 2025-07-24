# 🧪 Test Suite Overview

This file describes all unit tests for the solution, written with **xUnit** and using the **EF Core InMemory** database for isolation and speed.

## What is Tested?
- **Repositories:**
  - Add, update, and delete operations for departments and persons
  - InMemory DB context setup and teardown
- **Validation:**
  - Department and person validation (e.g., required fields, max lengths)
  - FluentValidation rules
- **Error Handling:**
  - Ensures validation errors are returned and handled as expected

## Example Tests
- Adding a department with a valid name returns success
- Adding a department with a null/empty name fails validation
- Adding a person with invalid data fails validation
- Deleting a person or department removes it from the database
- Updating a person or department changes the data as expected

## How to Run
```sh
dotnet test
```

---

See the main [README](./README.md) for more details.
