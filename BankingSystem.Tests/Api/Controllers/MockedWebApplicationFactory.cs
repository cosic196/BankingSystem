using BankingSystem.Api.Models;
using BankingSystem.Application.Mediator;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace BankingSystem.Tests.Api.Controllers;

public class MockedWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public Mock<IMediator> MediatorMock { get; private set; } = new Mock<IMediator>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped(_ => MediatorMock.Object);
            services.RemoveAll<AbstractValidator<AccountCreation>>();
            services.RemoveAll<AbstractValidator<AccountDeposit>>();
            services.RemoveAll<AbstractValidator<AccountWithdrawal>>();
        });
    }
}
