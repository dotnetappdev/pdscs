using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

// Ensure PersonReadRepository is available
// If the class is in a different namespace, add the correct using below
// using UKParliament.CodeTest.Services.Repositories;

namespace UKParliament.CodeTest.Tests.PersonTests
{
    public class PersonRepositoryTests
    {
        private PersonManagerContext GetInMemoryContext()
        {
            var uniqueDbName = $"TestDb_Persons_{System.Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: uniqueDbName)
                .Options;
            var context = new PersonManagerContext(options);
            return context;
        }
         [Fact]
        public void GetAll_ReturnsAllPersons()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Departments.AddRange(SampleData.GetDepartments());
            context.People.AddRange(SampleData.GetPeople());
            context.SaveChanges();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonReadRepository>();
            var repo = new PersonReadRepository(context, logger);

            // Act
            var result = repo.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(context.People.Count(), result.Count());
        }

        [Fact]
        public void GetById_WithValidId_ReturnsPerson()
        {
            // Arrange
            var context = GetInMemoryContext();
            context.Departments.AddRange(SampleData.GetDepartments());
            context.People.AddRange(SampleData.GetPeople());
            context.SaveChanges();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonReadRepository>();
            var repo = new PersonReadRepository(context, logger);
            var person = context.People.First();

            // Act
            var result = repo.GetById(person.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(person.FirstName, result.FirstName);
            Assert.Equal(person.LastName, result.LastName);
        }
        [Fact]
        public void AddPerson_WithValidPerson_ReturnsSuccessAndPerson()
        {
            // Arrange
            var context = GetInMemoryContext();
            // Seed both departments and people to use sample data
            context.Departments.AddRange(SampleData.GetDepartments());
            context.People.AddRange(SampleData.GetPeople());
            context.SaveChanges();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonWriteRepository>();
            var repo = new PersonWriteRepository(context, logger);
            // Use a unique name to avoid conflicts with seeded people
            var person = new Person
            {
                Id = 1000,
                FirstName = "TestAddUnique",
                LastName = "PersonTest",
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
            Assert.Equal("TestAddUnique", result.Data.FirstName);
            Assert.Equal("PersonTest", result.Data.LastName);
        }

        [Fact]
        public void AddPerson_WithNullPerson_ReturnsBadRequest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonWriteRepository>();
            var repo = new PersonWriteRepository(context, logger);

            // Act
            var result = repo.AddPerson(null!);

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
            var person = new Person
            {
                FirstName = "DeleteMe",
                LastName = "Person",
                DepartmentId = 1,
                Description = "To be deleted",
                DOB = new System.DateOnly(1990, 1, 1)
            };
            var addResult = repo.AddPerson(person);
            Assert.True(addResult.Success);
            Assert.NotNull(addResult.Data);
            var id = addResult.Data!.Id;

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
            context.SaveChanges();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<PersonWriteRepository>();
            var repo = new PersonWriteRepository(context, logger);
            var person = new Person
            {
                FirstName = "UpdateMe",
                LastName = "Person",
                DepartmentId = 1,
                Description = "To be updated",
                DOB = new System.DateOnly(1995, 5, 5)
            };
            var addResult = repo.AddPerson(person);
            Assert.True(addResult.Success);
            Assert.NotNull(addResult.Data);
            var id = addResult.Data!.Id;

            // Act
            var updatedPerson = new Person
            {
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
            Assert.NotNull(updateResult.Data);
            Assert.Equal("Updated", updateResult.Data!.FirstName);
            Assert.Equal("Updated description", updateResult.Data.Description);
        }
    }
}
