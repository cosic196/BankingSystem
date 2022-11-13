using BankingSystem.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BankingSystem.Application.Mediator;

public class InMemoryMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public InMemoryMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task SendCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var commandHandler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return commandHandler.HandleAsync(command);
    }

    public Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
    {
        var queryHandler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return queryHandler.HandleAsync(query);
    }
}
