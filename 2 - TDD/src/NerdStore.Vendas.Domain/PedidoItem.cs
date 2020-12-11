using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Domain
{
  public class PedidoItem
  {
    public Guid ProdutoId { get; private set; }
    public string Descricao { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }

    public PedidoItem(Guid produtoId, string descricao, int quantidade, decimal valorUnitario)
    {
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
