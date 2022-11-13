using BankingSystem.Domain;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BankingSystem.Tests.Domain;

public class AccountTests
{
    private const string Id = "testId";
    private const string UserId = "testUserId";

    [Fact]
    public void Should_have_data_given_during_creation()
    {
        var account = CreateAccount(Constants.MinimumBalance);

        account.Id.Should().Be(Id);
        account.UserId.Should().Be(UserId);
        account.Balance.Should().Be(Constants.MinimumBalance);
    }

    [Fact]
    public void Should_throw_exception_when_initialized_with_low_balance()
    {
        var moneyUnderLimit = Constants.MinimumBalance - 1M;
        var moneyOverLimit = Constants.MinimumBalance + 1M;

        var creatingAccountWithLowBalance = () => new Account(Id, UserId, moneyUnderLimit);
        var creatingAccountWithMinimumBalance = () => new Account(Id, UserId, Constants.MinimumBalance);
        var creatingAccountWithHighBalance = () => new Account(Id, UserId, moneyOverLimit);

        creatingAccountWithLowBalance.Should().Throw<InvalidAccountBalanceException>();
        creatingAccountWithMinimumBalance.Should().NotThrow<InvalidAccountBalanceException>();
        creatingAccountWithHighBalance.Should().NotThrow<InvalidAccountBalanceException>();
    }

    [Theory]
    [InlineData(100)]
    [InlineData(25)]
    [InlineData(40)]
    [InlineData(500)]
    public void Withdrawing_should_lower_balance_by_input_amount(decimal amountAddedToMinimumForStartingBalance)
    {
        var amountWithdrawn = amountAddedToMinimumForStartingBalance / (Random.Shared.Next(4) + 2);
        var startingBalance = Constants.MinimumBalance + amountAddedToMinimumForStartingBalance;
        var account = CreateAccount(startingBalance);

        account.Withdraw(amountWithdrawn);

        account.Balance.Should().Be(startingBalance - amountWithdrawn);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(25)]
    [InlineData(40)]
    [InlineData(500)]
    public void Depositing_should_increase_balance_by_input_amount(decimal amountDeposited)
    {
        var startingBalance = Constants.MinimumBalance;
        var account = CreateAccount(startingBalance);

        account.Deposit(amountDeposited);

        account.Balance.Should().Be(startingBalance + amountDeposited);
    }

    [Fact]
    public void Withdrawing_should_throw_exception_if_balance_results_in_lower_than_minimum()
    {
        var amountWithdrawn = Constants.MinimumBalance / (Random.Shared.Next(4) + 2);
        var account = CreateAccount(Constants.MinimumBalance);

        var withdrawal = () => account.Withdraw(amountWithdrawn);

        withdrawal.Should().Throw<InvalidAccountBalanceException>();
    }

    [Fact]
    public void Withdrawing_should_throw_exception_if_amount_is_more_than_max_allowed()
    {
        var startingBalance = Constants.MinimumBalance * 100;
        var amountWithdrawn = startingBalance * Constants.MaxBalanceWithdrawal + 1;
        var account = CreateAccount(startingBalance);

        var withdrawal = () => account.Withdraw(amountWithdrawn);

        withdrawal.Should().Throw<InvalidWithdrawalException>();
    }

    [Fact]
    public void Depositing_should_throw_exception_if_amount_is_more_than_max_allowed()
    {
        var amountDeposited = Constants.MaxDeposit + 1;
        var account = CreateAccount(Constants.MinimumBalance);

        var depositing = () => account.Deposit(amountDeposited);

        depositing.Should().Throw<InvalidDepositException>();
    }


    private static Account CreateAccount(Money balance) => new Account(Id, UserId, balance);
}
