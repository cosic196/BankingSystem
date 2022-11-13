using BankingSystem.Api.Models;
using FluentValidation;

namespace BankingSystem.Api.Validation;

public class AccountWithdrawalValidator : AbstractValidator<AccountWithdrawal>
{
    public AccountWithdrawalValidator()
    {
        RuleFor(model => model.Id).NotEmpty();
        RuleFor(model => model.Amount).GreaterThan(0);
    }
}