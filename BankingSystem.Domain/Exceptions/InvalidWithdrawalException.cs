namespace BankingSystem.Domain.Exceptions;

public class InvalidWithdrawalException : Exception
{
	public InvalidWithdrawalException()
		: base($"Tried withdrawing more than {Constants.MaxBalanceWithdrawal * 100}% of the account's current balance")
	{
	}
}
