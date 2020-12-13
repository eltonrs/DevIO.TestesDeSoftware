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
    private readonly AutoMocker _mocker;
    private readonly PedidoCommandHandler _pedidoCommandHandler;
    private readonly Guid _clienteId;
    private readonly Guid _produtoId;
    private readonly Pedido _pedido;
    public PedidoCommandHandlerTests()
    {
      _mocker = new AutoMocker();
      _pedidoCommandHandler = _mocker.CreateInstance<PedidoCommandHandler>();
      _clienteId = Guid.NewGuid();
      _produtoId = Guid.NewGuid();
      _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(_clienteId);
    }

    [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
    [Trait("Categoria", "Vendas - Pedido - Command Handler")]
    public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso() // o método foi transformado em "async" pq o resultado (Act) é async tbm
    {
      // Arrange
      var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

      _mocker.GetMock<IPedidoRepositorio>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

      // Act
      var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

      // Assert
      Assert.True(result);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Adicionar Novo Item Pedido Rascunho com Sucesso")]
    [Trait("Categoria", "Vendas - Pedido - Command Handler")]
    public async Task AdicionarItem_NovoItemAoPedidoRascunho_DeveExecutarComSucesso()
    {
      // Arrange

      var pedidoItemExistente = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
      _pedido.AdicionarItem(pedidoItemExistente);

      var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, Guid.NewGuid(), "Produto Teste", 2, 100);

      _mocker.GetMock<IPedidoRepositorio>().Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
      _mocker.GetMock<IPedidoRepositorio>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

      // Act
      var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

      // Assert
      Assert.True(result);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.AdicionarItem(It.IsAny<PedidoItem>()), Times.Once);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Adicionar Item Existente ao Pedido Rascunho com Sucesso")]
    [Trait("Categoria", "Vendas - Pedido - Command Handler")]
    public async Task AdicionarItem_ItemExistenteAoPedidoRascunho_DeveExecutarComSucesso()
    {
      // Arrange

      var pedidoItemExistente = new PedidoItem(_produtoId, "Produto Xpto", 2, 100);
      _pedido.AdicionarItem(pedidoItemExistente);

      var pedidoCommand = new AdicionarItemPedidoCommand(_clienteId, _produtoId, "Produto Xpto", 2, 100);

      _mocker.GetMock<IPedidoRepositorio>().Setup(r => r.ObterPedidoRascunhoPorClienteId(_clienteId)).Returns(Task.FromResult(_pedido));
      _mocker.GetMock<IPedidoRepositorio>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

      // Act
      var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

      // Assert
      Assert.True(result);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.AtualizarItem(It.IsAny<PedidoItem>()), Times.Once);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once);
      _mocker.GetMock<IPedidoRepositorio>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    }

    [Fact(DisplayName = "Adicionar Item Command Inválido")]
    [Trait("Categoria", "Vendas - Pedido - Command Handler")]
    public async Task AdicionarItem_CommandInvalido_DeveRetornarFalsoELancarEventosDeNotificacao()
    {
      // Arrange
      var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty, Guid.Empty, "", 0, 0);

      // Act
      var result = await _pedidoCommandHandler.Handle(pedidoCommand, CancellationToken.None);

      // Assert
      Assert.False(result);
      _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
    }
  }
}
