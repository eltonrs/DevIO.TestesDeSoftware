using NerdStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain.PedidoAgregacao
{
  public interface IPedidoRepositorio : IRepositorio<Pedido>
  {
    void Adicionar(Pedido pedido);
    void Atualizar(Pedido pedido);
    void AdicionarItem(PedidoItem pedidoItem);
    void AtualizarItem(PedidoItem pedidoItem);
    Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid id);
  }
}
