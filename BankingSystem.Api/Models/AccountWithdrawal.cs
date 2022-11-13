namespace BankingSystem.Api.Models;

public class AccountWithdrawal
{
    /// <summary>
    /// Account ID.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Amount to withdraw from the Account.
    /// </summary>
    public decimal Amount { get; set; }
}
