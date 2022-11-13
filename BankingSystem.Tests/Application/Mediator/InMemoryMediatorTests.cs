using BankingSystem.Application.Abstractions;
using BankingSystem.Application.Mediator;
using BankingSystem.Tests.Application.Mediator.Fakes;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankingSystem.Tests.Application.Mediator;

public sealed class InMemoryMediatorTests : IDisposable
{
	private readonly Mock<ICommandHandler<FakeCommand>> _commandHandlerMock;
	private readonly Mock<IQueryHandler<FakeQuery, int>> _queryHandlerMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
	private readonly InMemoryMediator _sut;

	public InMemoryMediatorTests()
	{
		_commandHandlerMock = new Mock<ICommandHandler<FakeCommand>>();
		_queryHandlerMock = new Mock<IQueryHandler<FakeQuery, int>>();
		_serviceProviderMock = new Mock<IServiceProvider>();
		_serviceProviderMock
			.Setup(x => x.GetService(typeof(ICommandHandler<FakeCommand>)))
			.Returns(_commandHandlerMock.Object);
		_serviceProviderMock
			.Setup(x => x.GetService(typeof(IQueryHandler<FakeQuery, int>)))
			.Returns(_queryHandlerMock.Object);

		_sut = new InMemoryMediator(_serviceProviderMock.Object);
	}

	[Fact]
	public async Task Should_forward_command_to_injected_handler()
	{
		var command = new FakeCommand();

		await _sut.SendCommandAsync(command);

		_serviceProviderMock.Verify(x => x.GetService(typeof(ICommandHandler<FakeCommand>)), Times.Once);
		_commandHandlerMock.Verify(x => x.HandleAsync(command), Times.Once);
    }

	[Theory]
	[InlineData(12)]
	[InlineData(-50)]
	[InlineData(20)]
	[InlineData(123545)]
	public async Task Should_forward_query_to_injected_handler_and_return_its_result(int queryResult)
	{
        var query = new FakeQuery();
		_queryHandlerMock
			.Setup(x => x.HandleAsync(query))
			.ReturnsAsync(queryResult);

        var result = await _sut.SendQueryAsync<FakeQuery, int>(query);

        _serviceProviderMock.Verify(x => x.GetService(typeof(IQueryHandler<FakeQuery, int>)), Times.Once);
        _queryHandlerMock.Verify(x => x.HandleAsync(query), Times.Once);
		result.Should().Be(queryResult);
    }

    public void Dispose()
    {
		_commandHandlerMock.VerifyNoOtherCalls();
		_queryHandlerMock.VerifyNoOtherCalls();
		_serviceProviderMock.VerifyNoOtherCalls();
    }
}