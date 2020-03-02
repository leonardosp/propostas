using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Combos.Events
{
    public class ComboPrincExcluidoEvent : BaseComboPrincEvent
    {
        public ComboPrincExcluidoEvent(Guid id)
        {
            ComboPrincId = id;
            AggregateId = id;
        }
    }
}
