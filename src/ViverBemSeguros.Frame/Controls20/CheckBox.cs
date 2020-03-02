using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class CheckBox : System.Web.UI.WebControls.CheckBox
    {
        private bool montagroup = true;
        private bool campoobrigatorio = true;

        public CheckBox()
            : base()
        {
            DataValue = new Dictionary<string, string>();
            CheckedLabel = "On";
            UncheckedLabel = "Off";
        }

        /// <summary>
        /// Define o Estilo(cores) do Checkbox(se o Template for switch)
        /// </summary>
        public CheckBoxStyle CheckboxStyle { get; set; }
        /// <summary>
        /// Define o template do checkbox
        /// </summary>
        public CheckBoxTemplate Template { get; set; }
        public String DataField { get; set; }
        public String OnClientChangeFunction { get; set; }
        public Dictionary<string, string> DataValue { get; set; }
        /// <summary>
        /// Define o texto que aparecera quando o checkbox for marcado
        /// Funciona apenas com o template switch
        /// </summary>
        public String CheckedLabel { get; set; }
        /// <summary>
        /// Define o texto que aparecera quando o checkbox for desmarcado
        /// Funciona apenas com o template switch
        /// </summary>
        public String UncheckedLabel { get; set; }
        public String Label { get; set; }
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
        public bool CampoObrigatorio
        {
            get
            {
                return this.campoobrigatorio;
            }
            set
            {
                this.campoobrigatorio = value;
            }
        }
        public TypeControlForm TypeControl { get; set; }
        public String Help { get; set; }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (montagroup)
                writer.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.Label : this.Label, this.ID, TypeControl, true, false, ToolTip));

            if (Template == CheckBoxTemplate.Default)
            {
                writer.Write("<div class='celebreInput'>");
                base.Render(writer);
                writer.Write("</div>");
            }
            else if (Template == CheckBoxTemplate.Switch)
            {
                //writer.Write("<label class='switch {0}'>", CheckboxStyle.GetStringValueClass());
                this.InputAttributes.Add("data-control", "Switch");

                if (CheckboxStyle != CheckBoxStyle.Default)
                    this.InputAttributes.Add("data-on-color", CheckboxStyle.GetStringValueClass());

                this.InputAttributes.Add("data-on-text", CheckedLabel);
                this.InputAttributes.Add("data-off-text", UncheckedLabel);
                this.Text = "";

                if (!string.IsNullOrWhiteSpace(OnClientChangeFunction))
                    this.InputAttributes.Add("data-client-change", OnClientChangeFunction);

                if (AutoPostBack)
                    this.InputAttributes.Add("onSwitchChange", "true");

                base.Render(writer);
            }

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlClose(Help, TypeControl));
        }
        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setFocusNextControl", "setFocusNextControl(" + this.ClientID + ");", true);
        }
        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            if (Template == CheckBoxTemplate.Switch)
                listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/bootstrap-switch.js") + "'></script>"));

            return listScriptsAndStyles;
        }
    }

    public enum CheckBoxTemplate
    {
        Default,
        Switch
    }
    public enum CheckBoxStyle
    {
        [StringValueClass("")]
        Default,
        [StringValueClass("primary")]
        Primary,
        [StringValueClass("danger")]
        Danger,
        [StringValueClass("warning")]
        Warning,
        [StringValueClass("info")]
        Info,
        [StringValueClass("success")]
        Success,
        [StringValueClass("inverse")]
        Inverse
    }
}
