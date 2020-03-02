using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Clientes.Commands;

namespace ViverBem.Domain.Combos.Commands
{
    public class ExcluirComboCommand : BaseComboCommand
    {
        public ExcluirComboCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
