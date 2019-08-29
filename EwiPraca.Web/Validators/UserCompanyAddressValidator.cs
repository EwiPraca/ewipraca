using EwiPraca.Models;
using EwiPracaConstants;
using FluentValidation;

namespace EwiPraca.Validators
{
    public class UserCompanyAddressValidator : AbstractValidator<AddressViewModel>
    {
        public UserCompanyAddressValidator()
        {
            RuleFor(x => x.City)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithMessage("Pole Miasto nie może być puste.")
               .Length(1, Validations.MaximumLength.CityNameLength)
               .WithMessage(string.Format("Pole Miasto może mieć maksymalnie {0} znaków.", Validations.MaximumLength.CityNameLength));

            RuleFor(x => x.StreetName)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithMessage("Pole Ulica nie może być puste.")
               .Length(1, Validations.MaximumLength.StreetNameLength)
               .WithMessage(string.Format("Pole Ulica może mieć maksymalnie {0} znaków.", Validations.MaximumLength.StreetNameLength));

            RuleFor(x => x.StreetNumber)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithMessage("Pole Numer nie może być puste.")
               .Length(1, Validations.MaximumLength.StreetNumberLength)
               .WithMessage(string.Format("Pole Numer może mieć maksymalnie {0} znaków.", Validations.MaximumLength.StreetNumberLength));

            RuleFor(x => x.ZIPCode)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty()
               .WithMessage("Pole Kod Pocztowy nie może być puste.")
               .Length(1, Validations.MaximumLength.ZIPCodeLength)
               .WithMessage(string.Format("Pole Kod Pocztowy może mieć maksymalnie {0} znaków.", Validations.MaximumLength.ZIPCodeLength));

            RuleFor(x => x.PlaceNumber)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .Length(0, Validations.MaximumLength.PlaceNumberLength)
               .WithMessage(string.Format("Pole Numer Lokal może mieć maksymalnie {0} znaków.", Validations.MaximumLength.PlaceNumberLength));
        }
    }
}