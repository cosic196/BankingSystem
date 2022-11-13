namespace BankingSystem.Application.Abstractions;

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}
