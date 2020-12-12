using System;
using Xunit;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain.Tests
{
  public class PedidoItemTests
  {
    [Fact(DisplayName = "Novo Item Pedido com unidades abaixo do permitido")] // estava "Acima de 15". Não é bom colocar valores fixos.
    [Trait("Categoria", "Vendas - Pedido - Pedido Item")]
    public void AdicionarItemPedido_UnidadesItemAbaixoPermitido_DeveRetornarException()
    {
      /* Lendo:
       * Como a excessão está sendo disparada dentor do ctor do PedidoItem, o Arrange, Act e Assert estão todos juntos
       */
      // Arrange & Act & Assert
      Assert.Throws<DomainException>(() => new PedidoItem(Guid.NewGuid(), "Produto 1 Teste", PedidoConstantes.MIN_UNIDADES_ITEM - 1, 100));
    }
  }
}
