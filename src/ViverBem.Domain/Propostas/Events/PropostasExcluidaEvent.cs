using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Propostas.Events
{
    public class PropostasExcluidaEvent : BasePropostasEvent
    {
        public PropostasExcluidaEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
