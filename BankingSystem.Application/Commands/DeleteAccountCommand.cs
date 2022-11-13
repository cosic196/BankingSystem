using BankingSystem.Application.Abstractions;

namespace BankingSystem.Application.Commands;

public class DeleteAccountCommand : ICommand
{
    public DeleteAccountCommand(string id)
    {
        Id = id;
    }

    public string Id { get; }
}
