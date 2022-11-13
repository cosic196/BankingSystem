using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities;

public class Account
{
	private Money _balance = null!;
	
	public Account(string id, string userId, Money initialBalance)
	{
		Id = id;
		UserId = userId;
		Balance = initialBalance;
	}

	public string Id { get; }

	public string UserId { get; }

	public Money Balance
	{
		get
		{
			return _balance;
		}
		private set
		{
			_balance = value >= Constants.MinimumBalance ? value : throw new InvalidAccountBalanceException();
		}
	}

	public void Withdraw(Money amount)
	{
		if (amount.Amount > Balance.Amount * Constants.MaxBalanceWithdrawal)
		{
			throw new InvalidWithdrawalException();
		}

		Balance -= amount;
	}

	public void Deposit(Money amount)
	{
		if (amount > Constants.MaxDeposit)
		{
			throw new InvalidDepositException();
		}

		Balance += amount;
	}
}
