using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Token.Events
{
    public class TokenRegistradoEvent : BaseTokenEvent
    {
        public TokenRegistradoEvent(Guid id, string accesToken, string error, string erroDescription, DateTime limitedoToken)
        {
            Id = id;
            AccessToken = accesToken;
            Error = error;
            ErrorDescription = erroDescription;
            LimiteDoToken = limitedoToken;
        }
    }
}
