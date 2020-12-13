using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.DomainObjects
{
  public abstract class Entidade
  {
    public Guid Id { get; set; }

    protected Entidade()
    {
      Id = Guid.NewGuid();
    }
  }
}
