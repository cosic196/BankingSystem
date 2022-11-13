using BankingSystem.Application.Queries;
using BankingSystem.Application.QueryHandlers;
using BankingSystem.Domain;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Application.CommandHandlers;

public sealed class GetAllAccountsQueryHandlerTests : IDisposable
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly GetAllAccountsQueryHandler _sut;

    public GetAllAccountsQueryHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _sut = new GetAllAccountsQueryHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_return_all_accounts_from_repository()
    {
        var getAllAccountsQuery = new GetAllAccountsQuery();
        var account = new Account("id", "userId", Constants.MinimumBalance);
        _accountRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new[] { account });

        var result = await _sut.HandleAsync(getAllAccountsQuery);

        _accountRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        result.Should().BeEquivalentTo(new[] { account });
    }

    public void Dispose()
    {
        _accountRepositoryMock.VerifyNoOtherCalls();
    }
}
