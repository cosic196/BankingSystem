using BankingSystem.Application.Abstractions;
using BankingSystem.Application.Commands;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;

namespace BankingSystem.Application.CommandHandlers;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Task HandleAsync(CreateAccountCommand command)
    {
        var createdAccount = new Account(command.Id, command.UserId, command.InitialBalance);
        return _accountRepository.InsertAsync(createdAccount);
    }
}
