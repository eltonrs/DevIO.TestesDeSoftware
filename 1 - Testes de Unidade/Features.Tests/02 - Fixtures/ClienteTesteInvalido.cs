using Xunit;

namespace Features.Tests
{
  [Collection(nameof(ClienteCollection))] // todas as classes de teste que utilizando a coleção, devem indicar aqui <<
  public class ClienteTesteInvalido
  {
    private readonly ClienteTestsFixture _clienteTestsFixture;

    public ClienteTesteInvalido(ClienteTestsFixture clienteTestsFixture) // injeção de dependência
    {
      _clienteTestsFixture = clienteTestsFixture;
    }

    [Fact(DisplayName = "Novo Cliente Inválido")]
    [Trait("Cliente", "Fixture")]
    public void Cliente_NovoCliente_DeveEstarInvalido()
    {
      // Arrange
      var cliente = _clienteTestsFixture.GerarClienteInvalido();

      // Act
      var result = cliente.EhValido();

      // Assert 
      Assert.False(result);
      Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
    }
  }
}