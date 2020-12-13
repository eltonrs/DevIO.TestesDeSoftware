using NerdStore.Vendas.Domain.VoucherAgregacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
  public class VoucherTests
  {
    [Fact(DisplayName = "Validar Voucher Tipo Valor Válido")]
    [Trait("Categoria", "Vendas - Voucher")]
    public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
    {
      // Arrange
      var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

      // Act
      var result = voucher.ValidarSeAplicavel();

      // Assert
      
      /* Lendo:
       * O "IsValid" é um método do FluentValidator, ou seja, se aquelas validações implementadas na classe
       * VoucherAplicavelValidation são válidas.
       */
      Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Validar Voucher Tipo Valor Inválido")]
    [Trait("Categoria", "Vendas - Voucher")]
    public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
    {
      // Arrange
      /* Lendo:
       * São vários os erros que serão gerados aqui devido aos valores dos parâmetros. O ideal seria validar cada erro (**).
       */
      var voucher = new Voucher("", null, null, 0, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

      // Act
      var result = voucher.ValidarSeAplicavel();

      // Assert

      /* Lendo:
       * O "IsValid" é um método do FluentValidator, ou seja, se aquelas validações implementadas na classe
       * VoucherAplicavelValidation são válidas.
       */
      Assert.False(result.IsValid);
      /* Lendo:
       * (**) Pode ser validada a quantidade de erros que foram retornados pelo FluentValidationResult baseados nos paramêtros.
       */
      Assert.Equal(6, result.Errors.Count);
      /* Lendo:
       * Dá para verificar erro por erro, basta verificar se as mensagens de erros (definidas na classe VoucherAplicavelValidation)
       * estão dentro da lista de mensagens de erros.
       *
       * A mensagem no primeiro parâmetro está dentro da lista de erros?
       */
      Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.ValorDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));
    }
    [Fact(DisplayName = "Validar Voucher Tipo Percentual Válido")]
    [Trait("Categoria", "Vendas - Voucher")]
    public void Voucher_ValidarVoucherTipoPercentual_DeveEstarValido()
    {
      // Arrange
      var voucher = new Voucher("PROMO-15-REAIS", 10, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(15), true, false);

      // Act
      var result = voucher.ValidarSeAplicavel();

      // Assert

      /* Lendo:
       * O "IsValid" é um método do FluentValidator, ou seja, se aquelas validações implementadas na classe
       * VoucherAplicavelValidation são válidas.
       */
      Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Validar Voucher Tipo Percentual Inválido")]
    [Trait("Categoria", "Vendas - Voucher")]
    public void Voucher_ValidarVoucherTipoPercentual_DeveEstarInvalido()
    {
      // Arrange
      /* Lendo:
       * São vários os erros que serão gerados aqui devido aos valores dos parâmetros. O ideal seria validar cada erro (**).
       */
      var voucher = new Voucher("", null, null, 0, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(-1), false, true);

      // Act
      var result = voucher.ValidarSeAplicavel();

      // Assert

      /* Lendo:
       * O "IsValid" é um método do FluentValidator, ou seja, se aquelas validações implementadas na classe
       * VoucherAplicavelValidation são válidas.
       */
      Assert.False(result.IsValid);
      /* Lendo:
       * (**) Pode ser validada a quantidade de erros que foram retornados pelo FluentValidationResult baseados nos paramêtros.
       */
      Assert.Equal(6, result.Errors.Count);
      /* Lendo:
       * Dá para verificar erro por erro, basta verificar se as mensagens de erros (definidas na classe VoucherAplicavelValidation)
       * estão dentro da lista de mensagens de erros.
       *
       * A mensagem no primeiro parâmetro está dentro da lista de erros?
       */
      Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
      Assert.Contains(VoucherAplicavelValidation.PercentualDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));
    }
  }
}
