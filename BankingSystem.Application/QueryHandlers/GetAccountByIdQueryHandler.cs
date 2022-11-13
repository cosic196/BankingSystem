using BankingSystem.Application.Abstractions;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Queries;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;

namespace BankingSystem.Application.QueryHandlers;

public class GetAccountByIdQueryHandler : IQueryHandler<GetAccountByIdQuery, Account>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Account> HandleAsync(GetAccountByIdQuery query)
    {
        var account = await _accountRepository.GetByIdAsync(query.Id);
        return account ?? throw new AccountNotFoundException(query.Id);
    }
}
