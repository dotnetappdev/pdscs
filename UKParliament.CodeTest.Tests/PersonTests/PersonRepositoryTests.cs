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
    }
}
