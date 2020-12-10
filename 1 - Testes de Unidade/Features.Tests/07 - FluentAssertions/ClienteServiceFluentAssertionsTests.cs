using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Features.Clientes;
using MediatR;
using Moq;
using Xunit;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace Features.Tests
{
  [Collection(nameof(ClienteAutoMockerCollection))]
  public class ClienteServiceFluentAssertionsTests
  {
    private readonly ClienteTestsAutoMockerFixture _clienteTestsAutoMockerFixture;
    private readonly ClienteService _clienteService;

    public ClienteServiceFluentAssertionsTests(ClienteTestsAutoMockerFixture clienteTestsAutoMockerFixture)
    {
      _clienteTestsAutoMockerFixture = clienteTestsAutoMockerFixture;
      _clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();
    }

    [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
    [Trait("Fluent Assertion", "Cliente Service Fluent Assertion Tests")]
    public void ClienteService_Adicionar_DeveExecutarComSucesso()
    {
      // Arrange
      var cliente = _clienteTestsAutoMockerFixture.GerarClienteValido();
      // a linha abaixo está ficando repetida em todos os métodos de testes...
      //var clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();
      // para elimina-la, basta fazer no ctor

      // Act
      _clienteService.Adicionar(cliente);

      // Assert
      _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Once);
      _clienteTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Adicionar Cliente com Falha")]
    [Trait("Fluent Assertion", "Cliente Service Fluent Assertion Tests")]
    public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
    {
      // Arrange
      var cliente = _clienteTestsAutoMockerFixture.GerarClienteInvalido();
      //var clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();

      // Act
      _clienteService.Adicionar(cliente);

      // Assert
      _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Never);
      _clienteTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
    }

    [Fact(DisplayName = "Obter Clientes Ativos")]
    [Trait("Fluent Assertion", "Cliente Service Fluent Assertion Tests")]
    public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
    {
      // Arrange
      //var clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();

      _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
        .Returns(_clienteTestsAutoMockerFixture.ObterClientesVariados());

      // Act
      var clientes = _clienteService.ObterTodosAtivos(); // que vai internamente chama o método "ObterTodos" do repositório

      // Assert
      //Assert.True(clientes.Any());
      //Assert.False(clientes.Count(c => !c.Ativo) > 0);

      //clientes.Any().Should().BeTrue();
      //clientes.Count(c => !c.Ativo).Should().BeGreaterThan(0);

      // Fluent Assertion
      clientes.Should().NotContain(c => !c.Ativo);
      clientes.Should().HaveCountGreaterOrEqualTo(1).And.OnlyHaveUniqueItems();
      
      _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);

      // Extra: dá para falhar um teste se o tempo dele for maior que o desejado:
      _clienteService.ExecutionTimeOf(cs => cs.ObterTodosAtivos())
        .Should()
        .BeLessOrEqualTo(1.Milliseconds(),
        "demorou mais que 1 milisegundos");
    }
  }
}
