using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using Celebre.Web.Utils;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class RadioButtonGroup : StateManagedCompositeControl
    {
        private string label;
        private string help;
        private bool campoobrigatorio = true;
        private RadioButtons radioButtons = null;
        private bool inline = true;

        /// <summary>
        /// Define o Template do Radio Button
        /// </summary>
        public RadioButtonTemplate Template { get; set; }
        public TypeControlForm TypeControl { get; set; }

        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public RadioButtons RadioButtons
        {
            get { return radioButtons ?? (radioButtons = new RadioButtons()); }
            set { radioButtons = value; }
        }

        public bool Inline
        {
            get { return inline; }
            set { inline = value; }
        }
        public String GroupName { get; set; }
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            foreach (var item in radioButtons)
                item.ParentGroup = this;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var item in this.radioButtons)
            {
                if (String.IsNullOrWhiteSpace(GroupName))
                    GroupName = ID;

                item.GroupName = GroupName;
                Controls.Add(item);
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.label : this.label, this.ID, TypeControl));

            if (Template == RadioButtonTemplate.Switch)
                writer.Write("<div class='btn-group radio-button-group' data-toggle='buttons'{0}>", this.radioButtons.Any(p => p.AutoPostBack == true) ? " data-control='Radio'" : "");

            foreach (var item in this.radioButtons)
            {
                if (Template == RadioButtonTemplate.Default)
                {
                    writer.Write(string.Format("<label class='{0}'>", inline ? "radio-inline" : "radio"));
                    item.RenderControl(writer);
                    writer.Write("</label>");
                }
                else if (Template == RadioButtonTemplate.Switch)
                {
                    writer.Write("<label class='btn btn-default{0}{1}'{2}>", item.Checked ? " active" : "", Enabled ? "" : " disabled", item.AutoPostBack ? " data-post='true'" : "");
                    item.RenderControl(writer);
                    writer.Write("</label>");
                }
            }

            if (Template == RadioButtonTemplate.Switch)
                writer.Write("</div>");

            writer.Write(ComponentUtils.RenderControlClose(help, TypeControl));
        }
    }
    [Serializable]
    [ParseChildren(false), PersistChildren(false)]
    public class RadioButton : System.Web.UI.WebControls.RadioButton
    {
        public RadioButtonGroup ParentGroup { get; set; }

        public override bool Checked
        {
            get => base.Checked;
            set
            {
                if (value && ParentGroup != null)
                {
                    var radiobuttons = ParentGroup.RadioButtons;

                    foreach (var item in radiobuttons)
                        item.Checked = false;
                }

                base.Checked = value;
            }
        }

        [Browsable(false)]
        public override string GroupName
        {
            get
            {
                return base.GroupName;
            }
            set
            {
                base.GroupName = value;
            }
        }
    }
    [Serializable]
    public class RadioButtons : List<RadioButton>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }

    public enum RadioButtonTemplate
    {
        Default,
        Switch
    }
}
