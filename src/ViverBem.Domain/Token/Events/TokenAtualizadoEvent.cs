using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Token.Events
{
    public class TokenAtualizadoEvent : BaseTokenEvent
    {
        public TokenAtualizadoEvent(Guid id,string accestoken,string error,string errordescription,DateTime limitetoken)
        {
            Id = id;
            AccessToken = accestoken;
            Error = error;
            ErrorDescription = errordescription;
            LimiteDoToken = limitetoken;
        }
    }
}
