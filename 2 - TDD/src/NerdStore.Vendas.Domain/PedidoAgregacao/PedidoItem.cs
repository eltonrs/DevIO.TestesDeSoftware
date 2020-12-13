using System;
using System.Collections.Generic;
using System.Text;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain.PedidoAgregacao
{
  public class PedidoItem
  {
    public Guid ProdutoId { get; private set; }
    public string Descricao { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }

    public PedidoItem(Guid produtoId, string descricao, int quantidade, decimal valorUnitario)
    {
      /* Lendo:
       * 1 - Como o mínimo é 1, ou seja, 0 (nada) não é aceitável... já nem deixa criar. Bloqueia no ctor.
       * 2 - Como a classe "Pedido" é a raiz da agregação (que contém as constantes em PedidoConstantes) e 
       *     a classe PedidoItem é filha (dependente) dela, então a validação usando mecanismos da classe
       *     Pedido, podem ser utilizados.
       */
      if (quantidade < PedidoConstantes.MIN_UNIDADES_ITEM)
        throw new DomainException($"A qtde de itens por produto não pode ser menor que {PedidoConstantes.MIN_UNIDADES_ITEM}.");

      ProdutoId = produtoId;
      Descricao = descricao;
      Quantidade = quantidade;
      ValorUnitario = valorUnitario;
    }

    internal void AdicionarUnidades(int unidades)
    {
      Quantidade += unidades;
    }

    internal decimal CalcularValor()
    {
      return Quantidade * ValorUnitario;
    }
  }
}
