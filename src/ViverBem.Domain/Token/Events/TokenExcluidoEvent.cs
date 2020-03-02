using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Token.Events
{
    public class TokenExcluidoEvent : BaseTokenEvent
    {
        public TokenExcluidoEvent(Guid id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
