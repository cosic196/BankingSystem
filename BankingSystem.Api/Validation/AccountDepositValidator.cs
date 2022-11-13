using BankingSystem.Api.Models;
using BankingSystem.Domain;
using FluentValidation;

namespace BankingSystem.Api.Validation;

public class AccountDepositValidator : AbstractValidator<AccountDeposit>
{
	public AccountDepositValidator()
	{
        RuleFor(model => model.Id).NotEmpty();
        RuleFor(model => model.Amount).GreaterThan(0);
        RuleFor(model => model.Amount).LessThanOrEqualTo(Constants.MaxDeposit);
    }
}
