using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    public class Spinner : System.Web.UI.WebControls.TextBox
    {
        private bool campoobrigatorio = true;
        private bool montagroup = true;

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

        public string Label { get; set; }
        public int MaxValue
        {
            get
            {
                object o = ViewState["SpinnerMaxValue"];
                return o == null ? 0 : (int)o;
            }
            set { ViewState["SpinnerMaxValue"] = value; }
        }
        public int MinValue {
            get
            {
                object o = ViewState["SpinnerMinValue"];
                return o == null ? 0 : (int)o;
            }
            set { ViewState["SpinnerMinValue"] = value; }
        }
        public TypeControlForm TypeControl { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            CssClass = "form-control " + CssClass;
            Attributes.Add("data-control", "Spinner");
            Attributes.Add("data-control-required", campoobrigatorio.ToString());

            if (MaxValue > int.MinValue)
                Attributes.Add("data-control-maxvalue", MaxValue.ToString());
            if (MinValue > int.MinValue)
                Attributes.Add("data-control-minvalue", MinValue.ToString());

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlOpen(campoobrigatorio ? "*" + Label : Label, ID, TypeControl, true, false, ToolTip));

            base.Render(writer);

            writer.Write("<div class='input-group-addon input-group-btn-vertical'><a href='javascript:void(0)' class='btn btn-default'><i class='fa fa-caret-up'></i></a><a  href='javascript:void(0)' class='btn btn-default'><i class='fa fa-caret-down'></i></a></div>");

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlClose("", TypeControl, true));
        }
    }
}
