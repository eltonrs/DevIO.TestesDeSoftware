using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Domain.PedidoAgregacao
{
  public interface IPedidoRepositorio
  {
    void Adicionar(Pedido pedido);
  }
}
