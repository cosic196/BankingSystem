using BankingSystem.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BankingSystem.Tests.Domain;

public class MoneyTests
{
    [Theory]
    [InlineData(12)]
    [InlineData(22)]
    [InlineData(1800)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-12)]
    [InlineData(-123)]
    public void Should_allow_only_positive_values(decimal amount)
    {
        var moneyInitialization = () => new Money(amount);

        if (amount < 0)
        {
            moneyInitialization.Should().Throw<ArgumentException>();
        }
        else
        {
            moneyInitialization.Should().NotThrow();
        }
    }

    [Theory]
    [InlineData(12.33, 25.1)]
    [InlineData(35.56, 44.2)]
    [InlineData(1234, 5678)]
    public void Addition_should_produce_correct_amount(decimal amount1, decimal amount2)
    {
        var money1 = new Money(amount1);
        var money2 = new Money(amount2);

        var result = money1 + money2;

        result.Amount.Should().Be(money1.Amount + money2.Amount);
    }

    [Theory]
    [InlineData(25.1, 12.33)]
    [InlineData(44.2, 35.56)]
    [InlineData(5678, 1234)]
    public void Subtraction_should_produce_correct_amount(decimal amount1, decimal amount2)
    {
        var money1 = new Money(amount1);
        var money2 = new Money(amount2);

        var result = money1 - money2;

        result.Amount.Should().Be(money1.Amount - money2.Amount);
    }

    [Theory]
    [InlineData(25.1, 25.1)]
    [InlineData(12, 12)]
    [InlineData(1.5, 1.5)]
    [InlineData(1.5, 1.6)]
    [InlineData(321, 456)]
    public void Two_moneys_with_same_amount_should_be_considered_equal(decimal amount1, decimal amount2)
    {
        var areEqualAmounts = amount1 == amount2;
        var money1 = new Money(amount1);
        var money2 = new Money(amount2);

        money1.Equals(money2).Should().Be(areEqualAmounts);
        (money1 == money2).Should().Be(areEqualAmounts);
        (money1.GetHashCode() == money2.GetHashCode()).Should().Be(areEqualAmounts);
    }

    [Theory]
    [InlineData(25.1)]
    [InlineData(44.2)]
    [InlineData(5678)]
    public void Should_be_implicitly_convertable_to_and_from_decimal(decimal amount)
    {
        Money moneyFromDecimal = amount;
        decimal decimalFromMoney = new Money(amount);

        moneyFromDecimal.Amount.Should().Be(amount);
        decimalFromMoney.Should().Be(amount);
    }
}
