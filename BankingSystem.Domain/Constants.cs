using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain;

public static class Constants
{
    public static readonly Money MinimumBalance = 100M;

    public static readonly Money MaxDeposit = 10000M;

    public static readonly decimal MaxBalanceWithdrawal = 0.9M;
}
