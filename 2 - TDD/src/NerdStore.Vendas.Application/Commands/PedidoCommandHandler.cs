using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain.PedidoAgregacao;
using System;
using System.Collections.Generic;
using System.Linq;
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
      if (!ValidarComando(message))
        return false;

      var pedido = await _pedidoRepositorio.ObterPedidoRascunhoPorClienteId(message.ClienteId);
      var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);
      
      if (pedido == null)
      {
        pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
        pedido.AdicionarItem(pedidoItem);

        _pedidoRepositorio.Adicionar(pedido);
      }
      else
      {
        var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
        pedido.AdicionarItem(pedidoItem);

        if (pedidoItemExistente)
          _pedidoRepositorio.AtualizarItem(pedido.PedidoItens.FirstOrDefault(pi => pi.ProdutoId == pedidoItem.ProdutoId));
        else
          _pedidoRepositorio.AdicionarItem(pedidoItem);
      }
      
      _pedidoRepositorio.Atualizar(pedido);

      //await _mediator.Publish(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade), cancellationToken);
      pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade));

      return await _pedidoRepositorio.UnitOfWork.Commit();
    }

    private bool ValidarComando(Command message)
    {
      if (message.EhValido())
        return true;
      
      foreach (var error in message.ValidationResult.Errors)
      {
        _mediator.Publish(new DomainNotification(message.MessageType, error.ErrorMessage));
      }

      return false;
      
    }
  }
}
