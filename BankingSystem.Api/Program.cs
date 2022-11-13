using BankingSystem.Api.Middlewares;
using BankingSystem.Application.Abstractions;
using BankingSystem.Application.CommandHandlers;
using BankingSystem.Application.Commands;
using BankingSystem.Application.Mediator;
using BankingSystem.Application.Queries;
using BankingSystem.Application.QueryHandlers;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Repositories;
using BankingSystem.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BankingSystem.Api;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
#pragma warning disable CS0618 // MicroElements.Swashbuckle.FluentValidation.AspNetCore workaround: https://github.com/micro-elements/MicroElements.Swashbuckle.FluentValidation/issues/97
        builder.Services.TryAddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
#pragma warning restore CS0618
        builder.Services.AddFluentValidationRulesToSwagger();

        builder.Services.AddScoped<IMediator, InMemoryMediator>();
        builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();

        builder.Services.AddScoped<ICommandHandler<CreateAccountCommand>, CreateAccountCommandHandler>();
        builder.Services.AddScoped<ICommandHandler<DeleteAccountCommand>, DeleteAccountCommandHandler>();
        builder.Services.AddScoped<ICommandHandler<DepositToAccountCommand>, DepositToAccountCommandHandler>();
        builder.Services.AddScoped<ICommandHandler<WithdrawFromAccountCommand>, WithdrawFromAccountCommandHandler>();

        builder.Services.AddScoped<IQueryHandler<GetAccountByIdQuery, Account>, GetAccountByIdQueryHandler>();
        builder.Services.AddScoped<IQueryHandler<GetAllAccountsQuery, IEnumerable<Account>>, GetAllAccountsQueryHandler>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMvc();
        builder.Services.AddFluentValidationAutoValidation();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.UseMiddleware<BankingSystemExceptionHandlingMiddleware>();

        app.MapControllers();

        app.Run();
    }
}