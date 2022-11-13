namespace BankingSystem.Api.Models;

public class AccountDetails
{
    /// <summary>
    /// Account ID.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// ID of the User owning the Account.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Currently available balance on the Account.
    /// </summary>
    public decimal AvailableBalance { get; set; }
}
