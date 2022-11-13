using BankingSystem.Application.Abstractions;
using BankingSystem.Domain.Entities;

namespace BankingSystem.Application.Queries;

public class GetAllAccountsQuery : IQuery<IEnumerable<Account>>
{
}
