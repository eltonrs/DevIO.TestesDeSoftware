using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.Messages
{
  public abstract class Command : Message, IRequest<bool>
  {
    /* O Command (no CQRS) é responsável por carregar os dados que vão dar a possibilidade da criação da instância de uma classe.
     * Ele se auto valida (os dados)
     */

    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
      Timestamp = DateTime.Now;
    }

    public abstract bool EhValido(); // "todo mundo que é um command, vai ter que se auto validar".
  }
}
