using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Combos.Commands
{
    public class RegistrarComboPrincCommand : Command
    {
        public String Corretor { get; private set; }

        public string CodCombo { get; private set; }
        public Guid ComboPrincId { get; private set; }

        public RegistrarComboPrincCommand(string corretor,string codcombo)
        {
            Corretor = corretor;
            CodCombo = codcombo;
        }

    }
}
