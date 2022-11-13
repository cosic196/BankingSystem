using BankingSystem.Api;
using BankingSystem.Api.Models;
using BankingSystem.Application.Commands;
using BankingSystem.Application.Queries;
using BankingSystem.Domain;
using BankingSystem.Domain.Entities;
using FluentAssertions;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BankingSystem.Tests.Api.Controllers;

public class AccountsControllerTests
{
    private const string ApiBaseRoute = "/api/accounts";

    private readonly MockedWebApplicationFactory<Program> _factory;

    public AccountsControllerTests()
    {
        _factory = new MockedWebApplicationFactory<Program>();
    }

    [Fact]
    public async Task Get_Sends_query_to_get_all_accounts_and_returns_query_response()
    {
        var returnedAccounts = new[]
        {
            new Account("id1", "userId", Constants.MinimumBalance),
            new Account("id2", "userId", Constants.MinimumBalance),
        };

        _factory.MediatorMock
            .Setup(x => x.SendQueryAsync<GetAllAccountsQuery, IEnumerable<Account>>(It.IsAny<GetAllAccountsQuery>()))
            .ReturnsAsync(returnedAccounts);
        var client = _factory.CreateClient();

        var response = await client.GetAsync(ApiBaseRoute);
        var responseAccountDetails = await response.Content.ReadFromJsonAsync<IEnumerable<AccountDetails>>();

        _factory.MediatorMock.Verify(x => x.SendQueryAsync<GetAllAccountsQuery, IEnumerable<Account>>(It.IsAny<GetAllAccountsQuery>()), Times.Once);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseAccountDetails.Should().BeEquivalentTo(returnedAccounts.ToApiModel());
    }

    [Fact]
    public async Task GetById_Sends_query_to_get_account_by_id_and_returns_query_response()
    {
        var returnedAccount = new Account("id1", "userId", Constants.MinimumBalance);

        _factory.MediatorMock
            .Setup(x => x.SendQueryAsync<GetAccountByIdQuery, Account>(It.IsAny<GetAccountByIdQuery>()))
            .ReturnsAsync(returnedAccount);
        var client = _factory.CreateClient();

        var response = await client.GetAsync($"{ApiBaseRoute}/{returnedAccount.Id}");
        var responseAccountDetails = await response.Content.ReadFromJsonAsync<AccountDetails>();

        _factory.MediatorMock.Verify(x => x.SendQueryAsync<GetAccountByIdQuery, Account>(It.IsAny<GetAccountByIdQuery>()), Times.Once);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseAccountDetails.Should().BeEquivalentTo(returnedAccount.ToApiModel());
    }

    [Fact]
    public async Task Create_Sends_command_to_create_account()
    {
        var client = _factory.CreateClient();
        var model = new AccountCreation
        {
            InitialBalance = Constants.MinimumBalance,
            UserId = "userId",
        };

        var response = await client.PostAsJsonAsync($"{ApiBaseRoute}", model);

        _factory.MediatorMock
            .Verify(x => x.SendCommandAsync(It.Is<CreateAccountCommand>(command => command.InitialBalance == model.InitialBalance && command.UserId == model.UserId)), Times.Once);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_Sends_command_to_delete_account()
    {
        const string Id = "testId";
        var client = _factory.CreateClient();

        var response = await client.DeleteAsync($"{ApiBaseRoute}/{Id}");

        _factory.MediatorMock
            .Verify(x => x.SendCommandAsync(It.Is<DeleteAccountCommand>(command => command.Id == Id)), Times.Once);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Withdraw_Sends_command_to_withdraw_from_account()
    {
        var client = _factory.CreateClient();
        var model = new AccountWithdrawal
        {
            Amount = Constants.MinimumBalance,
            Id = "accountId",
        };

        var response = await client.PatchAsync($"{ApiBaseRoute}/withdraw", JsonContent.Create(model));

        _factory.MediatorMock
            .Verify(x => x.SendCommandAsync(It.Is<WithdrawFromAccountCommand>(command => command.Amount == model.Amount && command.Id == model.Id)), Times.Once);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Deposit_Sends_command_to_deposit_to_account()
    {
        var client = _factory.CreateClient();
        var model = new AccountDeposit
        {
            Amount = Constants.MinimumBalance,
            Id = "accountId",
        };

        var response = await client.PatchAsync($"{ApiBaseRoute}/deposit", JsonContent.Create(model));

        _factory.MediatorMock
            .Verify(x => x.SendCommandAsync(It.Is<DepositToAccountCommand>(command => command.Amount == model.Amount && command.Id == model.Id)), Times.Once);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
