using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace Features.Tests
{
  [Collection(nameof(ClienteAutoMockerCollection))]
  public class ClienteFluentAssertionsTests
  {
    private readonly ClienteTestsAutoMockerFixture _clienteTestsAutoMockerFixture;
    public ClienteFluentAssertionsTests(ClienteTestsAutoMockerFixture clienteTestsAutoMockerFixture)
    {
      _clienteTestsAutoMockerFixture = clienteTestsAutoMockerFixture;
    }

    [Fact(DisplayName = "Novo Cliente Válido")]
    [Trait("FluentAssertion", "Cliente FluentAssertion Test")]
    public void Cliente_NovoCliente_DeveEstarValido()
    {
      // Arrange
      var cliente = _clienteTestsAutoMockerFixture.GerarClienteValido();

      // Act
      var result = cliente.EhValido();

      // Assert
      //Assert.True(result);
      //Assert.Equal(0, cliente.ValidationResult.Errors.Count);

      // Fluent Assertion
      result.Should().BeTrue("Cliente não é válido");
      cliente.ValidationResult.Errors.Should().HaveCount(0, "Ocorrem erros na validação do cliente");
    }

    [Fact(DisplayName = "Novo Cliente Inválido")]
    [Trait("FluentAssertion", "Cliente FluentAssertion Test")]
    public void Cliente_NovoCliente_DeveEstarInvalido()
    {
      // Arrange
      var cliente = _clienteTestsAutoMockerFixture.GerarClienteInvalido();

      // Act
      var result = cliente.EhValido();

      // Assert
      //Assert.False(result);
      //Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);

      // Fluent Assertion
      result.Should().BeFalse();
      cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1);
    }
  }
}
