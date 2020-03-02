using Celebre.Web.Controls;
using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    [
        ParseChildren(true),
        PersistChildren(false),
        ToolboxData("<{0}:ListControl runat=\"server\"> </{0}:ListControl>")
        ]
    public class ListControl : StateManagedCompositeControl
    {
        private Control[,] arrayControles;
        private ListControlActionCollection actionButtonCollection = null;

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ListControlActionCollection ActionButtonCollection
        {
            get
            {
                if (actionButtonCollection == null)
                {
                    actionButtonCollection = new ListControlActionCollection();
                }
                return actionButtonCollection;
            }
        }

        public String DisplayedLabel { get; set; }
        public String BadgeLabel { get; set; }
        public String BadgeColor { get; set; }

        [Browsable(false)]
        public IEnumerable<Object> DataSource
        {
            get
            {
                object o = ViewState["ListControlDataSource"];
                return o == null ? new List<Object>() : (IEnumerable<Object>)o;
            }
            set { ViewState["ListControlDataSource"] = value; }
        }

        private void BindControls()
        {
            //Adiciona os controles (necesário para a chamada event handler)
            arrayControles = new Control[DataSource.Count(), actionButtonCollection.Count];

            //Carrega os componentes do Body
            int row = 0, column = 0;
            foreach (var item in DataSource)
            {
                column = 0;
                foreach (ListControlAction actionButton in actionButtonCollection)
                {
                    Control ctrl = actionButton.OnLoad(item);
                    ctrl.ID = string.Format("{0}{1}{2}", ctrl.ID, row, column);
                    this.Controls.Add(ctrl);
                    arrayControles[row, column] = ctrl;

                    column++;
                }
                row++;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write("<div class='todo-list'><table>");//ui-sortable

            int row = 0;
            foreach (var item in DataSource ?? Enumerable.Empty<object>())
            {
                output.Write("<tr>");

                output.Write(String.Format("<td class='todo-actions' style='width:{0}px'>", actionButtonCollection.Count * 30));
                int actionIndex = 0;
                actionButtonCollection.ForEach(action =>
                {
                    Control ctrl = arrayControles[row, actionIndex];
                    ctrl.RenderControl(output);

                    actionIndex++;
                });
                output.Write("</td>");

                output.Write(String.Format("<td class='todo-content'>{0}</td>", ObjectHelper.GetPropertyValue(item, DisplayedLabel).ToString()));

                if (!String.IsNullOrWhiteSpace(BadgeLabel))
                {
                    output.Write("<td class='todo-badge'><span class='{0}'>{1}</span></td>", ((ListControlBadgeColor)Enum.Parse(typeof(ListControlBadgeColor), ObjectHelper.GetPropertyValue(item, BadgeColor).ToString())).GetStringValueClass(), ObjectHelper.GetPropertyValue(item, BadgeLabel).ToString());
                }

                output.Write("</tr>");

                row++;
            }

            output.Write("</table></div>");
        }

        public override void DataBind()
        {
            base.OnDataBinding(EventArgs.Empty);

            Controls.Clear();

            BindControls();
        }

        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/assets/js/uncompressed/jquery-ui-1.10.3.custom.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/assets/js/jquery.flot.min.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/assets/js/jquery.flot.pie.min.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/assets/js/jquery.flot.categories.min.js") + "'></script>"));

            return listScriptsAndStyles;
        }
    }
    [Serializable]
    public class ListControlActionCollection : List<ListControlAction>
    {

    }
    [Serializable]
    public class ListControlAction : StateManagedItem
    {

        public String ID
        {
            get
            {
                object o = ViewState["ListControlActionID"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["ListControlActionID"] = value; }
        }
        public String DataField
        {
            get
            {
                object o = ViewState["ListControlActionDataField"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["ListControlActionDataField"] = value; }
        }

        [Browsable(true)]
        public event EventHandler Click;
        public delegate void EventHandler(Button sender, EventArgs e);

        public Control OnLoad(Object value)
        {
            //Create Control
            Button btn = new Button();
            btn.ID = ID;
            btn.Click += OnClick;
            btn.DataValue = new Dictionary<string, string>();

            //Resolve Data
            foreach (string arg in this.DataField.Split(';'))
            {
                btn.DataValue.Add(arg, ObjectHelper.GetPropertyValue(value, arg).ToString());
            }

            return btn;
        }

        protected virtual void OnClick(object sender, EventArgs e)
        {
            if (this.Click != null)
                Click((Button)sender, e);
        }
    }
    public enum ListControlBadgeColor
    {
        [StringValueClass("label label-default")]
        Default,
        [StringValueClass("label label-primary")]
        Primary,
        [StringValueClass("label label-success")]
        Success,
        [StringValueClass("label label-info")]
        Info,
        [StringValueClass("label label-warning")]
        Warning,
        [StringValueClass("label label-danger")]
        Danger
    }
}
