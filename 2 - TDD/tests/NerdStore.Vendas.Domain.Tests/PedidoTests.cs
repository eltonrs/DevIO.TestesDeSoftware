using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NerdStore.Vendas.Domain;
using System.Linq;
using NerdStore.Core.DomainObjects;
using NerdStore.Vendas.Domain.VoucherAgregacao;

namespace NerdStore.Vendas.Domain.Tests
{
  public class PedidoTests
  {
    [Fact(DisplayName = "Adicionar Item Novo Pedido")]
    [Trait("Categoria", "Vendas - Pedido")]
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
    [Trait("Categoria", "Vendas - Pedido")]
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

    [Fact(DisplayName = "Adicionar Item Pedido Acima do Permitido")] // estava "Acima de 15". Não é bom colocar valores fixos.
    [Trait("Categoria", "Vendas - Pedido")]
    public void AdicionarItemPedido_ItemAcimaPermitido_DeveRetornarException()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var produtoId = Guid.NewGuid();
      var pedidoItem = new PedidoItem(produtoId, "Produto 1 Teste", PedidoConstantes.MAX_UNIDADES_ITEM + 1, 100);

      // Act & Assert
      Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem));
    }

    [Fact(DisplayName = "Adicionar item existente acima da qtde máxima permitida")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var produtoId = Guid.NewGuid();
      var pedidoItem1 = new PedidoItem(produtoId, "Produto 1 Teste", 1, 100);
      var pedidoItem2 = new PedidoItem(produtoId, "Produto 1 Teste", PedidoConstantes.MAX_UNIDADES_ITEM, 100);
      pedido.AdicionarItem(pedidoItem1);

      // Act & Assert
      Assert.Throws<DomainException>(() => pedido.AdicionarItem(pedidoItem2));
    }

    [Fact(DisplayName = "Atualizar item pedido inexistente")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AtualizarItemPedido_ItemNaoExisteNoPedido_DeveRetornarExcecao()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var produtoId = Guid.NewGuid();
      var pedidoItem1 = new PedidoItem(produtoId, "Produto 1 Teste", 1, 100);

      // Act & Assert
      Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItem1));

    }

    [Fact(DisplayName = "Atualizar quantidade item pedido")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AtualizarItemPedido_ItemExisteNoPedido_DeveSomarQuantidade()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var produtoId = Guid.NewGuid();
      var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
      pedido.AdicionarItem(pedidoItem);
      var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 100);
      var novaQuantidade = pedidoItemAtualizado.Quantidade;

      // Act
      pedido.AtualizarItem(pedidoItemAtualizado);

      // Assert
      Assert.Equal(novaQuantidade, pedido.PedidoItens.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
    }

    [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var produtoId1 = Guid.NewGuid();
      var pedidoItem1 = new PedidoItem(produtoId1, "Produto 1 Teste", 2, 100); // Total item: 200
      pedido.AdicionarItem(pedidoItem1);

      var produtoId2 = Guid.NewGuid();
      var pedidoItem2 = new PedidoItem(produtoId2, "Produto 2 Teste", 1, 200); // Total item: 200
      pedido.AdicionarItem(pedidoItem2);

      // Total pedido: 400

      var pedidoItem2Atualizar = new PedidoItem(produtoId2, "Produto 2 Teste", 3, 200); // Total item: 600 (200 + 800)

      // Total pedido: 800

      var valorTotalPedido = (pedidoItem1.Quantidade * pedidoItem1.ValorUnitario) +
        (pedidoItem2Atualizar.Quantidade * pedidoItem2Atualizar.ValorUnitario);

      // Act
      pedido.AtualizarItem(pedidoItem2Atualizar);

      // Total pedido: 800

      // Assert
      Assert.Equal(valorTotalPedido, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Atualizar Item Pedido Quantidade acima do permitido")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void AtualizarItemPedido_ItemUnidadesAcimaDoPermitido_DeveRetornarException()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var produtoId = Guid.NewGuid();
      var pedidoItem = new PedidoItem(produtoId, "Produto 1 Teste", 1, 100);
      pedido.AdicionarItem(pedidoItem);

      var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto 1 Teste", PedidoConstantes.MAX_UNIDADES_ITEM + 1, 100);

      // Act & Assert
      Assert.Throws<DomainException>(() => pedido.AtualizarItem(pedidoItemAtualizado));
    }

    [Fact(DisplayName = "Remover Item Pedido Inexistente")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

      // Act & Assert
      Assert.Throws<DomainException>(() => pedido.RemoverItem(pedidoItemRemover));
    }


    [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
    [Trait("Categoria", "Vendas - Pedido")]
    public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var produtoId = Guid.NewGuid();
      var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
      var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
      pedido.AdicionarItem(pedidoItem1);
      pedido.AdicionarItem(pedidoItem2);

      var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

      // Act
      pedido.RemoverItem(pedidoItem1);

      // Assert
      Assert.Equal(totalPedido, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Aplicar voucher válido")]
    [Trait("Categoria", "Vendas - Pedido - Voucher")]
    public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

      // Act
      var result = pedido.AplicarVoucher(voucher);

      // Assert
      Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Aplicar voucher inválido")]
    [Trait("Categoria", "Vendas - Pedido - Voucher")]
    public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var voucher = new Voucher("PROMO-15-REAIS", null, null, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), true, false);

      // Act
      var result = pedido.AplicarVoucher(voucher);

      // Assert
      Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Aplicar voucher tipo valor desconto")]
    [Trait("Categoria", "Vendas - Pedido - Voucher")]
    public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

      var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
      var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
      pedido.AdicionarItem(pedidoItem1);
      pedido.AdicionarItem(pedidoItem2);

      var voucher = new Voucher("PROMO-5-REAIS", null, 5, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

      var valorTotalComDesconto = pedido.ValorTotal - voucher.ValorDesconto;

      // Act
      pedido.AplicarVoucher(voucher);

      // Assert
      Assert.Equal(valorTotalComDesconto, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Aplicar voucher tipo percentual desconto")]
    [Trait("Categoria", "Vendas - Pedido - Voucher")]
    public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

      var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
      var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 3, 15);
      pedido.AdicionarItem(pedidoItem1);
      pedido.AdicionarItem(pedidoItem2);

      var voucher = new Voucher("PROMO-15-OFF", 15, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(10), true, false);

      var valorDesconto = (pedido.ValorTotal * voucher.PercentualDesconto) / 100;
      var valorTotalComDesconto = pedido.ValorTotal - valorDesconto;

      // Act
      pedido.AplicarVoucher(voucher);

      // Assert
      Assert.Equal(valorTotalComDesconto, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Aplicar voucher desconto excede valor total")]
    [Trait("Categoria", "Vendas - Pedido - Voucher")]
    public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());

      var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
      pedido.AdicionarItem(pedidoItem1);

      var voucher = new Voucher("PROMO-15-OFF", null, 300, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);

      // Act
      pedido.AplicarVoucher(voucher);

      // Assert
      Assert.Equal(0, pedido.ValorTotal);
    }

    [Fact(DisplayName = "Aplicar voucher recalcular desconto na modificação do pedido")]
    [Trait("Categoria", "Vendas - Pedido - Voucher")]
    public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
    {
      // Arrange
      var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
      var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
      pedido.AdicionarItem(pedidoItem1);

      var voucher = new Voucher("PROMO-15-OFF", null, 50, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(10), true, false);
      pedido.AplicarVoucher(voucher);

      var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 4, 25);

      // Act
      pedido.AdicionarItem(pedidoItem2);

      // Assert
      var totalEsperado = pedido.PedidoItens.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
      Assert.Equal(totalEsperado, pedido.ValorTotal);
    }
  }
}