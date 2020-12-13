using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.Messages
{
  public abstract class Message
  {
    public string MessageType { get; protected set; } // 
    public Guid AggregateId { get; protected set; } // o id da raiz de agregação, que é sempre a entidade que persistirá os dados

    public Message()
    {
      MessageType = GetType().Name;
    }
  }
}
