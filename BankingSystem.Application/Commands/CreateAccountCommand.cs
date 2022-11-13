using BankingSystem.Application.Abstractions;

namespace BankingSystem.Application.Commands;

public class CreateAccountCommand : ICommand
{
    public CreateAccountCommand(string id, string userId, decimal initialBalance)
    {
        Id = id;
        UserId = userId;
        InitialBalance = initialBalance;
    }

    public string Id { get; }

    public string UserId { get; }

    public decimal InitialBalance { get; }
}
