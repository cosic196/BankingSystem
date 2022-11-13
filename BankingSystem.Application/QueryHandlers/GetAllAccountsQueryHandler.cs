using BankingSystem.Application.Abstractions;
using BankingSystem.Application.Queries;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;

namespace BankingSystem.Application.QueryHandlers;

public class GetAllAccountsQueryHandler : IQueryHandler<GetAllAccountsQuery, IEnumerable<Account>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAllAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public Task<IEnumerable<Account>> HandleAsync(GetAllAccountsQuery query)
    {
        return _accountRepository.GetAllAsync();
    }
}
