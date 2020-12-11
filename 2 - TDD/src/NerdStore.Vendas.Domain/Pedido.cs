using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace NerdStore.Vendas.Domain
{
  public enum PedidoStatus
  {
    Rascunho = 0,
    Iniciado = 1,
    Pago = 4,
    Entregue = 5,
    Cancelado = 6
  }

  public class Pedido
  {
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; private set; }
    public PedidoStatus PedidoStatus { get; set; }
    private readonly List<PedidoItem> _pedidoItems;
    public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItems;

    protected Pedido()
    {
      _pedidoItems = new List<PedidoItem>();
    }

    public void AdicionarItem(PedidoItem pedidoItem)
    {
      if (_pedidoItems.Any(pi => pi.ProdutoId == pedidoItem.ProdutoId))
      {
        var pedidoItemExistente = _pedidoItems.FirstOrDefault(pi => pi.ProdutoId == pedidoItem.ProdutoId);
        pedidoItemExistente.AdicionarUnidades(pedidoItem.Quantidade);
        
        pedidoItem = pedidoItemExistente;

        _pedidoItems.Remove(pedidoItemExistente);
      }

      _pedidoItems.Add(pedidoItem);
      CalcularValorPedido();
    }

    public void CalcularValorPedido()
    {
      ValorTotal = _pedidoItems.Sum(i => i.CalcularValor());
    }

    public void TornarRascunho()
    {
      PedidoStatus = PedidoStatus.Rascunho;
    }

    public static class PedidoFactory // Classe aninhada (dentro de outra classe)
    {
      public static Pedido NovoPedidoRascunho(Guid clienteId)
      {
        var pedido = new Pedido // por ser aninhada, consigo acessar o ctor que está protected
        {
          ClienteId = clienteId
        };

        pedido.TornarRascunho();
        return pedido;
      }
    }
  }
}
