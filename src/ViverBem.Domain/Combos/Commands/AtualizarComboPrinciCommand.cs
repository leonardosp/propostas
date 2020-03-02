using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Combos.Commands
{
    public class AtualizarComboPrinciCommand : BaseComboPrincCommand
    {
        public AtualizarComboPrinciCommand(string codcombo, string corretor)
        {
            CodCombo = codcombo;
            Corretor = corretor;
        }
    }
}
