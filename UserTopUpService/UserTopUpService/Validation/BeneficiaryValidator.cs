using FluentValidation;
using UserTopUpService.Domain;

namespace UserTopUpService.Validation
{

    public class BeneficiaryValidator : AbstractValidator<Beneficiary>
    {
        public BeneficiaryValidator()
        {


            RuleFor(benificiary => benificiary.Nickname)
              .NotNull()
              .Length(1, 20)
              .WithMessage("beneficiary must have a nickname with a maximum length of 20.");
        }
    }

}