namespace BankingSystem.Application.Exceptions;

public class AccountNotFoundException : Exception
{
	public AccountNotFoundException(string id)
		: base($"Account with id '{id}' not found")
	{
	}
}
