using System;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Validation;
using Xunit;

namespace UKParliament.CodeTest.Tests.PersonTests
{
    public class PersonValidationTests
    {
        [Fact]
        public void Person_WithUnder18DOB_FailsValidation()
        {
            // Arrange
            var validator = new PersonValidator();
            var person = new Person
            {
                FirstName = "Test",
                LastName = "User",
                DepartmentId = 1,
                Description = "Test description",
                DOB = DateOnly.FromDateTime(DateTime.Now.AddYears(-17)) // 17 years old
            };

            // Act
            var result = validator.Validate(person);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "DOB");
        }

        [Fact]
        public void Person_WithDescriptionOver1000Chars_FailsValidation()
        {
            // Arrange
            var validator = new PersonValidator();
            var person = new Person
            {
                FirstName = "Test",
                LastName = "User",
                DepartmentId = 1,
                Description = new string('a', 1001),
                DOB = DateOnly.FromDateTime(DateTime.Now.AddYears(-25))
            };

            // Act
            var result = validator.Validate(person);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Description");
        }
    }
}
