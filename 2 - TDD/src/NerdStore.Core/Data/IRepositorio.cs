using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Core.Data
{
  /* Lendo:
   * O ideal é um repositorio por agregação (falando em DDD).
   * Então é implementado uma interface para representar a classe root da agregaçao e então
   * o repositório deverá ser dessa agregação.
   */
  public interface IRepositorio<T> : IDisposable where T : IAggregateRoot
  {
    IUnitOfWork UnitOfWork { get; }
  }
}
