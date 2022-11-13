using BankingSystem.Application.Abstractions;

namespace BankingSystem.Application.Commands;

public class DepositToAccountCommand : ICommand
{
	public DepositToAccountCommand(string id, decimal amount)
	{
		Id = id;
		Amount = amount;
	}

	public string Id { get; }

	public decimal Amount { get; }
}
