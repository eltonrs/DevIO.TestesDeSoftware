using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Demo.Tests
{
  public class AssertingObjectTypesTests
  {
    [Fact]
    public void FuncionarioFactory_Criar_DeveRetornarTipoFuncionario()
    {
      // Arrange & Act
      var funcionario = FuncionarioFactory.Criar("Elton", 10000);

      // Assert
      Assert.IsType<Funcionario>(funcionario);
    }

    [Fact]
    public void FuncionarioFactory_Criar_DeveRetornarTipoDerivadoPessoa()
    {
      // Arrange & Act
      var funcionario = FuncionarioFactory.Criar("Elton", 10000);

      // Assert
      Assert.IsAssignableFrom<Pessoa>(funcionario);
    }
  }
}
