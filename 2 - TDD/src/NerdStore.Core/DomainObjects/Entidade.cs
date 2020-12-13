using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.DomainObjects
{
  public abstract class Entidade
  {
    public Guid Id { get; set; }
    private List<Event> _notifications;
    public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

    protected Entidade()
    {
      Id = Guid.NewGuid();
    }

    public void AdicionarEvento(Event evento)
    {
      _notifications = _notifications ?? new List<Event>(); // "??" sintaxe sugar (https://www.eduardopires.net.br/2012/08/c-sharp-iniciantes-syntactic-sugar/)
      _notifications.Add(evento);
    }

    public void RemoverEvento(Event evento)
    {
      _notifications?.Remove(evento);
    }

    public void LimparEventos()
    {
      _notifications?.Clear();
    }
  }
}
