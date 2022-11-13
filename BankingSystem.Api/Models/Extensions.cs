using BankingSystem.Application.Commands;
using BankingSystem.Domain.Entities;

namespace BankingSystem.Api.Models;

public static class Extensions
{
    public static CreateAccountCommand ToCommand(this AccountCreation accountCreation, string id)
    {
        return new CreateAccountCommand(id, accountCreation.UserId!, accountCreation.InitialBalance);
    }

    public static WithdrawFromAccountCommand ToCommand(this AccountWithdrawal accountWithdrawal)
    {
        return new WithdrawFromAccountCommand(accountWithdrawal.Id!, accountWithdrawal.Amount);
    }

    public static DepositToAccountCommand ToCommand(this AccountDeposit accountDeposit)
    {
        return new DepositToAccountCommand(accountDeposit.Id!, accountDeposit.Amount);
    }

    public static AccountDetails ToApiModel(this Account account)
    {
        return new AccountDetails
        {
            Id = account.Id.ToString(),
            UserId = account.UserId,
            AvailableBalance = account.Balance,
        };
    }

    public static IEnumerable<AccountDetails> ToApiModel(this IEnumerable<Account> accounts)
    {
        return accounts.Select(ToApiModel);
    }
}
