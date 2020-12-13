using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Domain.VoucherAgregacao
{
  public class Voucher
  {
    public string Codigo { get; private set; }
    public decimal? PercentualDesconto { get; private set; }
    public decimal? ValorDesconto { get; private set; }
    public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
    public int Quantidade { get; private set; }
    public DateTime DataValidade { get; private set; }
    public bool Ativo { get; private set; }
    public bool Utilizado { get; private set; }

    public Voucher(string codigo, decimal? percentualDesconto, decimal? valorDesconto, int quantidade,
      TipoDescontoVoucher tipoDescontoVoucher, DateTime dataValidade, bool ativo, bool utilizado)
    {
      Codigo = codigo;
      PercentualDesconto = percentualDesconto;
      ValorDesconto = valorDesconto;
      Quantidade = quantidade;
      TipoDescontoVoucher = tipoDescontoVoucher;
      DataValidade = dataValidade;
      Ativo = ativo;
      Utilizado = utilizado;
    }

    /* Lendo:
     * 1 - Usando a validação do FluentValidation (classe VoucherAplicavelValidation).
     * 2 - O retorno é um resultado de validação do FluentValidation.
     * 3 - O método "Validade" é do FluentValidation. Ele valida a classe Voucher (no caso, ela mesmo (this)).
     */
    public ValidationResult ValidarSeAplicavel()
    {
      var validacao = new VoucherAplicavelValidation();
      return validacao.Validate(this);
    }
  }
}
