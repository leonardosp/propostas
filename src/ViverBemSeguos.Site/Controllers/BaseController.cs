using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.Interfaces;

namespace ViverBemSeguos.Site.Controllers
{
    public class BaseController : Controller
    {
        private readonly IDomainNotificationHandler<DomainNotification> _notifications;
        private readonly IUser _user;

        public Guid FuncionarioId { get; set; }

        public BaseController(IDomainNotificationHandler<DomainNotification> notifications, IUser user)
        {
            _notifications = notifications;
            _user = user;

            if (_user.IsAuthenticated())
            {
                FuncionarioId = _user.GetUserId();
            }
        }

        protected bool OperacaoValida()
        {
            return (!_notifications.HasNotifications());
        }
    }
}