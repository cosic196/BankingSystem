using BankingSystem.Api.Models;
using BankingSystem.Application.Commands;
using BankingSystem.Application.Mediator;
using BankingSystem.Application.Queries;
using BankingSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Controllers;


[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountDetails>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<AccountDetails>>> GetAllAsync()
    {
        var result = await _mediator.SendQueryAsync<GetAllAccountsQuery, IEnumerable<Account>>(new GetAllAccountsQuery());
        return Ok(result.ToApiModel());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountDetails), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountDetails>> GetByIdAsync([FromRoute, Required(AllowEmptyStrings = false)] string id)
    {
        var result = await _mediator.SendQueryAsync<GetAccountByIdQuery, Account>(new GetAccountByIdQuery(id));
        return Ok(result.ToApiModel());
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccountCreationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AccountCreationResult>> CreateAsync([FromBody, Required] AccountCreation accountCreation)
    {
        var uniqueId = Guid.NewGuid().ToString();
        await _mediator.SendCommandAsync(accountCreation.ToCommand(uniqueId));
        return Ok(new AccountCreationResult { Id = uniqueId });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteAsync([FromRoute, Required(AllowEmptyStrings = false)] string id)
    {
        await _mediator.SendCommandAsync(new DeleteAccountCommand(id));
        return NoContent();
    }

    [HttpPatch("withdraw")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> WithdrawAsync([FromBody, Required] AccountWithdrawal accountWithdrawal)
    {
        await _mediator.SendCommandAsync(accountWithdrawal.ToCommand());
        return Ok();
    }

    [HttpPatch("deposit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DepositAsync([FromBody, Required] AccountDeposit accountDeposit)
    {
        await _mediator.SendCommandAsync(accountDeposit.ToCommand());
        return Ok();
    }
}
