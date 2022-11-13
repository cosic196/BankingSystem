using BankingSystem.Api.Models;
using BankingSystem.Domain;
using FluentValidation;

namespace BankingSystem.Api.Validation;

public class AccountCreationValidator : AbstractValidator<AccountCreation>
{
	public AccountCreationValidator()
	{
		RuleFor(model => model.UserId).NotEmpty();
		RuleFor(model => model.InitialBalance).GreaterThanOrEqualTo(Constants.MinimumBalance);
	}
}
