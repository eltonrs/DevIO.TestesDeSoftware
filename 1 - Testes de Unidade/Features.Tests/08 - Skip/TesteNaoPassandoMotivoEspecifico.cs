using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Features.Tests
{
  public class TesteNaoPassandoMotivoEspecifico
  {
    /* Lendo:
     */
    [Fact(DisplayName = "Novo Cliente 2.0", Skip = "Esse teste está quebrando na versão 2.0. Então está sendo ignorado.")]
    [Trait("Categoria", "Escapando dos testes")]
    public void Teste_NaoEstaPassando_VersaoNovaNaoCompativel()
    {
      // Arrange
      // Act
      // Assert
      Assert.True(false);
    }
  }
}
