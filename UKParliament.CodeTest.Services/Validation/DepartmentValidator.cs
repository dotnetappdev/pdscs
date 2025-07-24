using FluentValidation;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Validation
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(100).WithMessage("Department name must be 100 characters or fewer.");
        }
    }
}
