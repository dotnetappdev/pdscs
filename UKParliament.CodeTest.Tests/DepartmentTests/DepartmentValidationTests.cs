using UKParliament.CodeTest.Services.Validation;
using UKParliament.CodeTest.Data;
using Xunit;

namespace UKParliament.CodeTest.Tests.DepartmentTests
{
    public class DepartmentValidationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateDepartment_NameIsNullOrEmpty_ReturnsValidationMessage(string name)
        {
            // Arrange
            var department = new Department { Name = name };
            var validator = new DepartmentValidator();

            // Act
            var result = validator.Validate(department);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("Department name is required"));
        }

        [Fact]
        public void ValidateDepartment_NameIsValid_ReturnsValid()
        {
            // Arrange
            var department = new Department { Name = "Finance" };
            var validator = new DepartmentValidator();

            // Act
            var result = validator.Validate(department);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
