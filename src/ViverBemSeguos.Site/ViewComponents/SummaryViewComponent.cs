using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViveBem.Domain.Core.Notifications;

namespace ViverBemSeguos.Site.ViewComponents
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly IDomainNotificationHandler<DomainNotification> _notifications;

        public SummaryViewComponent(IDomainNotificationHandler<DomainNotification> notifications)
        {
            _notifications = notifications;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notifications.GetNotifications());
            notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Value));

            return View();
        }
    }
}
