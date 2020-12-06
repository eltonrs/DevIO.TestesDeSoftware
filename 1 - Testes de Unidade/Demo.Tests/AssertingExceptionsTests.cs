using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Demo.Tests
{
  public class AssertingExceptionsTests
  {
    [Fact]
    public void Calculator_Divisor_DeveRetornarErroDivisaoPorZero()
    {
      // Arrange
      var calculator = new Calculator();

      // Act & Assert
      // Lendo: não tem como fazer o Act separado, pq iria gerar a excessão
      Assert.Throws<DivideByZeroException>(() => calculator.Divisor(10, 0));
    }

    [Fact]
    public void Funcionario_Salario_DeveRetornarErroSalarioInferiorPermitido()
    {
      // Arrange & Act & Assert
      // Lendo: posso "guardar" o resultado do Assert
      var exception = Assert.Throws<Exception>(() => FuncionarioFactory.Criar("Elton", 250));

      Assert.Equal("Salario inferior ao permitido.", exception.Message);
    }
  }
}
