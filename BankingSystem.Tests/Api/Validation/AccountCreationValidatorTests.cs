using BankingSystem.Api.Models;
using BankingSystem.Api.Validation;
using BankingSystem.Domain;
using BankingSystem.Domain.ValueObjects;
using FluentValidation.TestHelper;
using Xunit;

namespace BankingSystem.Tests.Api.Validation;

public class AccountCreationValidatorTests
{
	private readonly AccountCreationValidator _sut;

	public AccountCreationValidatorTests()
	{
		_sut = new AccountCreationValidator();
	}

	[Fact]
	public void Should_return_valid_for_valid_AccountCreation()
	{
		var validAccountCreation = new AccountCreation
		{
			UserId = "someId",
			InitialBalance = Constants.MinimumBalance,
		};

		var result = _sut.TestValidate(validAccountCreation);

		result.ShouldNotHaveAnyValidationErrors();
	}

	[Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData(null)]
	public void Should_return_invalid_for_empty_userId(string userId)
	{
        var validAccountCreation = new AccountCreation
        {
            UserId = userId,
            InitialBalance = Constants.MinimumBalance,
        };

        var result = _sut.TestValidate(validAccountCreation);

        result.ShouldHaveValidationErrorFor(model => model.UserId);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-3)]
    public void Should_return_invalid_for_user_balance_lower_than_minimum(decimal amountDifferenceFromMinimum)
    {
        var initialBalance = new Money(Constants.MinimumBalance.Amount + amountDifferenceFromMinimum);
        var validAccountCreation = new AccountCreation
        {
            UserId = "someId",
            InitialBalance = initialBalance,
        };

        var result = _sut.TestValidate(validAccountCreation);

        if (amountDifferenceFromMinimum < 0)
        {
            result.ShouldHaveValidationErrorFor(model => model.InitialBalance);
        }
        else
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
