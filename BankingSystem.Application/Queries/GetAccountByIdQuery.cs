using BankingSystem.Application.Abstractions;
using BankingSystem.Domain.Entities;

namespace BankingSystem.Application.Queries;

public class GetAccountByIdQuery : IQuery<Account>
{
    public GetAccountByIdQuery(string id)
    {
        Id = id;
    }

    public string Id { get; }
}
