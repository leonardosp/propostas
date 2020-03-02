using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViveBem.Domain.Core.Commands
{
    public class Command : Message
    {
        public DateTime Timestamp { get; private set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
