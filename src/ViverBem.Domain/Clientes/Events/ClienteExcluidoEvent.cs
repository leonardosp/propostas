using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Clientes.Events
{
    public class ClienteExcluidoEvent : BaseClienteEvent
    {
        public ClienteExcluidoEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
