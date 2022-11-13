using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Queries;
using BankingSystem.Application.QueryHandlers;
using BankingSystem.Domain;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Application.CommandHandlers;

public sealed class GetAccountByIdQueryHandlerTests : IDisposable
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly GetAccountByIdQueryHandler _sut;

    public GetAccountByIdQueryHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _sut = new GetAccountByIdQueryHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_return_account_by_id()
    {
        var getAccountByIdQuery = new GetAccountByIdQuery("id");
        var account = new Account("id", "userId", Constants.MinimumBalance);
        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(getAccountByIdQuery.Id))
            .ReturnsAsync(account);

        var result = await _sut.HandleAsync(getAccountByIdQuery);

        _accountRepositoryMock.Verify(x => x.GetByIdAsync(getAccountByIdQuery.Id), Times.Once);
        result.Should().Be(account);
    }

    [Fact]
    public async Task Should_throw_AccountNotFoundException_if_account_not_found()
    {
        var getAccountByIdQuery = new GetAccountByIdQuery("id");
        _accountRepositoryMock
            .Setup(x => x.GetByIdAsync(getAccountByIdQuery.Id))
            .ReturnsAsync((Account?)null);

        var queryHandling = async () => await _sut.HandleAsync(getAccountByIdQuery);

        await queryHandling.Should().ThrowAsync<AccountNotFoundException>();
        _accountRepositoryMock.Verify(x => x.GetByIdAsync(getAccountByIdQuery.Id), Times.Once);
    }

    public void Dispose()
    {
        _accountRepositoryMock.VerifyNoOtherCalls();
    }
}
