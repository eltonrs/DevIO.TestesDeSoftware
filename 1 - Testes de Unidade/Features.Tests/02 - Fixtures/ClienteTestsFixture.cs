using System;
using Features.Clientes;
using Xunit;

namespace Features.Tests
{
  [CollectionDefinition(nameof(ClienteCollection))]
  public class ClienteCollection : ICollectionFixture<ClienteTestsFixture>
  {}

  public class ClienteTestsFixture : IDisposable
  {
    public Cliente clienteValido;

    public ClienteTestsFixture() 
    {
      /* No constructor, posso definir os objetos que vou utilizar, fazem-se desnecessário a utilizando do "Arrange" toda vez.
       * 
       A cada método de teste, a classe é instanciada, então o constructor é chamado. Sendo assim, o objeto e recriado para cada método de testes. Isso é bom para que não haja compartilhamento de informações entre vários testes (distintos).
      Ponto negativo: as vezes faz-se necessário manter o "estado" do objeto para N testes. Para isso há uma solução.
      */

      clienteValido = new Cliente(
        Guid.NewGuid(),
        "Elton",
        "Souza",
        DateTime.Now.AddYears(-37),
        "elton@souza.com",
        true,
        DateTime.Now);
    }

    [Trait("Cliente", "Fixture")]
    [Fact(DisplayName = "Gerar Valido")]
    public Cliente GerarClienteValido()
    {
      var cliente = new Cliente(
        Guid.NewGuid(),
        "Eduardo",
        "Pires",
        DateTime.Now.AddYears(-30),
        "edu@edu.com",
        true,
        DateTime.Now);

      return cliente;
    }

    [Trait("Cliente", "Fixture")]
    [Fact(DisplayName = "Gerar Invalido")]
    public Cliente GerarClienteInValido()
    {
      var cliente = new Cliente(
        Guid.NewGuid(),
        "",
        "",
        DateTime.Now,
        "edu2edu.com",
        true,
        DateTime.Now);

      return cliente;
    }

    public void Dispose()
    {
    }
  }
}