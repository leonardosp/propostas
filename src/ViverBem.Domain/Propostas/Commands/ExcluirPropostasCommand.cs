using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Propostas.Commands
{
    public class ExcluirPropostasCommand : BasePropostasCommand
    {
        public ExcluirPropostasCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
