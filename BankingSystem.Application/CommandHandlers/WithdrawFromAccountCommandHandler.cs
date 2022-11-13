using BankingSystem.Application.Abstractions;
using BankingSystem.Application.Commands;
using BankingSystem.Application.Exceptions;
using BankingSystem.Domain.Repositories;

namespace BankingSystem.Application.CommandHandlers;

public class WithdrawFromAccountCommandHandler : ICommandHandler<WithdrawFromAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public WithdrawFromAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task HandleAsync(WithdrawFromAccountCommand command)
    {
        var account = await _accountRepository.GetByIdAsync(command.Id);
        if (account == null)
        {
            throw new AccountNotFoundException(command.Id);
        }

        account.Withdraw(command.Amount);
        await _accountRepository.UpdateAsync(account);
    }
}
