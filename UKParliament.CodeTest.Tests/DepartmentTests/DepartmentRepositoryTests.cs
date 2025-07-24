using System;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UKParliament.CodeTest.Tests.DepartmentTests
{
    public class DepartmentRepositoryTests
    {
        private PersonManagerContext GetInMemoryContext()
        {
            var uniqueDbName = $"TestDb_Departments_{System.Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: uniqueDbName)
                .Options;
            var context = new PersonManagerContext(options);
            return context;
        }

        [Fact]
        public async Task AddDepartment_WithValidDepartment_ReturnsSuccessAndDepartment()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UKParliament.CodeTest.Services.DepartmentWriteService>();
            var repo = new UKParliament.CodeTest.Services.DepartmentWriteService(context, logger);
            var department = new Department {
                Name = "Test Department"
            };

            // Act
            var result = await repo.AddAsync(department);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Department", result.Name);
        }

        [Fact]
        public void AddDepartment_WithNullDepartmentName_FailsValidation()
        {
            // Arrange
            var validator = new UKParliament.CodeTest.Services.Validation.DepartmentValidator();
            var department = new Department {
                Id = 100, // Ensure unique ID for test
                Name = null!
            };

            // Act
            var result = validator.Validate(department);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Fact]
        public async Task DeleteDepartment_WithValidId_RemovesDepartment()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UKParliament.CodeTest.Services.DepartmentWriteService>();
            var repo = new UKParliament.CodeTest.Services.DepartmentWriteService(context, logger);
            var department = new Department {
                Name = "Delete Department"
            };
            var created = await repo.AddAsync(department);
            Assert.NotNull(created);
            var id = created.Id;

            // Act
            var deleted = await repo.DeleteAsync(id);

            // Assert
            Assert.True(deleted);
            Assert.Null(context.Departments.Find(id));
        }

        [Fact]
        public async Task UpdateDepartment_WithValidData_UpdatesDepartment()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UKParliament.CodeTest.Services.DepartmentWriteService>();
            var repo = new UKParliament.CodeTest.Services.DepartmentWriteService(context, logger);
            var department = new Department {
                Name = "Original Department"
            };
            var created = await repo.AddAsync(department);
            Assert.NotNull(created);
            var id = created.Id;

            // Act
            var updatedDepartment = new Department {
                Id = id,
                Name = "Updated Department"
            };
            var updated = await repo.UpdateAsync(updatedDepartment);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal("Updated Department", updated.Name);
        }
    }
}
