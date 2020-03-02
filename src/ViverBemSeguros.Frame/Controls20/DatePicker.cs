using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class DatePicker : System.Web.UI.WebControls.TextBox
    {
        private bool campoobrigatorio = true;
        private bool obrigatoriovazio;
        private string cssimage;
        private string label;
        private string help;
        private bool montagroup = true;
        public TypeControlForm TypeControl { get; set; }

        public bool ObrigatorioVazio
        {
            get
            {
                return obrigatoriovazio;
            }
            set
            {
                obrigatoriovazio = value;
            }
        }
        public string CSSImage
        {
            get { return cssimage; }
            set { cssimage = value; }
        }
        public String DataField { get; set; }
        public String DefaultValue { get; set; }
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("DefaultValue", DefaultValue);
            base.AddAttributesToRender(writer);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            MaxLength = 10;

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.label : this.label, this.ID, TypeControl, !string.IsNullOrEmpty(cssimage), ObrigatorioVazio, ToolTip));

            writer.Write("<div class=\"controls\">");
            writer.Write("<div class=\"input-group col-sm-12\" data-control=\"DatePicker\">");
            base.Render(writer);
            writer.Write("<span class=\"input-group-addon\"><i class=\"far fa-calendar-alt\"></i></span></div></div>");

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlClose(help, TypeControl, !string.IsNullOrEmpty(cssimage)));
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

        public string Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        public string Help
        {
            get
            {
                return this.help;
            }
            set
            {
                this.help = value;
            }
        }

        public DatePicker()
        {
            this.CssClass = "form-control";
        }

        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/bootstrap-datepicker.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/locales/bootstrap-datepicker.pt-BR.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<link rel='stylesheet' href='" + ResolveUrl("~/Includes/Styles/datepicker3.css") + "'/>"));

            return listScriptsAndStyles;
        }
    }
}
