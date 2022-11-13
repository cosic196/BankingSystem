using BankingSystem.Api.Models;
using BankingSystem.Api.Validation;
using BankingSystem.Domain;
using FluentValidation.TestHelper;
using Xunit;

namespace BankingSystem.Tests.Api.Validation;

public class AccountDepositValidatorTests
{
    private readonly AccountDepositValidator _sut;

    public AccountDepositValidatorTests()
    {
        _sut = new AccountDepositValidator();
    }

    [Fact]
    public void Should_return_valid_for_valid_AccountDeposit()
    {
        var validAccountDeposit = new AccountDeposit
        {
            Id = "someId",
            Amount = 20,
        };

        var result = _sut.TestValidate(validAccountDeposit);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Should_return_invalid_for_empty_id(string id)
    {
        var validAccountCreation = new AccountDeposit
        {
            Id = id,
            Amount = 20,
        };

        var result = _sut.TestValidate(validAccountCreation);

        result.ShouldHaveValidationErrorFor(model => model.Id);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-3000)]
    public void Should_return_invalid_for_0_or_negative_amount(decimal amount)
    {
        var validAccountCreation = new AccountDeposit
        {
            Id = "id",
            Amount = amount,
        };

        var result = _sut.TestValidate(validAccountCreation);

        result.ShouldHaveValidationErrorFor(model => model.Amount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(1250)]
    [InlineData(2305)]
    public void Should_return_invalid_for_greater_than_max_deposit_amount(decimal amountAddedToMaxDeposit)
    {
        var validAccountCreation = new AccountDeposit
        {
            Id = "id",
            Amount = Constants.MaxDeposit + amountAddedToMaxDeposit,
        };

        var result = _sut.TestValidate(validAccountCreation);

        if (amountAddedToMaxDeposit > 0)
        {
            result.ShouldHaveValidationErrorFor(model => model.Amount);
        }
        else
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
