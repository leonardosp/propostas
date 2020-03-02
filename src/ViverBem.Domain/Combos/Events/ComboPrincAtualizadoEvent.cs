using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Combos.Events
{
    public class ComboPrincAtualizadoEvent : BaseComboPrincEvent
    {
        public ComboPrincAtualizadoEvent(string codcombo,string corretor, Guid comboPrincId)
        {
            Corretor = corretor;
            CodCombo = codcombo;
            ComboPrincId = comboPrincId;
        }
    }
}
