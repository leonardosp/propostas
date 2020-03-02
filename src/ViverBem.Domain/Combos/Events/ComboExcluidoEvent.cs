using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Combos.Events
{
    public class ComboExcluidoEvent : BaseComboEvent
    {
        public ComboExcluidoEvent(Guid id)
        {
            ComboId = id;
            AggregateId = id;
        }
    }
}
