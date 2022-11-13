using BankingSystem.Application.Abstractions;
using BankingSystem.Application.Commands;
using BankingSystem.Application.Exceptions;
using BankingSystem.Domain.Repositories;

namespace BankingSystem.Application.CommandHandlers;

public class DepositToAccountCommandHandler : ICommandHandler<DepositToAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public DepositToAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task HandleAsync(DepositToAccountCommand command)
    {
        var account = await _accountRepository.GetByIdAsync(command.Id);
        if (account == null)
        {
            throw new AccountNotFoundException(command.Id);
        }

        account.Deposit(command.Amount);
        await _accountRepository.UpdateAsync(account);
    }
}
