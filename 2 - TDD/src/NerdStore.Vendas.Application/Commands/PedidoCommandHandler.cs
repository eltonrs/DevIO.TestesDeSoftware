using MediatR;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.PedidoAgregacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
  public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
  {
    private readonly IPedidoRepositorio _pedidoRepositorio;
    private readonly IMediator _mediator;

    public PedidoCommandHandler(IPedidoRepositorio pedidoRepositorio, IMediator mediator)
    {
      _pedidoRepositorio = pedidoRepositorio;
      _mediator = mediator;
    }

    public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
    {
      var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
      pedido.AdicionarItem(pedidoItem);

      _pedidoRepositorio.Adicionar(pedido);

      await _mediator.Publish(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade), cancellationToken);

      return true;
    }
  }
}
