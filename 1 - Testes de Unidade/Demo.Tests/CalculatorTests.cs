using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Demo;

namespace Demo.Tests
{
  public class CalculatorTests
  {
    [Fact]
    public void Calculator_Sum_ReturnSumValue()
    {
      // Arrange
      var calculator = new Calculator();

      // Act
      var result = calculator.Sum(2, 2);

      // Assert
      Assert.Equal(4, result); // Com Equal é mais expressiva a mensagem caso dê erro, Exemplo: esperado 4, deu 6.
      //Assert.True(result == 4); // Já o True só mostra True ou False.
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(3, 1, 4)]
    [InlineData(5, 5, 10)]
    [InlineData(100, 1, 101)]
    [InlineData(50, -50, 0)]
    // dá pra pegar os dados de fonte4s externas (Excel, banco de dados e etc.)
    public void Calculator_Sum_ReturnCorrectlyValues(double v1, double v2, double total)
    {
      // Arrange
      var calculator = new Calculator();

      // Act
      var result = calculator.Sum(v1, v2);

      // Assert
      Assert.Equal(total, result); // Com Equal é mais expressiva a mensagem caso dê erro, Exemplo: esperado 4, deu 6.
    }
  }
}
