using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using NerdStore.Core.DomainObjects;

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

  public static class PedidoConstantes
  {
    public static int MAX_UNIDADES_ITEM => 15; // "=> 15" = "{ get { return 15; } }" é uma expressão lambda
    public static int MIN_UNIDADES_ITEM
    {
      get { return 1; }
    }

    //public const int MAX_UNIDADES_ITEM = 15; // poderia ser assim também.
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

    private bool PedidoItemExistente(PedidoItem pedidoItem)
    {
      return _pedidoItems.Any(pi => pi.ProdutoId == pedidoItem.ProdutoId);
    }

    private void ValidarQuantidadeMaximaUnidadeItemPedido(PedidoItem pedidoItem)
    {
      int quantidadeTotal = pedidoItem.Quantidade;
      
      if (PedidoItemExistente(pedidoItem))
      {
        var pedidoItemExistente = _pedidoItems.FirstOrDefault(pi => pi.ProdutoId == pedidoItem.ProdutoId);
        quantidadeTotal += pedidoItemExistente.Quantidade;
      }

      if (quantidadeTotal > PedidoConstantes.MAX_UNIDADES_ITEM)
        throw new DomainException($"Excedeu o número itens por produto no pedido. O máximo são {PedidoConstantes.MAX_UNIDADES_ITEM} itens.");
    }

    public void AdicionarItem(PedidoItem pedidoItem)
    {
      ValidarQuantidadeMaximaUnidadeItemPedido(pedidoItem);
      
      if (!PedidoItemExistente(pedidoItem))
        _pedidoItems.Add(pedidoItem);
      else
        SomarUnidadesItemPedido(pedidoItem);

      CalcularValorPedido();
    }

    public void AtualizarItem(PedidoItem pedidoItem)
    {
      //ValidarPedidoItemInexistente(pedidoItem);

      RemoverItem(pedidoItem);

      AdicionarItem(pedidoItem);
    }

    public void RemoverItem(PedidoItem pedidoItem)
    {
      ValidarPedidoItemInexistente(pedidoItem);

      var pedidoItemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
      _pedidoItems.Remove(pedidoItemExistente);

      CalcularValorPedido();
    }

    private void SomarUnidadesItemPedido(PedidoItem pedidoItem)
    {
      _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId).AdicionarUnidades(pedidoItem.Quantidade);
    }

    private void ValidarPedidoItemInexistente(PedidoItem pedidoItem)
    {
      if (!PedidoItemExistente(pedidoItem))
        throw new DomainException($"Item inexistente no pedido.");
    }

    private void CalcularValorPedido()
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
