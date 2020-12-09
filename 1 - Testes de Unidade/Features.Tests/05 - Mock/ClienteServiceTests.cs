using Features.Clientes;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Features.Tests
{
  [Collection(nameof(ClienteBogusCollection))]
  public class ClienteServiceTests
  {
    readonly ClienteTestsBogusFixture _clienteTestsBogusFixture;

    public ClienteServiceTests(ClienteTestsBogusFixture clienteTestsBogusFixture)
    {
      _clienteTestsBogusFixture = clienteTestsBogusFixture;
    }

    [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
    [Trait("Mokando", "Cliente Service Mock Tests")]
    public void ClienteService_Adicionar_DeveExecutarComSucesso()
    {
      // Arrange
      var cliente = _clienteTestsBogusFixture.GerarClienteValido();
      /* Abaixo tem o problema com a injeção de dependência que terá que ser feita.
       * O objeto, ou os objetos, da classe que precisa ser passado, pode ter outras dependências. Se fosse um repositorio, e ele conecta-se a um banco de dados? Problema.
       */
      //var clienteService = new ClienteService();
      
      // Usando o MOQ...

      // Aqui os obejtos criados tendo como base os "contratos" (interfaces), são do tipo Mock...
      var clienteRepo = new Mock<IClienteRepository>();
      var mediator = new Mock<IMediator>();
      // ... para ter acesso ao objeto que está sendo mockado, só acessar a propriedade "Object".
      var clienteService = new ClienteService(clienteRepo.Object, mediator.Object);

      // Act
      clienteService.Adicionar(cliente);

      // Assert
      //Assert.True(cliente.EhValido()); // não necessitaria, pq o método "Adicionar" já o chama. O método de teste só deve validar somente aquela determinada questão (com um ou mais Asserts CUIDADO!!!)
      /* Lendo:
       * Como não é necessário fazer a asserção pelo "EhValido", temos que fazer de outra forma. Que é o seguinte:
       * No método "Adicionar" (classe ClienteService), se o objeto "cliente" for válido, vai chamar os métodos:
       *   IClienteRepository.Adicionar(ClienteService);
       *   IMediator.Publish(classe que herda de INotification, [CancellationToken]);
       * Então fazemos o seguinte:
       * O MOQ tem internamente uma "Assert" nos objetos mockados, então é só fazer o seguinte:
       * 
       * Para o objeto mockado "clienteRepo", verificar se o método "Adicionar" foi chamado pelo menos uma vez;
       * Para o objeto mockado "mediator", verificar se o método "Publish" foi chamado pelo menos uma vez.
       * 
       * Cuidados:
       *  1 - Com os parâmetros que são passados para os métodos.
       *    No caso do "Adicionar" foi passado o menos objeto "cliente" que foi passado no ClienteService.Adicionar(...);
       *    No caso do Publish, é mais complexo:
       *      1 - Ele passa um objeto do tipo ClienteEmailNotification, porém essa classe herda da INotification. Por isso é utilizado o "IsAny<tipo/interface>"
       *      2 - Ele passa outro parâmetro, no caso default (por isso não tem lá na ClienteService), mas nesse caso deve ser passado (aqui é analisar caso a caso).
       * 
       * Detalhe: os métodos "Adicionar" e "Publish" retornam void.
       */
      clienteRepo.Verify(r => r.Adicionar(cliente), Times.Once);
      mediator.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
    }

    [Fact(DisplayName = "Adicionar Cliente com Falha")]
    [Trait("Mokando", "Cliente Service Mock Tests")]
    public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
    {
      // Arrange
      var cliente = _clienteTestsBogusFixture.GerarClienteInvalido();
      var clienteRepo = new Mock<IClienteRepository>();
      var mediator = new Mock<IMediator>();
      
      var clienteService = new ClienteService(clienteRepo.Object, mediator.Object);

      // Act
      clienteService.Adicionar(cliente);

      // Assert
      clienteRepo.Verify(r => r.Adicionar(cliente), Times.Never);
      mediator.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
    }

    [Fact(DisplayName = "Obter Clientes Ativos")]
    [Trait("Mokando", "Cliente Service Mock Tests")]
    public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
    {
      // Arrange
      var clienteRepo = new Mock<IClienteRepository>();
      /* lendo: o Setup do Mock, é "ensinar" o método a fazer o que eu quero...
       * 
       * Quando o método "ObterTodos" for chamado, o retorno deve ser "_clienteTestsBogusFixture.ObterClientesVariados()".
       * 
       * Esquema é: Setup(... método).Returns(método que retornar o que vc quer);
       */
      clienteRepo.Setup(c => c.ObterTodos())
        .Returns(_clienteTestsBogusFixture.ObterClientesVariados());

      var mediator = new Mock<IMediator>();
      var clienteService = new ClienteService(clienteRepo.Object, mediator.Object);

      // Act
      /* Lendo:
       * Ao invocar o método "ObterTodosAtivos", os objetos utilizam instâncias do Mock, então não tem dados. Será necessario uma configuração inicial, que é a seguinte:
       * No Cliente Fixture, implementar um método que retorne uma lista de clientes, simples assim.
       */
      
      var clientes = clienteService.ObterTodosAtivos(); // que vai internamente chama o método "ObterTodos" do repositório

      // Assert
      clienteRepo.Verify(r => r.ObterTodos(), Times.Once); // Não precisaria
      // o teste de fato é valdiar o resultado (clientes)
      Assert.True(clientes.Any());
      Assert.False(clientes.Count(c => !c.Ativo) > 0);
    }
  }
}
