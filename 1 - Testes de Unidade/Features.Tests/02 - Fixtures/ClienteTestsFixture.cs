using System;
using Features.Clientes;
using Xunit;

namespace Features.Tests
{
  /* (***) O objeto do fixture é preparado antes de instanciar a classe de testes e destruído somente depois que terminar todos os testes.
   */

  [CollectionDefinition(nameof(ClienteCollection))]
  public class ClienteCollection : ICollectionFixture<ClienteTestsFixture>
  {
    
  }

  /* Lendo: por essa classe ser reaproveitavel entre as outras classes, ela implementada o IDisposible.
   * Uma vez que ela implementada o IDisposible, ela deve ter o método "Dispose".
   */
  public class ClienteTestsFixture : IDisposable
  {
    /* Forma 1 de utilizar o a feature Fixture: através de Constructors
      
     * No constructor, posso definir os objetos que vou utilizar, fazem-se desnecessário a utilizando do "Arrange" toda vez.
     * A cada método de teste, a classe é instanciada, então o constructor é chamado. Sendo assim, o objeto e recriado para cada método de testes. Isso é bom para que não haja compartilhamento de informações entre vários testes (distintos).
     * Ponto negativo: as vezes faz-se necessário manter o "estado" do objeto para N testes. Para isso há uma solução.
     * 
     * Forma 2: a que está implementada na versão final da classe (***)
     */

    public Cliente GerarClienteValido()
    {
      var cliente = new Cliente(
        Guid.NewGuid(),
        "Elton",
        "Souza",
        DateTime.Now.AddYears(-37),
        "elton@souza.com",
        true,
        DateTime.Now);

      return cliente;
    }

    public Cliente GerarClienteInvalido()
    {
      var cliente = new Cliente(
        Guid.NewGuid(),
        "",
        "",
        DateTime.Now,
        "elton.souza.com",
        true,
        DateTime.Now);

      return cliente;
    }

    public void Dispose()
    {
      /* Utilizado, por exemplo, para destruir banco de dados em memória (que podem ter sido criados para serem utilizados em todos os testes).
       */
    }
  }
}