using BankingSystem.Application.Abstractions;

namespace BankingSystem.Application.Mediator;

public interface IMediator
{
    Task SendCommandAsync<TCommand>(TCommand command)
        where TCommand : ICommand;

    Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>;
}
