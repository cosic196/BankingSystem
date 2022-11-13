using BankingSystem.Application.Exceptions;
using BankingSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Middlewares;

public class BankingSystemExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public BankingSystemExceptionHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			await _next(httpContext);
		}
		catch (Exception ex) when (
		ex is InvalidAccountBalanceException
		|| ex is InvalidDepositException
		|| ex is InvalidWithdrawalException)
		{
			await ReturnProblemDetailsAsync(httpContext, StatusCodes.Status400BadRequest, ex.Message);
		}
		catch (Exception ex) when (
		ex is AccountNotFoundException)
		{
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        }
	}

	private async Task ReturnProblemDetailsAsync(HttpContext httpContext, int status, string details)
	{
        var problemDetails = new ProblemDetails
        {
            Detail = details,
			Status = status,
		};

        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}
