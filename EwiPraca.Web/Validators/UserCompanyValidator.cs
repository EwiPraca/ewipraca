using EwiPraca.Models;
using FluentValidation;

namespace EwiPraca.Validators
{
    public class UserCompanyValidator : AbstractValidator<UserCompanyViewModel>
    {
        public UserCompanyValidator()
        {
            RuleFor(x => x.NIP)
                .Must(x => x.IsValidNIP()).
                WithMessage(WebResources.IncorrectNIPMessage);

            RuleFor(x => x.REGON)
                .Must(x => x.IsValidREGON()).
                WithMessage(WebResources.IncorrectREGONMessage);
        }
    }
}