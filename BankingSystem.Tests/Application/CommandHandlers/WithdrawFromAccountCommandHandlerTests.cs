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

public sealed class WithdrawFromAccountCommandHandlerTests : IDisposable
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly WithdrawFromAccountCommandHandler _sut;

    public WithdrawFromAccountCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _sut = new WithdrawFromAccountCommandHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_deposit_to_account_and_update_it_in_repository()
    {
        var withdrawFromAccountCommand = new WithdrawFromAccountCommand("id", 20);
        var account = new Account("id", "userId", Constants.MinimumBalance + withdrawFromAccountCommand.Amount);
        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(account.Id))
            .ReturnsAsync(account);

        await _sut.HandleAsync(withdrawFromAccountCommand);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(account.Id), Times.Once);
        _accountRepositoryMock.Verify(x => x.UpdateAsync(account), Times.Once);
        account.Balance.Should().Be(Constants.MinimumBalance);
    }

    [Fact]
    public async Task Should_throw_AccountNotFoundException_if_account_not_found()
    {
        var withdrawFromAccountCommand = new WithdrawFromAccountCommand("id", 20);
        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(withdrawFromAccountCommand.Id))
            .ReturnsAsync((Account?)null);

        var commandHandling = async () => await _sut.HandleAsync(withdrawFromAccountCommand);

        await commandHandling.Should().ThrowAsync<AccountNotFoundException>();
        _accountRepositoryMock.Verify(x => x.GetByIdAsync(withdrawFromAccountCommand.Id), Times.Once);
    }

    public void Dispose()
    {
        _accountRepositoryMock.VerifyNoOtherCalls();
    }
}
