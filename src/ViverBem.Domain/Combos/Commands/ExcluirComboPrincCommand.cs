using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Combos.Commands
{
    public class ExcluirComboPrincCommand : BaseComboPrincCommand
    {
        public ExcluirComboPrincCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
