using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NerdStore.Vendas.Domain;
using System.Linq;

namespace NerdStore.Vendas.Domain.Tests
{
  public class PedidoTests
  {
    [Fact(DisplayName = "Adicionar Item Novo Pedido")]
    [Trait("Categoria", "Pedido Tests")]
    public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
    {
      /* Versão 1 do fonte Vermelha no TDD (etapa 1 do . Lendo:
       * 1.1 - A classe Pedido e PedidoItem ainda não tinham sido criadas.
       * 1.2 - Os parâmetros do constructor da classe PedidoItem foram baseados no descrito no roteiro.
       * 1.3 - Na implementação do método AdicionarItem, foi implementado um resultado fixo, só para passar nos testes.
       * 
       * 2 - Etapa Verde do TDD
       * 2.1 - Testar. Vai funcionar pq foi colocado um valor fixo.
       * 
       * 3 - Etapa Roxa do TDD
       * 3.1 - Refatorar
       */

      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto 1 Teste", 2, 100); // campos baseados no roteiro: Id, nome do produto, qtde e valor
      // Act
      pedido.AdicionarItem(pedidoItem);

      // Assert
      Assert.Equal(200, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Adicionar Item Pedido Existente")]
    [Trait("Categoria", "Pedido Tests")]
    public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadesSomarValores()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      
      var produtoId = Guid.NewGuid(); // para que seja o mesmo produto
      var pedidoItem = new PedidoItem(produtoId, "Produto 1 Teste", 2, 100);
      pedido.AdicionarItem(pedidoItem);
      
      var pedidoItem2 = new PedidoItem(produtoId, "Produto 1 Teste", 1, 100);

      // Act
      pedido.AdicionarItem(pedidoItem2);

      // Assert
      Assert.Equal(300, pedido.ValorTotal);
      Assert.Equal(1, pedido.PedidoItens.Count);
      Assert.Equal(3, pedido.PedidoItens.FirstOrDefault(pi => pi.ProdutoId == produtoId).Quantidade);
    }
  }
}
