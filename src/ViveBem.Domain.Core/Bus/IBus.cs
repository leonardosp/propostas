using ViveBem.Domain.Core.Commands;
using ViveBem.Domain.Core.Events;

namespace ViveBem.Domain.Core.Bus
{
    public interface IBus
    {
        void SendCommand<T>(T theCommand) where T : Command;
        void RaiseEvent<T>(T theEvent) where T : Event;
    }
}
