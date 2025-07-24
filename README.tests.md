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

## Example xUnit Tests (Backend)

| Test Name                                      | Description                                      |
|------------------------------------------------|--------------------------------------------------|
| AddPerson_WithValidPerson_ReturnsSuccessAndPerson | Adds a valid person and checks result            |
| AddPerson_WithNullPerson_ReturnsBadRequest     | Ensures null person returns BadRequest           |
| DeletePerson_WithValidId_RemovesPerson         | Deletes a person and checks removal              |
| UpdatePerson_WithValidData_UpdatesPerson       | Updates a person and checks new values           |
| Jasmine: deletes a person and removes their row from the table | Verifies person row is removed from table after delete |
| Jasmine: updates a person and reflects the change in the table | Verifies person row is updated in table after update |
| AddDepartment_WithValidDepartment_ReturnsSuccessAndDepartment | Adds a valid department and checks result      |
| AddDepartment_WithNullDepartmentName_FailsValidation | Ensures null department name fails validation |
| DeleteDepartment_WithValidId_RemovesDepartment | Deletes a department and checks removal          |
| UpdateDepartment_WithValidData_UpdatesDepartment | Updates a department and checks new values      |
| Jasmine: deletes a department and removes its row from the table | Verifies department row is removed from table after delete |
| Jasmine: updates a department and reflects the change in the table | Verifies department row is updated in table after update |

## Example Jasmine Tests (Frontend)

| Test File                        | Description                                      |
|----------------------------------|--------------------------------------------------|
| persons.component.spec.ts        | Renders persons table, checks CRUD UI, error display |
| department.component.spec.ts     | Renders departments table, checks CRUD UI, error display |
| department.service.spec.ts       | Tests API calls for department service            |
| person.service.spec.ts           | Tests API calls for person service                |

> For a full list and live results, see the [GitHub Actions test runs](https://github.com/<OWNER>/<REPO>/actions).

## How to Run
```sh
dotnet test           # Run backend xUnit tests
cd UKParliament.CodeTest.Web/ClientApp
ng test               # Run frontend Jasmine/Karma tests
```

---

See the main [README](./README.md) for more details.
