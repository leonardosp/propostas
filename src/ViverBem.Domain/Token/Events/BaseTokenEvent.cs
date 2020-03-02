using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Token.Events
{
    public abstract class BaseTokenEvent : Event
    {
        public Guid Id { get; protected set; }

        public string AccessToken { get; protected set; }

        public string Error { get; protected set; }

        public string ErrorDescription { get; protected set; }

        public DateTime LimiteDoToken { get; protected set; }
    }
}
