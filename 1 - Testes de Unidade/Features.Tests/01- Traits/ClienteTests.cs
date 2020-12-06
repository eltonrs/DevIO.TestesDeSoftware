using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Features.Clientes;

namespace Features.Tests
{
  public class ClienteTests
  {
    [Fact(DisplayName = "Cliente - Método Novo - Valido")] // O nome amigável/explicável que aparece no "Test Explorer"
    [Trait("Cliente", "Testes de criação")] // Organizando por categorias. No Test Explorer, só usar o Agrupamento
    public void Cliente_NovoCliente_DeveEstarValido()
    {
      // Arrange
      var cliente = new Cliente(
          Guid.NewGuid(),
          "Elton",
          "Souza",
          DateTime.Now.AddYears(-37),
          "elton@souza.com",
          true,
          DateTime.Now);

      // Act
      var result = cliente.EhValido();

      // Assert 
      Assert.True(result);
      Assert.Equal(0, cliente.ValidationResult.Errors.Count);
    }

    [Fact(DisplayName = "Cliente - Método Novo - Invalido")]
    [Trait("Cliente", "Testes de criação")] // o mesmo do de cima
    public void Cliente_NovoCliente_DeveEstarInvalido()
    {
      // Arrange
      var cliente = new Cliente(
          Guid.NewGuid(),
          "",
          "",
          DateTime.Now,
          "elton.souza.com",
          true,
          DateTime.Now);

      // Act
      var result = cliente.EhValido();

      // Assert 
      Assert.False(result);
      Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
    }
  }
}
