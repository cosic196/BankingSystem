using BankingSystem.Application.CommandHandlers;
using BankingSystem.Application.Commands;
using BankingSystem.Domain;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Application.CommandHandlers;

public sealed class CreateAccountCommandHandlerTests : IDisposable
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly CreateAccountCommandHandler _sut;

    public CreateAccountCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _sut = new CreateAccountCommandHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_insert_account_to_repository()
    {
        var createAccountCommand = new CreateAccountCommand("id", "userId", Constants.MinimumBalance);

        await _sut.HandleAsync(createAccountCommand);

        _accountRepositoryMock
            .Verify(x => x.InsertAsync(It.Is<Account>(acc => 
                acc.Id == createAccountCommand.Id && 
                acc.UserId == createAccountCommand.UserId && 
                acc.Balance == createAccountCommand.InitialBalance)),
            Times.Once);
    }

    public void Dispose()
    {
        _accountRepositoryMock.VerifyNoOtherCalls();
    }
}
