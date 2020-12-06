using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Demo.Tests
{
  public class AssertStringsTests
  {
    [Fact]
    public void StringsTools_UnirNomes_RetornarNomeCompleto()
    {
      // Arrange
      var sut = new StringsTools();

      // Act
      var nomeCompleto = sut.Unir("Elton", "Souza");

      // Assert
      Assert.Equal("Elton Souza", nomeCompleto);
    }

    [Fact]
    public void StringsTools_UnirNomes_DeveIgnorarCase()
    {
      // Arrange
      var sut = new StringsTools();

      // Act
      var nomeCompleto = sut.Unir("Elton", "Souza");

      // Assert
      Assert.Equal("ELTON SOUZA", nomeCompleto, true);
    }

    [Fact]
    public void StringsTools_UnirNomes_DeveConterTrecho()
    {
      // Arrange
      var sut = new StringsTools();

      // Act
      var nomeCompleto = sut.Unir("Elton", "Souza");

      // Assert
      Assert.Contains("ton", nomeCompleto);
    }

    [Fact]
    public void StringsTools_UnirNomes_DeveComecarCom()
    {
      // Arrange
      var sut = new StringsTools();

      // Act
      var nomeCompleto = sut.Unir("Elton", "Souza");

      // Assert
      Assert.StartsWith("Elton", nomeCompleto);
    }

    [Fact]
    public void StringsTools_UnirNomes_DeveAcabarCom()
    {
      // Arrange
      var sut = new StringsTools();

      // Act
      var nomeCompleto = sut.Unir("Elton", "Souza");

      // Assert
      Assert.EndsWith("uza", nomeCompleto);
    }

    [Fact]
    public void StringsTools_UnirNomes_ValidarExpressaoregular()
    {
      // Arrange
      var sut = new StringsTools();

      // Act
      var nomeCompleto = sut.Unir("Elton", "Souza");

      // Assert
      Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", nomeCompleto);
    }
  }
}
