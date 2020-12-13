using FluentValidation;
using NerdStore.Vendas.Domain.PedidoAgregacao;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Application.Commands.Validations
{
  public class AdicionarItemPedidoValidation : AbstractValidator<AdicionarItemPedidoCommand>
  {
    public static string IdClienteErroMsg => "Id do cliente inválido";
    public static string IdProdutoErroMsg => "Id do produto inválido";
    public static string NomeErroMsg => "O nome do produto não foi informado";
    public static string QtdMaxErroMsg => $"A quantidade máxima de um item é {PedidoConstantes.MAX_UNIDADES_ITEM}";
    public static string QtdMinErroMsg => "A quantidade miníma de um item é 1";
    public static string ValorErroMsg => "O valor do item precisa ser maior que 0";

    public AdicionarItemPedidoValidation()
    {
      RuleFor(c => c.ClienteId)
        .NotEqual(Guid.Empty)
        .WithMessage(IdClienteErroMsg);

      RuleFor(c => c.ProdutoId)
        .NotEqual(Guid.Empty)
        .WithMessage(IdProdutoErroMsg);

      RuleFor(c => c.Nome)
        .NotEmpty()
        .WithMessage(NomeErroMsg);

      RuleFor(c => c.Quantidade)
        .GreaterThan(0)
        .WithMessage(QtdMinErroMsg)
        .LessThanOrEqualTo(PedidoConstantes.MAX_UNIDADES_ITEM)
        .WithMessage(QtdMaxErroMsg);

      RuleFor(c => c.ValorUnitario)
        .GreaterThan(0)
        .WithMessage(ValorErroMsg);
    }
  }
}
