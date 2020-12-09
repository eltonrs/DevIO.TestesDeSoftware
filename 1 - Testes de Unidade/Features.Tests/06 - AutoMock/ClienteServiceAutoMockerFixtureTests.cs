using Xunit;
using Moq;
using Features.Clientes;
using MediatR;
using System.Threading;
using System.Linq;

namespace Features.Tests
{
  [Collection(nameof(ClienteAutoMockerCollection))]
  public class ClienteServiceAutoMockerFixtureTests
  {
    readonly ClienteTestsAutoMockerFixture _clienteTestsAutoMockerFixture;
    private readonly ClienteService _clienteService;

    public ClienteServiceAutoMockerFixtureTests(ClienteTestsAutoMockerFixture clienteTestsAutoMockFixture)
    {
      _clienteTestsAutoMockerFixture = clienteTestsAutoMockFixture;
      _clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();
    }

    [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
    [Trait("Mokando", "Cliente Service AutoMockFixture Tests")]
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
    [Trait("Mokando", "Cliente Service AutoMockFixture Tests")]
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
    [Trait("Mokando", "Cliente Service AutoMockFixture Tests")]
    public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
    {
      // Arrange
      //var clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();

      _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
        .Returns(_clienteTestsAutoMockerFixture.ObterClientesVariados());

      // Act
      var clientes = _clienteService.ObterTodosAtivos(); // que vai internamente chama o método "ObterTodos" do repositório

      // Assert
      _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once); // Não precisaria
      // o teste de fato é validar o resultado (clientes)
      Assert.True(clientes.Any());
      Assert.False(clientes.Count(c => !c.Ativo) > 0);
    }
  }
}
