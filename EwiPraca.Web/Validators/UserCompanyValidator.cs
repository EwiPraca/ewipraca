using EwiPraca.Models;
using EwiPracaConstants;
using FluentValidation;

namespace EwiPraca.Validators
{
    public class UserCompanyValidator : AbstractValidator<UserCompanyViewModel>
    {
        public UserCompanyValidator()
        {
            RuleFor(x => x.CompanyName)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithMessage("Nazwa firmy nie może być pusta.")
               .Length(1, Validations.MaximumLength.CompanyNameLength)
               .WithMessage(string.Format("Pole Nazwa firmy może mieć maksymalnie {0}", Validations.MaximumLength.CompanyNameLength));

            RuleFor(x => x.NIP)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Numer NIP nie może być pusty.")
                .Must(x => x.IsValidNIP()).
                WithMessage(WebResources.IncorrectNIPMessage);

            RuleFor(x => x.REGON)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Numer REGON nie może być pusty.")
                .Must(x => x.IsValidREGON()).
                WithMessage(WebResources.IncorrectREGONMessage);

            RuleFor(x => x.Notes)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .Length(0, Validations.MaximumLength.CompanyNotesLength)
               .WithMessage(WebResources.TooLongCompanyNotesMessage);
        }
    }
}