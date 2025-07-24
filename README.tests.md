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
| GetAllPersonsAsync_ReturnsAllPersons           | Verifies async retrieval of all persons          |
| AddPersonAsync_AddsPersonSuccessfully          | Verifies async add method for person             |
| UpdatePersonAsync_UpdatesPersonSuccessfully    | Verifies async update method for person          |
| DeletePersonAsync_DeletesPersonSuccessfully    | Verifies async delete method for person          |
| AddDepartment_WithValidDepartment_ReturnsSuccessAndDepartment | Adds a valid department and checks result      |
| AddDepartment_WithNullDepartmentName_FailsValidation | Ensures null department name fails validation |
| DeleteDepartment_WithValidId_RemovesDepartment | Deletes a department and checks removal          |
| UpdateDepartment_WithValidData_UpdatesDepartment | Updates a department and checks new values      |
| GetAllDepartmentsAsync_ReturnsAllDepartments   | Verifies async retrieval of all departments      |
| AddDepartmentAsync_AddsDepartmentSuccessfully  | Verifies async add method for department         |
| UpdateDepartmentAsync_UpdatesDepartmentSuccessfully | Verifies async update method for department    |
| DeleteDepartmentAsync_DeletesDepartmentSuccessfully | Verifies async delete method for department    |

## Example Jasmine Tests (Frontend)

| Test File                        | Description                                      |
|----------------------------------|--------------------------------------------------|
| persons.component.spec.ts        | Renders persons table, checks CRUD UI, error display |
| persons.component.spec.ts        | Tests sorting by first name and last name, toggling sort direction |
| department.component.spec.ts     | Renders departments table, checks CRUD UI, error display |
| department.component.spec.ts     | Tests sorting by department name, toggling sort direction |
| department.service.spec.ts       | Tests API calls for department service (async CRUD) |
| person.service.spec.ts           | Tests API calls for person service (async CRUD)   |

> For a full list and live results, see the [GitHub Actions test runs](https://github.com/<OWNER>/<REPO>/actions).

## How to Run
```sh
dotnet test           # Run backend xUnit tests
cd UKParliament.CodeTest.Web/ClientApp
ng test               # Run frontend Jasmine/Karma tests
```

---

See the main [README](./README.md) for more details.
