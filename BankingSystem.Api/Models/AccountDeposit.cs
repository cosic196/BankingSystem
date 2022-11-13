namespace BankingSystem.Api.Models;

public class AccountDeposit
{
    /// <summary>
    /// Account ID.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Amount to deposit to Account.
    /// </summary>
    public decimal Amount { get; set; }
}
