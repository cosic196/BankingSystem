namespace BankingSystem.Domain.Exceptions;

public class InvalidAccountBalanceException : Exception
{
    public InvalidAccountBalanceException()
        : base($"Invalid account balance. Account balance must be at least {Constants.MinimumBalance}")
    {
    }
}
