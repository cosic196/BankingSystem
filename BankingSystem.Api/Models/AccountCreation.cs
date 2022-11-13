namespace BankingSystem.Api.Models;

public class AccountCreation
{
    /// <summary>
    /// ID of the user creating the account.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Initial balance of the created account.
    /// </summary>
    public decimal InitialBalance { get; set; }
}
