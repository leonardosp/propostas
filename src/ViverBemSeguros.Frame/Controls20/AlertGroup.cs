using Celebre.Web.Controls;
using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class AlertGroup : StateManagedCompositeControl
    {
        public AlertGroup()
        {
            Messages = new AlertMessages();
        }
        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AlertMessages Messages { get; set; }

        public void SetMessage(string message)
        {
            Messages.Add(new AlertMessage()
            {
                Message = message
            });
        }
        public void SetMessage(string message, string strongMessage, bool closable, AlertMessageType type)
        {
            Messages.Add(new AlertMessage()
            {
                Message = message,
                StrongMessage = strongMessage,
                Closable = closable,
                AlertType = type
            });

        }
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div class='box-content alerts'>");

            foreach (var message in Messages)
            {
                writer.Write("<div class='alert {0}'>", message.AlertType.GetStringValueClass());

                if (message.Closable)
                    writer.Write("<button type='button' class='close' data-dismiss='alert'>×</button>");

                writer.Write("<strong>{0}</strong>{1}</div>", message.StrongMessage, message.Message);
            }

            writer.Write("</div>");
        }
    }

    [Serializable]
    public class AlertMessages : List<AlertMessage>
    {

    }
    [Serializable]
    public class AlertMessage : StateManagedCompositeControl
    {
        public AlertMessageType AlertType { get; set; }
        public string Message { get; set; }
        public bool Closable { get; set; }
        public string StrongMessage { get; set; }
    }
    public enum AlertMessageType
    {
        [StringValueClass("alert-info")]
        Info,
        [StringValueClass("alert-success")]
        Success,
        [StringValueClass("alert-warning")]
        Warning,
        [StringValueClass("alert-danger")]
        Danger
    }
}
