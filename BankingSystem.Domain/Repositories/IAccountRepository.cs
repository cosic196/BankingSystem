using BankingSystem.Domain.Entities;

namespace BankingSystem.Domain.Repositories;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAllAsync();

    Task<Account?> GetByIdAsync(string id);

    Task InsertAsync(Account account);

    Task UpdateAsync(Account account);

    Task DeleteAsync(string id);
}
