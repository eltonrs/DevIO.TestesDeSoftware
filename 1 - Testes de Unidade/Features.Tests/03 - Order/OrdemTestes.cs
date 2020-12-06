using Xunit;

namespace Features.Tests
{
  /* Ordenação de testes
   * Para teste de unidade, não faz sentido. ERRADO pensar dessa forma.
   * Para testes de integração, pode até fazer. Visto que podem existem dependencias entre features.
   * 
   * Para isso, é simples:
   * 1 - Indicar, através de um atributo de classe, que a classe possui métodos priorizados e qual a classe que vai fazer a execução dos métodos de forma priorizada, através do atributo "TestCaseOrderer";
   * 2 - Para cada método, utilizar o atributo "TestPriorit(N)" para definir a prioridade de execução.
   */

  [TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
  public class OrdemTestes
  {
    public static bool Teste1Chamado;
    public static bool Teste2Chamado;
    public static bool Teste3Chamado;
    public static bool Teste4Chamado;

    [TestPriority(3)]
    [Fact(DisplayName = "Teste 04")]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste04()
    {
      Teste4Chamado = true;

      Assert.True(Teste3Chamado);
      Assert.True(Teste1Chamado);
      Assert.False(Teste2Chamado);
    }

    [TestPriority(2)]
    [Fact(DisplayName = "Teste 01")]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste01()
    {
      Teste1Chamado = true;

      Assert.True(Teste3Chamado);
      Assert.False(Teste4Chamado);
      Assert.False(Teste2Chamado);
    }

    [TestPriority(1)]
    [Fact(DisplayName = "Teste 03")]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste03()
    {
      Teste3Chamado = true;

      Assert.False(Teste1Chamado);
      Assert.False(Teste2Chamado);
      Assert.False(Teste4Chamado);
    }

    [TestPriority(4)]
    [Fact(DisplayName = "Teste 02")]
    [Trait("Categoria", "Ordenacao Testes")]
    public void Teste02()
    {
      Teste2Chamado = true;

      Assert.True(Teste3Chamado);
      Assert.True(Teste4Chamado);
      Assert.True(Teste1Chamado);
    }
  }
}