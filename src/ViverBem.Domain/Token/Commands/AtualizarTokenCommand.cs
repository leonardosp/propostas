using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Token.Commands
{
    public class AtualizarTokenCommand : BaseTokenCommand
    {
        public AtualizarTokenCommand(Guid id, string accestoken,string error,string errordescription,string retornototal,DateTime limitedotoken)
        {
            Id = id;
            AccessToken = accestoken;
            Error = error;
            ErrorDescription = errordescription;
            RETORNOTOTAL = retornototal;
            LimiteDoToken = limitedotoken;
        }
    }
}
