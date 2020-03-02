using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Token.Commands
{
    public class RegistrarTokenCommand : BaseTokenCommand
    {
        public RegistrarTokenCommand(string accesstoken, string error, string errordescription,string retornototal, DateTime limitedotoken)
        {
            AccessToken = accesstoken;
            Error = error;
            ErrorDescription = errordescription;
            RETORNOTOTAL = retornototal;
            LimiteDoToken = limitedotoken;
        }
    }
}
