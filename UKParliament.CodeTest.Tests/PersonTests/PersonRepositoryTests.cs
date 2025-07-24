using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using UKParliament.CodeTest.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UKParliament.CodeTest.Tests.PersonTests
{
    public class PersonRepositoryTests
    {
        private PersonManagerContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Persons")
                .Options;
            var context = new PersonManagerContext(options);
            return context;
        }

        [Fact]
        public void AddPerson_WithValidPerson_ReturnsSuccessAndPerson()
        {
            // Arrange
            var context = GetInMemoryContext();
            // Seed with SampleData for consistency
            context.Departments.AddRange(SampleData.GetDepartments());
            context.People.AddRange(SampleData.GetPeople());
            context.SaveChanges();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonWriteRepository>();
            var repo = new PersonWriteRepository(context, logger);
            var person = new Person {
                FirstName = "TestAdd",
                LastName = "Person", // similar to sample but unique for test
                DepartmentId = 1,
                Description = "HR Specialist",
                DOB = new System.DateOnly(1998, 1, 26)
            };

            // Act
            var result = repo.AddPerson(person);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("TestAdd", result.Data.FirstName);
            Assert.Equal("Person", result.Data.LastName);
        }

        [Fact]
        public void AddPerson_WithNullPerson_ReturnsBadRequest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonWriteRepository>();
            var repo = new PersonWriteRepository(context, logger);

            // Act
            var result = repo.AddPerson(null);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task DeletePerson_WithValidId_RemovesPerson()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Departments.AddRange(SampleData.GetDepartments());
            context.People.AddRange(SampleData.GetPeople());
            context.SaveChanges();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonWriteRepository>();
            var repo = new PersonWriteRepository(context, logger);
            var person = new Person {
                FirstName = "DeleteMe",
                LastName = "Person",
                DepartmentId = 1,
                Description = "To be deleted",
                DOB = new System.DateOnly(1990, 1, 1)
            };
            var addResult = repo.AddPerson(person);
            Assert.True(addResult.Success);
            var id = addResult.Data.Id;

            // Act
            var deleteResult = await repo.DeletePerson(id);

            // Assert
            Assert.True(deleteResult.Success);
            Assert.Equal(System.Net.HttpStatusCode.OK, deleteResult.StatusCode);
            Assert.Null(context.People.Find(id));
        }

        [Fact]
        public void UpdatePerson_WithValidData_UpdatesPerson()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Departments.AddRange(SampleData.GetDepartments());
            context.People.AddRange(SampleData.GetPeople());
            context.SaveChanges();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonWriteRepository>();
            var repo = new PersonWriteRepository(context, logger);
            var person = new Person {
                FirstName = "UpdateMe",
                LastName = "Person",
                DepartmentId = 1,
                Description = "To be updated",
                DOB = new System.DateOnly(1995, 5, 5)
            };
            var addResult = repo.AddPerson(person);
            Assert.True(addResult.Success);
            var id = addResult.Data.Id;

            // Act
            var updatedPerson = new Person {
                Id = id,
                FirstName = "Updated",
                LastName = "Person",
                DepartmentId = 1,
                Description = "Updated description",
                DOB = new System.DateOnly(1995, 5, 5)
            };
            var updateResult = repo.UpdatePerson(id, updatedPerson);

            // Assert
            Assert.True(updateResult.Success);
            Assert.Equal("Updated", updateResult.Data.FirstName);
            Assert.Equal("Updated description", updateResult.Data.Description);
        }
    }
}
