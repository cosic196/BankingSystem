using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;
using System.Collections.Concurrent;

namespace BankingSystem.Infrastructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly ConcurrentDictionary<string, Account> _accountsDictionary;

    public InMemoryAccountRepository()
    {
        _accountsDictionary = new ConcurrentDictionary<string, Account>();
    }

    public Task InsertAsync(Account account)
    {
        if (_accountsDictionary.ContainsKey(account.Id))
        {
            throw new InvalidOperationException($"Account with id '{account.Id}' already exists");
        }

        _accountsDictionary.TryAdd(account.Id, account);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id)
    {
        if (_accountsDictionary.ContainsKey(id))
        {
            _accountsDictionary.TryRemove(id, out _);
        }

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Account>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Account>>(_accountsDictionary.Values);
    }

    public Task<Account?> GetByIdAsync(string id)
    {
        _accountsDictionary.TryGetValue(id, out var account);
        return Task.FromResult(account);
    }

    public Task UpdateAsync(Account account)
    {
        return Task.CompletedTask;
    }
}
