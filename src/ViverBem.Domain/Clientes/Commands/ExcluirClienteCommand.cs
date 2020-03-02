using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Clientes.Commands
{
    public class ExcluirClienteCommand : BaseClienteCommand
    {
        public ExcluirClienteCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
