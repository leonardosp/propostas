using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViveBem.Domain.Core.Notifications
{
    public interface IDomainNotificationHandler<T> : IHandler<T> where T : Message
    {
        bool HasNotifications();
        List<T> GetNotifications();
    }
}
