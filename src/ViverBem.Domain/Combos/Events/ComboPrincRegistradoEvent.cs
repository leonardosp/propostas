using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Combos.Events
{
    public class ComboPrincRegistradoEvent : Event
    {
        public String Corretor { get; private set; }

        public string CodCombo { get; private set; }
        public Guid ComboPrincId { get; private set; }
        public ComboPrincRegistradoEvent(string codcombo, string corretor,Guid comboprincid)
        {
            ComboPrincId = comboprincid;
            CodCombo = codcombo;
            Corretor = corretor;
        }
    }
}
