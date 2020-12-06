using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Demo.Tests
{
  public class AssertNullBoolTests
  {
    [Fact]
    public void Funcionario_Nome_NaoDeveSerNuloOuVazio()
    {
      // Arrange & Act
      var funcionario = new Funcionario("", 1000);

      // Assert
      Assert.False(string.IsNullOrEmpty(funcionario.Nome));
    }

    [Fact]
    public void Funcionario_Nome_NaoDeveterApelido()
    {
      // Arrange & Act
      var funcionario = new Funcionario("Elton", 1000);

      // Assert
      Assert.Null(funcionario.Apelido);

      // Assert Bool
      Assert.True(string.IsNullOrEmpty(funcionario.Apelido));
      Assert.False(funcionario.Apelido?.Length > 0);
    }
  }
}
