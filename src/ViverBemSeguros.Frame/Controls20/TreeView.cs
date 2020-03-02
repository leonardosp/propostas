using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Newtonsoft.Json;
using Enums;
using Celebre.Frame.FrameWork;
using System.Data;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class TreeView : StateManagedCompositeControl
    {
        private bool isExpandoObject = false;
        private object dataSource = null;
        private object tbvDataSource
        {
            get
            {
                object o = ViewState["CTableViewDataSource"];
                if (dataSource != null)
                    return dataSource;
                else if (dataSource == null && o != null && !string.IsNullOrWhiteSpace(o.ToString()))
                {
                    if (isExpandoObject)
                        return JsonConvert.DeserializeObject(o.ToString());
                    else
                    {
                        var auxDebug = ComponentUtils.DescompactObject(o.ToString());

                        if (auxDebug is DataTable)
                            return auxDebug;
                        else
                            return auxDebug;
                    }
                }
                else
                    return new List<object>();
            }
            set
            {
                if (value == null)
                    ViewState["CTableViewDataSource"] = null;
                else
                {
                    if (isExpandoObject)
                        ViewState["CTableViewDataSource"] = JsonConvert.SerializeObject(value, Formatting.Indented);
                    else
                        ViewState["CTableViewDataSource"] = ComponentUtils.CompactObject(value);
                }

                dataSource = value;
            }
        }
        public IList<Object> DataSource
        {
            get
            {
                if (tbvDataSource is DataTable)
                    return new List<object>();
                else
                    return (IList<object>)tbvDataSource;
            }
        }

        public string TextProperty { get; set; }
        public string ValueProperty { get; set; }
        public string ChildProperty { get; set; }

        public string[] SelectedValues { get; set; }

        private string GetChilds(object parent)
        {
            var child = ObjectHelper.GetPropertyValueObj(parent, ChildProperty);
            if (child != null && (child as IEnumerable<object>).Count() > 0)
            {

                List<string> itens = new List<string>();
                foreach (var item in (child as IEnumerable<object>))
                {
                    string value = ObjectHelper.GetPropertyValue(item, ValueProperty).ToString();
                    bool selected = false;

                    if (SelectedValues != null)
                        selected = SelectedValues.Contains(value);

                    itens.Add($"{{ \"text\": \"{ObjectHelper.GetPropertyValue(item, TextProperty).ToString()}\", \"value\": \"{value}\", \"state\": {{ \"selected\": {selected.ToString().ToLower()} }} {GetChilds(item)} }}");

                    child = ObjectHelper.GetPropertyValueObj(item, ChildProperty);
                }

                return $", \"nodes\": [ {string.Join(", ", itens)} ]";
            }

            return "";
        }
        public void SetDataSource(IEnumerable<Object> DataSource)
        {
            if (DataSource != null)
            {
                isExpandoObject = DataSource.Count() > 0 && DataSource.First() is System.Dynamic.ExpandoObject;
                this.tbvDataSource = DataSource.Select(p => p).ToList();
            }
            else
                this.tbvDataSource = null;
        }
        protected override void OnInit(EventArgs e)
        {
            var hdnArea = new System.Web.UI.WebControls.HiddenField();
            hdnArea.ID = ID + "_hdnArea";
            hdnArea.Value = "";
            Controls.Add(hdnArea);

            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if(Page.Request.Form.AllKeys.Any(p => p.Contains(ID + "_hdnArea")))
            {
                var bbb = Page.Request.Form.AllKeys.Where(p => p.Contains(ID + "_hdnArea"));
            }
           
            if (Page.IsPostBack && Page.Request.Form.AllKeys.Any(p => p.Contains(ID + "_hdnArea")))
            {
                var values = Page.Request.Form[Page.Request.Form.AllKeys.First(p => p.Contains(ID + "_hdnArea"))].ToString();
                SelectedValues = values.Split(',');
            }

            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            List<string> itens = new List<string>();
            foreach (var item in DataSource)
            {
                string value = ObjectHelper.GetPropertyValue(item, ValueProperty).ToString();
                bool selected = false;

                if (SelectedValues != null)
                    selected = SelectedValues.Contains(value);

                itens.Add($"{{ \"text\": \"{ObjectHelper.GetPropertyValue(item, TextProperty).ToString()}\", \"value\": \"{value}\", \"state\": {{ \"selected\": {selected.ToString().ToLower()} }} {GetChilds(item)} }}");
            }

            //writer.Write("<div>[{0}]</div>", string.Join(", ", itens));

            writer.Write("<div id='{0}' data-control='TreeView' class='hidden'>[{1}]</div>", ID, string.Join(", ", itens));

            base.Render(writer);
        }
        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/Controls/TreeView.min.js") + "' ></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<link rel='stylesheet' href='" + ResolveUrl("~/Includes/Styles/Controls/TreeView.min.css") + "'/>"));

            return listScriptsAndStyles;
        }
    }
}
