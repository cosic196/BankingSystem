using BankingSystem.Api;
using BankingSystem.Api.Models;
using BankingSystem.Domain;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BankingSystem.Tests.Api.IntegrationTests;

[Trait("Category", "Integration")]
public class AccountControllerIntegrationTests
{
    private const string ApiBaseRoute = "/api/accounts";

    private readonly HttpClient _httpClient;

    public AccountControllerIntegrationTests()
    {
        var factory = new WebApplicationFactory<Program>();
        _httpClient = factory.CreateClient();
    }

    [Theory(DisplayName = "A user can have as many accounts as they want.")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(100)]
    [InlineData(2)]
    public async Task A_user_can_have_as_many_accounts_as_they_want(int numberOfAccounts)
    {
        var accountCreationModels = GenerateRandomAccountCreationModel(numberOfAccounts);

        foreach (var model in accountCreationModels)
        {
            var creationResponse = await CreateAccountAsync(model);
            var createdAccountId = await GetCreatedAccountIdAsync(creationResponse);
            var getAccountResponse = await GetAccountAsync(createdAccountId);
            getAccountResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var accountDetails = await getAccountResponse.Content.ReadFromJsonAsync<AccountDetails>() ?? throw new NullReferenceException("Content was null.");
            accountDetails.Id.Should().Be(createdAccountId);
            accountDetails.UserId.Should().Be(model.UserId);
            accountDetails.AvailableBalance.Should().Be(model.InitialBalance);
        }
    }

    [Fact(DisplayName = "A user can create and delete accounts.")]
    public async Task A_user_can_create_and_delete_accounts()
    {
        var accountCreationModel = GenerateRandomAccountCreationModel();

        var creationResponse = await CreateAccountAsync(accountCreationModel);
        var createdAccountId = await GetCreatedAccountIdAsync(creationResponse);
        (await DeleteAccountAsync(createdAccountId)).EnsureSuccessStatusCode();
        var getAccountResponse = await GetAccountAsync(createdAccountId);
        getAccountResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(DisplayName = "A user can deposit and withdraw from accounts.")]
    public async Task A_user_can_deposit_and_withdraw_from_accounts()
    {
        var accountCreationModel = GenerateRandomAccountCreationModel();

        var creationResponse = await CreateAccountAsync(accountCreationModel);
        var createdAccountId = await GetCreatedAccountIdAsync(creationResponse);

        var accountDepositModel = new Faker<AccountDeposit>()
            .CustomInstantiator(f => new AccountDeposit
            {
                Id = createdAccountId,
                Amount = f.Random.Decimal(1, Constants.MinimumBalance)
            })
            .Generate();
        var accountWithdrawalModel = new Faker<AccountWithdrawal>()
            .CustomInstantiator(f => new AccountWithdrawal
            {
                Id = createdAccountId,
                Amount = f.Random.Decimal(1, Constants.MinimumBalance)
            })
            .Generate();
        (await DepositToAccountAsync(accountDepositModel)).EnsureSuccessStatusCode();
        (await WithdrawFromAccountAsync(accountWithdrawalModel)).EnsureSuccessStatusCode();

        var getAccountResponse = await GetAccountAsync(createdAccountId);
        getAccountResponse.EnsureSuccessStatusCode();
        var accountDetails = await getAccountResponse.Content.ReadFromJsonAsync<AccountDetails>() ?? throw new NullReferenceException("Content was null.");
        accountDetails.AvailableBalance.Should().Be(accountCreationModel.InitialBalance + accountDepositModel.Amount - accountWithdrawalModel.Amount);
    }

