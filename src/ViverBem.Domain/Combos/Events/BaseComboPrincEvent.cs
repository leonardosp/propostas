using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Combos.Events
{
    public class BaseComboPrincEvent : Event
    {
        public Guid ComboPrincId { get; set; }
        public String Corretor { get; protected set; }

        public string CodCombo { get; protected set; }
    }
}
