namespace BankingSystem.Domain.Exceptions;

public class InvalidDepositException : Exception
{
	public InvalidDepositException()
		: base($"Tried depositing more than {Constants.MaxDeposit}")
	{
	}
}
