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
               .NotEmpty().WithMessage("First Name is required.")
               .MinimumLength(4).WithMessage("First Name must be at least 2 characters long.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MinimumLength(4).WithMessage("Last Name must be at least 2 characters long.");

            RuleFor(x => x.DOB)
                .Must(dob => dob != null)
                .WithMessage("Must not be null.")
                .Must(dob => dob == null || dob >= new DateOnly(1900, 1, 1))
                .WithMessage("Date of birth cannot be before 01/01/1900.")
                .Must(dob => dob == null || dob <= DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                .WithMessage("Person must be at least 18 years old.");
        }
    }
}
