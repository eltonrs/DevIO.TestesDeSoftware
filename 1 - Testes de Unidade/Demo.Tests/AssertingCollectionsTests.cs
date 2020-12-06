using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Demo.Tests
{
  public class AssertingCollectionsTests
  {
    [Fact]
    public void Funcionario_Habilidades_NaoDevePossuirHabilidadesVazias()
    {
      // Arrange & Act
      var funcionario = FuncionarioFactory.Criar("Elton", 10000);

      // Assert
      // Lendo: nas Habilidades (parametro 1), para cada uma (parametro 2) não pode estar vazio ou com espaços em branco (um Assert para cada uma das habilidades)
      Assert.All(funcionario.Habilidades, habilidades => Assert.False(string.IsNullOrWhiteSpace(habilidades)));
    }

    [Fact]
    public void Funcionario_Habilidades_JuniorDevePossuirHabilidadeBasica()
    {
      // Arrange & Act
      var funcionario = FuncionarioFactory.Criar("Elton", 10000);

      // Assert
      // Lendo: Contém um item (parametro 1) na lista (paraemtro 2)
      Assert.Contains("OOP", funcionario.Habilidades);
    }

    [Fact]
    public void Funcionario_Habilidades_JuniorNaoDevePossuirHabilidadesAvancadas()
    {
      // Arrange & Act
      var funcionario = FuncionarioFactory.Criar("Elton", 1500);

      // Assert
      // Lendo: Não contém o item (parametro 1) na lista (paraemtro 2)
      Assert.DoesNotContain("Microservicos", funcionario.Habilidades);
    }

    [Fact]
    public void Funcionario_Habilidades_SeniorDevePossuirTodasHabilidades()
    {
      // Arrange & Act
      var funcionario = FuncionarioFactory.Criar("Elton", 15000);

      var habilidadesBasicas = new[]
      {
        "Lógica de Programação",
        "OOP",
        "Testes",
        "Microservicos"
      };

      // Assert
      // Lendo: Comparando duas coleções
      Assert.Equal(habilidadesBasicas, funcionario.Habilidades);
    }
  }
}
