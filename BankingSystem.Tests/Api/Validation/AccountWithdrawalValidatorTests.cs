using BankingSystem.Api.Models;
using BankingSystem.Api.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace BankingSystem.Tests.Api.Validation;

public class AccountWithdrawalValidatorTests
{
    private readonly AccountWithdrawalValidator _sut;

    public AccountWithdrawalValidatorTests()
    {
        _sut = new AccountWithdrawalValidator();
    }

    [Fact]
    public void Should_return_valid_for_valid_AccountWithdrawal()
    {
        var validAccountDeposit = new AccountWithdrawal
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
        var validAccountCreation = new AccountWithdrawal
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
        var validAccountCreation = new AccountWithdrawal
        {
            Id = "id",
            Amount = amount,
        };

        var result = _sut.TestValidate(validAccountCreation);

        result.ShouldHaveValidationErrorFor(model => model.Amount);
    }
}
