using Celebre.Web.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public class Calendar : StateManagedCompositeControl, IPostBackEventHandler
    {
        public DateTime DefaultDate
        {
            get
            {
                object o = ViewState["CCalendarCompetencia"];
                if (o == null)
                    ViewState["CCalendarCompetencia"] = DateTime.Now;
                return (DateTime)ViewState["CCalendarCompetencia"];
            }
            set { ViewState["CCalendarCompetencia"] = value; }
        }
        public string ClientDayClick { get; set; }
        public string ClientEventClick { get; set; }
        public bool ShowTimeEvent { get; set; } = true;
        [Browsable(false)]
        public List<CalendarEvent> CalendarEvents
        {
            get
            {
                if (ViewState["CalendarDataSource"] == null)
                    CalendarEvents = new List<CalendarEvent>();

                return JsonConvert.DeserializeObject<IEnumerable<CalendarEvent>>(ViewState["CalendarDataSource"].ToString()).ToList();
            }
            set { ViewState["CalendarDataSource"] = JsonConvert.SerializeObject(value); }
        }

        public event EventHandler DayClick;
        public delegate void EventHandler(Object sender, DayClickEventArgs e);

        public event EventEventHandler EventClick;
        public delegate void EventEventHandler(Object sender, EventClickEventArgs e);

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.ToUpper().Contains("DAYCLICK"))
                DayClick?.Invoke(this, new DayClickEventArgs() { Date = eventArgument.Replace("DAYCLICK*", "") });
            else if (eventArgument.ToUpper().Contains("EVENTCLICK"))
                EventClick?.Invoke(this, new EventClickEventArgs() { EventId = eventArgument.Replace("EVENTCLICK*", "") });
        }

        protected override void OnInit(EventArgs e)
        {
            var hdnArea = new System.Web.UI.WebControls.HiddenField();
            hdnArea.ID = ID + "_hdnArea";
            hdnArea.Value = "";

            Controls.Add(hdnArea);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            var hdnArea = (System.Web.UI.WebControls.HiddenField)FindControl(ID + "_hdnArea");
            hdnArea.Value = JsonConvert.SerializeObject(CalendarEvents);
            hdnArea.RenderControl(writer);

            var dayclick = string.IsNullOrWhiteSpace(ClientDayClick) ? "" : $" data-control-jsClickDay='{ClientDayClick}'";
            dayclick += DayClick == null ? "" : $" data-control-serverClickDay=\"{Page.ClientScript.GetPostBackEventReference(this, "DAYCLICK")}\"";

            var eventclick = string.IsNullOrWhiteSpace(ClientEventClick) ? "" : $" data-control-jsClickEvent='{ClientEventClick}'";
            eventclick += EventClick == null ? "" : $" data-control-serverClickEvent=\"{Page.ClientScript.GetPostBackEventReference(this, "EVENTCLICK")}\"";

            var showTimeEvent = ShowTimeEvent ? 1 : 0;
            writer.Write($"<div id='{ID}' data-control='Calendar' data-control-ShowTimeEvent='{showTimeEvent}' data-control-defaultDate='{DefaultDate.ToString("yyyy-MM-dd")}'{dayclick}{eventclick}></div>");
        }

        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<link rel='stylesheet' href='" + ResolveUrl("~/Includes/Styles/Controls/fullcalendar.css") + "' />"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/moment.min.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/Controls/fullcalendar.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/locales/fullcalendar.pt-br.js") + "'></script>"));

            return listScriptsAndStyles;
        }
    }

    [Serializable]
    public class DayClickEventArgs : EventArgs
    {
        public String Date { get; set; }
    }

    [Serializable]
    public class EventClickEventArgs : EventArgs
    {
        public String EventId { get; set; }
    }

    [Serializable]
    public class CalendarEvent
    {
        public string id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        //public DateTime end { get; set; }
        //public string url { get; set; }
    }
}
