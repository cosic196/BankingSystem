using BankingSystem.Application.CommandHandlers;
using BankingSystem.Application.Commands;
using BankingSystem.Application.Exceptions;
using BankingSystem.Domain;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Application.CommandHandlers;

public sealed class DepositToAccountCommandHandlerTests : IDisposable
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly DepositToAccountCommandHandler _sut;

    public DepositToAccountCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _sut = new DepositToAccountCommandHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_deposit_to_account_and_update_it_in_repository()
    {
        var depositToAccountCommand = new DepositToAccountCommand("id", 20);
        var account = new Account("id", "userId", Constants.MinimumBalance);
        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(depositToAccountCommand.Id))
            .ReturnsAsync(account);

        await _sut.HandleAsync(depositToAccountCommand);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(depositToAccountCommand.Id), Times.Once);
        _accountRepositoryMock.Verify(x => x.UpdateAsync(account), Times.Once);
        account.Balance.Should().Be(Constants.MinimumBalance + depositToAccountCommand.Amount);
    }

    [Fact]
    public async Task Should_throw_AccountNotFoundException_if_account_not_found()
    {
        var depositToAccountCommand = new DepositToAccountCommand("id", 20);
        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(depositToAccountCommand.Id))
            .ReturnsAsync((Account?)null);

        var commandHandling = async () => await _sut.HandleAsync(depositToAccountCommand);

        await commandHandling.Should().ThrowAsync<AccountNotFoundException>();
        _accountRepositoryMock.Verify(x => x.GetByIdAsync(depositToAccountCommand.Id), Times.Once);
    }

    public void Dispose()
    {
        _accountRepositoryMock.VerifyNoOtherCalls();
    }
}
