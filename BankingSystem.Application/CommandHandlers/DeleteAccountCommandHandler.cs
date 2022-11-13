using BankingSystem.Application.Abstractions;
using BankingSystem.Application.Commands;
using BankingSystem.Domain.Repositories;

namespace BankingSystem.Application.CommandHandlers;

public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Task HandleAsync(DeleteAccountCommand command)
    {
        return _accountRepository.DeleteAsync(command.Id);
    }
}
