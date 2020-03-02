using System;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Commands;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;

namespace ViverBem.Infra.CrossCutting.Bus
{
    public sealed class InMemoryBus : IBus
    {
        public static Func<IServiceProvider> ContainerAcessor { get; set; }
        private static IServiceProvider Container => ContainerAcessor();
        public void RaiseEvent<T>(T theEvent) where T : Event
        {
            Publish(theEvent);
        }

        public void SendCommand<T>(T theCommand) where T : Command
        {
            Publish(theCommand);
        }

        private static void Publish<T>(T message) where T : Message
        {
            if (Container == null) return;

            var obj = Container.GetService(message.MessageType.Equals("DomainNotification") ? typeof(IDomainNotificationHandler<T>) : typeof(IHandler<T>));

            ((IHandler<T>)obj).Handle(message);
        }
    }
}
