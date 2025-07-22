using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.Validation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("*")
                .MinimumLength(2).WithMessage("Must be at least 2 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("*")
                .MinimumLength(2).WithMessage("Must be at least 2 characters.");

            RuleFor(p => p.DepartmentId)
                .NotEmpty().WithMessage("*");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("*")
                .MinimumLength(2).WithMessage("Must be at least 2 characters.")
                .MaximumLength(1000).WithMessage("Too long (max 1000 characters).");

            RuleFor(x => x.DOB)
                .NotNull().WithMessage("*")
                .Must(dob => dob == null || dob >= new DateOnly(1900, 1, 1))
                .WithMessage("Must be after 01/01/1900.")
                .Must(dob => dob == null || dob <= DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                .WithMessage("Must be at least 18 years old.");
        }
    }

}
