using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Token.Commands
{
    public class ExcluirTokenCommand : BaseTokenCommand
    {
        public ExcluirTokenCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
