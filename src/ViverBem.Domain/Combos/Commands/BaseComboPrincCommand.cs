using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Combos.Commands
{
    public class BaseComboPrincCommand : Command
    {
        public Guid Id { get; protected set; }
        public string Corretor { get; protected set; }

        public string CodCombo { get; protected set; }
    }
}
