using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class MultiSelectList : StateManagedCompositeControl
    {
        private string[] selectedvalues;
        private string label;
        private bool campoobrigatorio = true, montagroup = true;

        public IEnumerable<object> DataSource
        {
            get
            {
                object o = ViewState["MultiSelectListDataSource"];
                return o == null ? new List<Object>() : (IEnumerable<Object>)o;
            }
            private set { ViewState["MultiSelectListDataSource"] = value; }
        }
        public string ValuePropertyName
        {
            get
            {
                object o = ViewState["MultiSelectListvaluePropertyName"];
                return o == null ? "" : o.ToString();
            }
            private set { ViewState["MultiSelectListvaluePropertyName"] = value; }
        }
        public string DisplayPropertyName
        {
            get
            {
                object o = ViewState["MultiSelectListdisplayPropertyName"];
                return o == null ? "" : o.ToString();
            }
            private set { ViewState["MultiSelectListdisplayPropertyName"] = value; }
        }
        public string[] SelectedValues { get { return selectedvalues; } }
        public void AddSelectedValue(string selectedValue)
        {
            var aux = selectedvalues == null ? new List<string>() : selectedvalues.ToList();
            aux.Add(selectedValue);
            selectedvalues = aux.ToArray();

            ((HiddenField)FindControl("MultiselectValues")).Value = string.Join(",", selectedvalues);
        }
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

        public void Reset()
        {
            selectedvalues = null;
            ((HiddenField)FindControl("MultiselectValues")).Value = "";
        }
        public void Carrega(string valuePropertyName, string displayPropertyName, IEnumerable<object> datasource)
        {
            ValuePropertyName = valuePropertyName;
            DisplayPropertyName = displayPropertyName;
            DataSource = datasource;

            DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            var hdnMultiselect = new HiddenField();
            hdnMultiselect.ID = "MultiselectValues";
            Controls.Add(hdnMultiselect);

        }
        protected override void OnLoad(EventArgs e)
        {
            if (Page.Request.Form[ClientID.Replace("_", "$") + "$MultiselectValues"] != null)
                selectedvalues = Page.Request.Form[ClientID.Replace("_", "$") + "$MultiselectValues"].Split(new char[] { ',' });

            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (montagroup)
                writer.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.label : this.label, this.ID, TypeControl, false));

            writer.Write("<select id='{0}' multiple='multiple' data-control='Multiselect'>", ClientID);

            foreach (var item in DataSource)
            {
                var value = ObjectHelper.GetPropertyValue(item, ValuePropertyName);

                bool selected = selectedvalues == null ? false : selectedvalues.Contains(value);

                writer.Write("<option value='{0}' text='{1}'{2}>{1}</option>", value, ObjectHelper.GetPropertyValue(item, DisplayPropertyName), selected ? " selected='selected'" : "");
            }

            writer.Write("</select>");

            base.Render(writer);

            if (montagroup)
                writer.Write(ComponentUtils.RenderControlClose("", TypeControl, false));
        }
        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/Controls/Multiselect.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<link rel='stylesheet' href='" + ResolveUrl("~/Includes/Styles/Multiselect.css") + "'/>"));

            return listScriptsAndStyles;
        }
    }
}

