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
    [ToolboxData("<{0}:input runat='server'></{0}:input>")]
    public class Button : System.Web.UI.WebControls.Button, IPostBackEventHandler
    {
        private bool montagroup = false;
        private GroupButtons groupButtons = null;

        public bool AllowDoubleClick { get; set; }
        public string NotificationValue
        {
            get
            {
                object o = ViewState[ID + "NotificationValue"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState[ID + "NotificationValue"] = value; }
        }

        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public GroupButtons GroupButtons
        {
            get { return groupButtons ?? (groupButtons = new GroupButtons()); }
            set { groupButtons = value; }
        }
        public ButtonType ButtonType { get; set; }
        public FontAwesomeIcons ButtonIcon { get; set; }
        public FontAwesomeType ButtonIconType { get; set; }
        public ButtonSize ButtonSize { get; set; }
        public ButtonStyle ButtonStyle { get; set; }        

        public String DataField
        {
            get
            {
                object o = ViewState["CButtonDataField"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CButtonDataField"] = value; }
        }
        public String Name
        {
            get
            {
                object o = ViewState["ButtonName"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["ButtonName"] = value; }
        }
        public bool ToolBarButton { get; set; }
        /// <summary>
        /// Caminho para a pagina de manutenção onde o regitro será criado
        /// ex: FRO/Pessoa_item.aspx
        /// </summary>
        public String PathAddValue { get; set; }

        public override bool Visible
        {
            get
            {
                var o = ViewState["CButtonVisible"];
                return o == null || (Boolean)o;
            }
            set { ViewState["CButtonVisible"] = value; }
        }
        public Dictionary<string, string> DataValue { get; set; }
        public string ModalName { get; set; }
        public bool MontaGroup
        {
            get
            {
                return this.montagroup;
            }
            set
            {
                this.montagroup = value;
            }
        }
        [Browsable(true)]
        public event AddValueReturnFunctionEventHandler AddValueReturnFunction;

        [Browsable(false)]
        public int TableColumnButtonRowIndex { get; set; }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Button;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var item in GroupButtons)
                Controls.Add(item);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string cssclass = " " + this.CssClass;
            CssClass = string.Empty;

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlOpen("", this.ID, TypeControlForm.Default));

            if (DataValue != null && DataValue.Count > 0)
                Attributes.Add("data-values", string.Join(";", DataValue.Select(x => x.Key + "=" + x.Value).ToArray()));

            if (ButtonType == ButtonType.Default)
            {
                if (ToolBarButton)
                    this.CssClass = "btn btn-default ";
                else
                    this.CssClass += ButtonStyle.GetStringValueClass() + " ";
            }
            else if (ButtonType == ButtonType.QuickButton)
            {
                this.CssClass = "quick-button ";
                this.CssClass += ButtonStyle.GetStringValueClass() + " ";
            }
            else if (ButtonType == ButtonType.Inline)
            {
                CssClass = ButtonStyle.GetStringValueClass() + " btn-inline ";
                CssClass += cssclass;
            }

            if (ButtonSize != ButtonSize.NotSet)
            {
                this.CssClass += ButtonSize.GetStringValueClass() + " ";
            }

            if (!String.IsNullOrWhiteSpace(this.ToolTip))
            {
                this.Attributes.Add("data-toggle", "tooltip");
                this.Attributes.Add("title", this.ToolTip);
                this.Attributes.Add("data-placement", "bottom");

                this.ToolTip = string.Empty;
            }

            //Prevent Double Click
            this.Attributes.Add("AllowDoubleClick", AllowDoubleClick.ToString().ToLower());

            if (!String.IsNullOrWhiteSpace(this.ModalName))
            {
                this.Attributes.Add("data-toggle", "modal");
                this.Attributes.Add("data-target", string.Format("#{0}", this.ModalName));
                this.OnClientClick += $"$('#{ModalName}_OpenedModal').val('true');$('#{ModalName}').one('shown.bs.modal', function (e) {{modalAutoPositioning()}});return false;";
            }
            else if (!String.IsNullOrWhiteSpace(PathAddValue))
            {
                this.OnClientClick += String.Format("OpenAddModal(\"{0}\",\"../{1}\",\"{2}\");return false;", this.ClientID, PathAddValue, Page.ResolveClientUrl("~/Includes/images/loader.gif"));
            }

            this.UseSubmitBehavior = false;
            this.CssClass += cssclass;

            if (!string.IsNullOrWhiteSpace(NotificationValue))
                CssClass += " btn-notification";

            if (GroupButtons.Count(p=> p.Visible) > 0)
                writer.Write("<div class='btn-group'>");            

            base.Render(writer);

            if (GroupButtons.Count(p => p.Visible) > 0)
            {
                writer.Write("<button type='button' class='btn {0} dropdown-toggle' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'>", ToolBarButton ? "btn-default" : ButtonStyle.GetStringValueClass());
                writer.Write("<span class='caret'></span><span class='sr-only'>Toggle Dropdown</span></button>");
                writer.Write("<ul class='dropdown-menu'>");

                foreach (var item in GroupButtons.Where(p => p.Visible))
                    writer.Write("<li><a href='javascript:void(0)' onclick='__doPostBack(\"{0}\",\"\")'>{1}</a></li>", item.ID, item.Text);

                writer.Write("</ul>");
                writer.Write("</div>");
            }

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlClose("", TypeControlForm.Default));
        }
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (ButtonIcon != FontAwesomeIcons.NotSet)
            {
                string cssToAdd = "";
                if (ToolBarButton || !String.IsNullOrWhiteSpace(this.Text))
                    cssToAdd += " marginright5";

                writer.Write("<i class=\"{0} {1} {2}\"></i>", ButtonIconType.GetStringValueClass(), ButtonIcon.GetStringValueClass(), cssToAdd);

                if (ButtonType == ButtonType.QuickButton)
                    writer.Write(String.Format("<p>{0}</p>", this.Text));
                else
                    writer.Write(this.Text);
            }
            else
            {
                writer.Write(this.Text);
            }

            if (!string.IsNullOrWhiteSpace(NotificationValue))
                writer.Write("<span class='notification orange'>{0}</span>", NotificationValue);

            base.RenderContents(writer);
        }
        public void RaisePostBackEvent(string eventArgument)
        {
            if (AddValueReturnFunction != null && eventArgument.Contains(RaiseEvents.OnAddValueReturnFunctionEvent.ToString()))
                AddValueReturnFunction(this, new AddValueReturnFunctionEventArgs(eventArgument));

            else
                OnClick(new EventArgs());
        }

        private enum RaiseEvents
        {
            OnAddValueReturnFunctionEvent
        }
    }
    [Serializable]
    public class GroupButtons : List<Button>
    {

    }
    
    public enum ButtonType
    {
        Default,
        QuickButton,
        Inline
    }
    public enum ButtonSize
    {
        NotSet,
        [StringValueClass("btn-xs")]
        xSmall,
        [StringValueClass("btn-sm")]
        Small,
        [StringValueClass("btn-md")]
        Medium,
        [StringValueClass("btn-lg")]
        Large
    }
    public enum ButtonStyle
    {
        [StringValueClass("btn btn-default")]
        Default,
        [StringValueClass("btn btn-primary")]
        Primary,
        [StringValueClass("btn btn-danger")]
        Danger,
        [StringValueClass("btn btn-warning")]
        Warning,
        [StringValueClass("btn btn-info")]
        Info,
        [StringValueClass("btn btn-success")]
        Success,
        [StringValueClass("btn btn-inverse")]
        Inverse
    }
}
