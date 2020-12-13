using FluentValidation.Results;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Commands.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Application.Commands
{
  public class AdicionarItemPedidoCommand : Command
  {
    public Guid ClienteId { get; set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }

    public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
    {
      ClienteId = clienteId;
      ProdutoId = produtoId;
      Nome = nome;
      Quantidade = quantidade;
      ValorUnitario = valorUnitario;
    }

    public override bool EhValido()
    {
      ValidationResult = new AdicionarItemPedidoValidation().Validate(this);
      return ValidationResult.IsValid;
    }
  }
}
