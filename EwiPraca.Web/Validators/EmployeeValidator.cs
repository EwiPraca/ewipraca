using EwiPraca.Models;
using EwiPracaConstants;
using FluentValidation;

namespace EwiPraca.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeViewModel>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.FirstName)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("Pole Imię nie może być puste.")
              .Length(1, Validations.MaximumLength.FirstNameLength)
              .WithMessage(string.Format("Pole Imię może mieć maksymalnie {0}", Validations.MaximumLength.FirstNameLength));

            RuleFor(x => x.Surname)
             .Cascade(CascadeMode.StopOnFirstFailure)
             .NotEmpty()
             .WithMessage("Pole Nazwisko nie może być puste.")
             .Length(1, Validations.MaximumLength.SurnameLength)
             .WithMessage(string.Format("Pole Nazwisko może mieć maksymalnie {0}", Validations.MaximumLength.SurnameLength));

            RuleFor(x => x.BirthDate)
             .Cascade(CascadeMode.StopOnFirstFailure)
             .NotEmpty()
             .WithMessage("Pole Data Urodzenia nie może być puste.");

            RuleFor(x => x.PESEL)
             .Cascade(CascadeMode.StopOnFirstFailure)
             .NotEmpty()
             .WithMessage("Numer PESEL nie może być pusty.")
             .Must(x => x.IsValidPESEL())
             .WithMessage(WebResources.IncorrectPESELMessage);
        }
    }
}