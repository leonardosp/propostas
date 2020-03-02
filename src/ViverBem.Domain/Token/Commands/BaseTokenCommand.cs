using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Token.Commands
{
    public abstract class BaseTokenCommand : Command
    {
        public Guid Id { get; protected set; }

        public string AccessToken { get; protected set; }

        public string Error { get; protected set; }

        public string ErrorDescription { get; protected set; }
        public string RETORNOTOTAL { get; protected set; }

        public DateTime LimiteDoToken { get; protected set; }
    }
}
