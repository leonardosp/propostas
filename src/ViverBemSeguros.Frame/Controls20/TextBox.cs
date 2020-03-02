using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Celebre.Web.Pages;
using Celebre.Frame.Util;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;
using Celebre.Web.Utils;
using Celebre.Web.Util;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class TextBox : System.Web.UI.WebControls.TextBox
    {
        private bool campoobrigatorio = true;
        private bool obrigatoriovazio = false;
        private string label;
        private string help;
        private bool montagroup = true;
        private string datafieldname;
        private string custommask;
        private int decimalplaces;
        private InputGroupButtons inputGroupButtons = null;

        public int DecimalPlaces
        {
            get
            {
                return this.decimalplaces;
            }
            set
            {
                this.decimalplaces = value;
            }
        }
        public string DataFieldName
        {
            get
            {
                return this.datafieldname;
            }
            set
            {
                this.datafieldname = value;
            }
        }
        public string CustomMask
        {
            get { return custommask; }
            set { custommask = value; }
        }
        public MaskTextBox Mask
        {
            get
            {
                if (ViewState["MaskTextBox"] == null)
                    return MaskTextBox.NaoDefinido;

                return (MaskTextBox)ViewState["MaskTextBox"];
            }
            set { ViewState["MaskTextBox"] = value; }
        }
        public FontAwesomeIcons TextBoxIcon { get; set; }
        public FontAwesomeType TextBoxIconType { get; set; }
        public String DataField { get; set; }
        public String DefaultValue { get; set; }
        public TypeControlForm TypeControl { get; set; }
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
        public bool AllowNegativeValues { get; set; }
        public bool ObrigatorioVazio
        {
            get
            {
                return this.obrigatoriovazio;
            }
            set
            {
                this.obrigatoriovazio = value;
            }
        }
        /// <summary>
        /// Acessivel apenas dentro do Framework
        /// </summary>
        internal string TableViewEventArgs
        {
            get
            {
                if (ViewState["TXTTableViewEventArgs"] == null)
                {
                    return "";
                }
                else
                {
                    return ViewState["TXTTableViewEventArgs"].ToString();
                }

            }
            set
            {
                ViewState["TXTTableViewEventArgs"] = value;
            }
        }
        public bool AutoComplete { get; set; } = false;

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
        [Browsable(false)]
        public object TableViewItem { get; set; }
        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public InputGroupButtons GroupButtons
        {
            get { return inputGroupButtons ?? (inputGroupButtons = new InputGroupButtons()); }
            set { inputGroupButtons = value; }
        }
        public override string Text
        {
            get
            {
                switch (Mask)
                {
                    case MaskTextBox.CPF:
                    case MaskTextBox.CNPJ:
                    case MaskTextBox.PIS:
                        return base.Text.Replace(".", "").Replace("-", "").Replace("/", "");
                    case MaskTextBox.Phone:
                        return base.Text.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                    case MaskTextBox.Placa:
                    case MaskTextBox.CEP:
                        return base.Text.Replace("-", "");
                    case MaskTextBox.MesAno:
                        return base.Text.Replace("/", "");
                    case MaskTextBox.Decimal:
                    case MaskTextBox.Integer:
                        return base.Text.Replace(".", "");
                    case MaskTextBox.CodigoBarras:
                        return CelFunctions.RegExMatechesToString(Regex.Matches(base.Text, @"\d+"));
                    case MaskTextBox.Custom:
                    case MaskTextBox.Email:
                    case MaskTextBox.Hour:
                    case MaskTextBox.NaoDefinido:
                    default:
                        return base.Text;
                }
            }
            set
            {
                base.Text = value;
            }
        }
        public string MaskedText
        {
            get
            {
                return base.Text;
            }
        }
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("DefaultValue", DefaultValue);
            base.AddAttributesToRender(writer);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var item in GroupButtons)
                Controls.Add(item);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.CssClass = "form-control " + this.CssClass;

            StringBuilder sb = new StringBuilder();

            Attributes.Add("autocomplete", AutoComplete ? "on" : "off");

            if (this.HasEvent("EventTextChanged"))
                this.AutoPostBack = true;

            if (Mask != MaskTextBox.NaoDefinido)
            {
                if (Mask == MaskTextBox.Decimal || Mask == MaskTextBox.Integer)
                {
                    Attributes.Add("data-control-allownegative", AllowNegativeValues.ToString());

                    if (Mask == MaskTextBox.Decimal && DecimalPlaces > 0)
                        Attributes.Add("data-control-decimalplaces", DecimalPlaces.ToString());
                }

                this.Attributes.Add("data-control", Mask.ToString());
            }

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.label : this.label, this.ID, TypeControl, TextBoxIcon != FontAwesomeIcons.NotSet, ObrigatorioVazio, ToolTip));

            base.Attributes.Add("data-control-required", campoobrigatorio.ToString());

            if (TextBoxIcon != FontAwesomeIcons.NotSet)
            {
                writer.Write("<span class=\"input-group-addon\"><i class=\"{0} {1}\"></i></span>", TextBoxIconType.GetStringValueClass(), TextBoxIcon.GetStringValueClass());
                base.Render(writer);
            }
            else
            {
                if (GroupButtons.Count > 0)
                {
                    writer.Write("<div class='input-group'>");

                    if (GroupButtons.Count(p => p.ButtonPosition == InputGroupButton.InputGroupButtonPosition.Left) > 0)
                    {
                        writer.Write("<div class='input-group-btn'>");

                        foreach (var btn in GroupButtons.Where(p => p.ButtonPosition == InputGroupButton.InputGroupButtonPosition.Left))
                            btn.RenderControl(writer);

                        writer.Write("</div>");
                    }
                }


                base.Render(writer);

                if (GroupButtons.Count > 0)
                {
                    if (GroupButtons.Count(p => p.ButtonPosition == InputGroupButton.InputGroupButtonPosition.Right) > 0)
                    {

                        writer.Write("<div class='input-group-btn'>");

                        foreach (var btn in GroupButtons.Where(p => p.ButtonPosition == InputGroupButton.InputGroupButtonPosition.Right))
                            btn.RenderControl(writer);

                        writer.Write("</div>");
                    }

                    writer.Write("</div>");
                }
            }

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlClose(help, TypeControl, TextBoxIcon != FontAwesomeIcons.NotSet));
        }
        private bool HasEvent(string eventName)
        {
            EventHandlerList events =
                (EventHandlerList)
                (this.GetType()).BaseType
                 .GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance)
                 .GetValue(this, null);

            if ((this.GetType()).BaseType
                .GetField(eventName, BindingFlags.NonPublic | BindingFlags.Static) != null)
            {
                object key = (this.GetType()).BaseType
                    .GetField(eventName, BindingFlags.NonPublic | BindingFlags.Static)
                    .GetValue(null);

                Delegate handlers = events[key];

                return handlers != null && handlers.GetInvocationList().Any();
            }
            else
                return false;
        }
        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            if (Mask != MaskTextBox.NaoDefinido || !String.IsNullOrWhiteSpace(CustomMask))
            {
                if (Mask == MaskTextBox.Decimal || Mask == MaskTextBox.Integer)
                    listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/jquery.mask.money.js") + "'></script>"));
                else
                    listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/jquery.mask.js") + "'></script>"));
            }

            return listScriptsAndStyles;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (UniqueID == Page.Request.Params["__EVENTTARGET"])
            {
                base.OnTextChanged(e);

                if (this.Page != null)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "setFocusNextControl", "setFocusNextControl(" + this.ClientID + ");", true);
            }
        }

        public enum MaskTextBox
        {
            NaoDefinido,

            [StringValueText("Custom")]
            Custom,
            [StringValueText("CodigoBarras")]
            CodigoBarras,
            [StringValueText("CPF")]
            CPF,
            [StringValueText("CNPJ")]
            CNPJ,
            [StringValueText("Email")]
            Email,
            [StringValueText("Emails")]
            Emails,
            [StringValueText("Phone")]
            Phone,
            [StringValueText("Placa")]
            Placa,
            [StringValueText("CEP")]
            CEP,
            [StringValueText("Decimal")]
            Decimal,
            [StringValueText("Integer")]
            Integer,
            [StringValueText("Hour")]
            Hour,
            [StringValueText("MESANO")]
            MesAno,
            [StringValueText("PIS")]
            PIS
        }
    }

    [Serializable]
    public class InputGroupButtons : List<InputGroupButton>
    {

    }

    public class InputGroupButton : Button
    {
        public InputGroupButtonPosition ButtonPosition { get; set; }
        public enum InputGroupButtonPosition { Left, Right }
    }
}