using BankingSystem.Api.Middlewares;
using BankingSystem.Application.Exceptions;
using BankingSystem.Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BankingSystem.Tests.Api.Middlewares;

public class BankingSystemExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task Should_return_400_with_problem_details_when_InvalidAccountBalanceException_is_thrown()
    {
        var exception = new InvalidAccountBalanceException();

        await VerifyMiddlewareForExceptionAsync(exception, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_return_400_with_problem_details_when_InvalidDepositException_is_thrown()
    {
        var exception = new InvalidDepositException();

        await VerifyMiddlewareForExceptionAsync(exception, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_return_400_with_problem_details_when_InvalidWithdrawalException_is_thrown()
    {
        var exception = new InvalidWithdrawalException();

        await VerifyMiddlewareForExceptionAsync(exception, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_return_404_when_AccountNotFoundException_is_thrown()
    {
        const string Id = "id";
        var exception = new AccountNotFoundException(Id);
        var host = await GetHostAsync(exception);
        var httpClient = host.GetTestClient();

        var response = await httpClient.GetAsync(string.Empty);
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        content.Should().BeEmpty();
    }

    private async Task VerifyMiddlewareForExceptionAsync(Exception exception, HttpStatusCode statusCode)
    {
        var host = await GetHostAsync(exception);
        var httpClient = host.GetTestClient();

        var response = await httpClient.GetAsync(string.Empty);
        var content = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        if (content == null)
        {
            throw new NullReferenceException("Content was null.");
        }
        content.Status.Should().Be((int)statusCode);
        content.Detail.Should().Be(exception.Message);
    }

    private async Task<IHost> GetHostAsync(Exception exceptionToThrow)
    {
        return await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .Configure(app =>
                    {
                        app.UseMiddleware<BankingSystemExceptionHandlingMiddleware>();
                        app.Run(httpContext =>
                        {
                            throw exceptionToThrow;
                        });
                    });
            })
            .StartAsync();
    }
}
