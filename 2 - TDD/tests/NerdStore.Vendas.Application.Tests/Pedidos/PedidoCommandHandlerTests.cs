using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain.PedidoAgregacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
  public class PedidoCommandHandlerTests
  {
    [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
    [Trait("Categoria", "Vendas - Pedido - Command Handler")]
    public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso() // o método foi transformado em "async" pq o resultado (Act) é async tbm
    {
      // Arrange
      var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

      var mocker = new AutoMocker();
      var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

      mocker.GetMock<IPedidoRepositorio>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

      // Act
      var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

      // Assert
      Assert.True(result);
      mocker.GetMock<IPedidoRepositorio>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
      //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
      mocker.GetMock<IPedidoRepositorio>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }
  }
}
