using BankingSystem.Application.CommandHandlers;
using BankingSystem.Application.Commands;
using BankingSystem.Domain.Repositories;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Application.CommandHandlers;

public sealed class DeleteAccountCommandHandlerTests : IDisposable
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly DeleteAccountCommandHandler _sut;

    public DeleteAccountCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _sut = new DeleteAccountCommandHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_delete_account_from_repository()
    {
        var deleteAccountCommand = new DeleteAccountCommand("id");

        await _sut.HandleAsync(deleteAccountCommand);

        _accountRepositoryMock.Verify(x => x.DeleteAsync(deleteAccountCommand.Id), Times.Once);
    }

    public void Dispose()
    {
        _accountRepositoryMock.VerifyNoOtherCalls();
    }
}