    [Fact(DisplayName = $"An account cannot have less than 100$ at any time in an account - creation.")]
    public async Task An_account_cannot_have_less_than_minimum_at_any_time_in_an_account_creation()
    {
        var accountCreationModel = new AccountCreation
        {
            InitialBalance = Constants.MinimumBalance - 1,
            UserId = "userId",
        };

        var creationResponse = await CreateAccountAsync(accountCreationModel);

        creationResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = $"An account cannot have less than 100$ at any time in an account - withdrawal.")]
    public async Task An_account_cannot_have_less_than_minimum_at_any_time_in_an_account_withdrawal()
    {
        var accountCreationModel = GenerateRandomAccountCreationModel();

        var creationResponse = await CreateAccountAsync(accountCreationModel);
        var createdAccountId = await GetCreatedAccountIdAsync(creationResponse);

        var accountWithdrawalModel = new AccountWithdrawal
        {
            Id = createdAccountId,
            Amount = accountCreationModel.InitialBalance + Constants.MinimumBalance - 1,
        };

        var withdrawFromAccountResponse = await WithdrawFromAccountAsync(accountWithdrawalModel);
        withdrawFromAccountResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "A user cannot withdraw more than 90% of their total balance from an account in a single transaction.")]
    public async Task A_user_cannot_withdraw_more_than_max_allowed()
    {
        var accountCreationModel = new AccountCreation
        {
            InitialBalance = Constants.MinimumBalance * 10,
            UserId = "userId",
        };

        var creationResponse = await CreateAccountAsync(accountCreationModel);
        var createdAccountId = await GetCreatedAccountIdAsync(creationResponse);

        var accountWithdrawalModel = new AccountWithdrawal
        {
            Id = createdAccountId,
            Amount = accountCreationModel.InitialBalance * Constants.MaxBalanceWithdrawal + 1,
        };

        var withdrawFromAccountResponse = await WithdrawFromAccountAsync(accountWithdrawalModel);
        withdrawFromAccountResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "A user cannot deposit more than $10,000 in a single transaction.")]
    public async Task A_user_cannot_deposit_more_than_max_allowed()
    {
        var accountCreationModel = GenerateRandomAccountCreationModel();

        var creationResponse = await CreateAccountAsync(accountCreationModel);
        var createdAccountId = await GetCreatedAccountIdAsync(creationResponse);

        var accountDepositModel = new AccountDeposit
        {
            Id = createdAccountId,
            Amount = Constants.MaxDeposit + 1,
        };

        var depositToAccountResponse = await DepositToAccountAsync(accountDepositModel);
        depositToAccountResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private IEnumerable<AccountCreation> GenerateRandomAccountCreationModel(int number)
    {
        return new Faker<AccountCreation>()
            .CustomInstantiator(faker => new AccountCreation
            {
                InitialBalance = faker.Random.Decimal(Constants.MinimumBalance * 2, Constants.MinimumBalance * 10),
                UserId = Guid.NewGuid().ToString(),
            })
            .Generate(number);
    }

    private AccountCreation GenerateRandomAccountCreationModel()
    {
        return GenerateRandomAccountCreationModel(1).First();
    }

    private async Task<HttpResponseMessage> GetAccountAsync(string id)
    {
        return await _httpClient.GetAsync($"{ApiBaseRoute}/{id}");
    }

    private async Task<HttpResponseMessage> CreateAccountAsync(AccountCreation accountCreation)
    {
        return await _httpClient.PostAsJsonAsync(ApiBaseRoute, accountCreation);
    }

    private async Task<HttpResponseMessage> DeleteAccountAsync(string id)
    {
        return await _httpClient.DeleteAsync($"{ApiBaseRoute}/{id}");
    }

    private async Task<HttpResponseMessage> DepositToAccountAsync(AccountDeposit accountDeposit)
    {
        return await _httpClient.PatchAsync($"{ApiBaseRoute}/deposit", JsonContent.Create(accountDeposit));
    }

    private async Task<HttpResponseMessage> WithdrawFromAccountAsync(AccountWithdrawal accountWithdrawal)
    {
        return await _httpClient.PatchAsync($"{ApiBaseRoute}/withdraw", JsonContent.Create(accountWithdrawal));
    }

    private async Task<string> GetCreatedAccountIdAsync(HttpResponseMessage creationResponse)
    {
        creationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var creationResponseContent = await creationResponse.Content.ReadFromJsonAsync<AccountCreationResult>() ?? throw new NullReferenceException("Content was null.");
        return creationResponseContent.Id ?? throw new NullReferenceException($"{nameof(creationResponseContent.Id)} was null.");
    }
}
