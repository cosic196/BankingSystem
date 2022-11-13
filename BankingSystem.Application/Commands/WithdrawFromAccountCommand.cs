using BankingSystem.Application.Abstractions;

namespace BankingSystem.Application.Commands;

public class WithdrawFromAccountCommand : ICommand
{
	public WithdrawFromAccountCommand(string id, decimal amount)
	{
		Id = id;
		Amount = amount;
	}

	public string Id { get; }

	public decimal Amount { get; }
}