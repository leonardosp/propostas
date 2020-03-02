using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Data;
using System.Text;
using Celebre.Web.Utils;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Security.Permissions;
using System.Design;
using System.Web.UI.Design;
using System.Reflection;
using System.Text.RegularExpressions;
using Celebre.Web.Util;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace Celebre.Web.Controls
{
    public delegate void ChartClickEventHandler(object sender, ClickEventArgs e);

    [Serializable]
    [ParseChildren(true), PersistChildren(false), ToolboxData("<{0}:Chart runat=\"server\"> </{0}:Chart>")]
    public class Chart : StateManagedCompositeControl, IPostBackEventHandler
    {
        private string width;
        private string height;
        private string chartlabel;
        private string chartvalor;
        private bool showtableonclick;
        private string modaltitle;
        private bool closable;
        private bool enableclickarea;
        private TipoChartEnum tipoChart;
        private Dictionary<string, string> dataFieldsGrid = new Dictionary<string, string>();
        private string jsonFixedLines = "";

        [Browsable(true)]
        public event ChartClickEventHandler Click;
        [Browsable(false)]
        public Dictionary<string, string> DataFieldsGrid
        {
            get { return dataFieldsGrid; }
            set { dataFieldsGrid = value; }
        }

        public TipoChartEnum TipoChart
        {
            get { return tipoChart; }
            set { tipoChart = value; }
        }

        public string ChartWidth
        {
            get { return width; }
            set { width = value; }
        }
        public string ChartHeight
        {
            get { return height; }
            set { height = value; }
        }
        public string ChartLabel
        {
            get
            {
                if (string.IsNullOrEmpty(chartlabel))
                    chartlabel = "Label";

                return chartlabel;
            }
            set { chartlabel = value; }
        }
        public string ChartGroup
        {
            get
            {
                object o = ViewState["ChartGroup"];
                return o == null ? "" : o.ToString();
            }
            set { ViewState["ChartGroup"] = value; }
        }
        public string ChartValue
        {
            get { return chartvalor; }
            set { chartvalor = value; }
        }
        public string ChartAuxDisplay { get; set; }
        public bool ShowTableOnClick
        {
            get { return showtableonclick; }
            set { showtableonclick = value; }
        }
        public bool EnableClickArea
        {
            get { return enableclickarea; }
            set { enableclickarea = value; }
        }
        public string ModalTitle
        {
            get { return modaltitle; }
            set { modaltitle = value; }
        }
        public bool Closable
        {
            get { return closable; }
            set { closable = value; }
        }

        public string CssTable { get; set; }
        public bool ShowLabel { get; set; }
        public bool ShowLegend { get; set; }
        public decimal Angle { get; set; }
        public decimal InnerRadius { get; set; }
        public decimal Depth3D { get; set; }
        public string LoadJSFunction { get; set; }
        public FormatChartValue FormatValue { get; set; }
        public string ClientClick { get; set; }
        public List<FixedLine> FixedLines
        {
            get
            {
                object o = ViewState["ChartFixedLines"];

                if (o == null)
                {
                    ViewState["ChartFixedLines"] = new List<FixedLine>();
                    o = ViewState["ChartFixedLines"];
                }

                return (List<FixedLine>)o;
            }
            set { ViewState["ChartFixedLines"] = value; }
        }

        [Browsable(false)]
        public IEnumerable<Object> DataSource
        {
            get
            {
                List<dynamic> list = new List<dynamic>();
                IEnumerable<Object> o = JsonConvert.DeserializeObject<IEnumerable<Object>>(ViewState["CChartDataSource"].ToString());

                foreach (var item in o ?? new List<object>())
                    list.Add(JsonConvert.DeserializeObject<ExpandoObject>(item.ToString(), new ExpandoObjectConverter()));

                return list;

            }
            set { ViewState["CChartDataSource"] = JsonConvert.SerializeObject(value); }
        }
        [Browsable(false)]
        public void DataSourceTable(IEnumerable<Object> DataSourceTable)
        {
            var hdnArea = (HiddenField)FindControl("hdnArea");

            var modal = (Modal)FindControl(string.Concat(ID, "_modal"));
            modal.Title = string.IsNullOrEmpty(modaltitle) ? hdnArea.Value : string.Concat(modaltitle, " ", hdnArea.Value);

            hdnArea.Value = string.Empty;
            modal.Show();
        }
        [Browsable(false)]
        private StringBuilder sbData
        {
            get
            {
                object o = ViewState["CChartData"];
                return o == null ? new StringBuilder() : (StringBuilder)o;
            }
            set { ViewState["CChartData"] = value; }
        }
        private Dictionary<string, int> dic = new Dictionary<string, int>();

        public override void DataBind()
        {
            sbData = new StringBuilder();

            if (DataSource != null && DataSource.Any())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("Label");
                dt.Columns.Add("Group");
                dt.Columns.Add("Value");
                dt.Columns.Add("AuxDisplay");

                DataRow dr;
                int rowIndex = 0;

                foreach (var item in DataSource)
                {
                    dr = dt.NewRow();
                    dr["Id"] = rowIndex;
                    dr["Label"] = ObjectHelper.GetPropertyValue(item, ChartLabel).ToString();

                    if (!String.IsNullOrWhiteSpace(ChartGroup))
                    {
                        dr["Group"] = ObjectHelper.GetPropertyValue(item, ChartGroup);

                        if (!dic.ContainsKey(dr["Group"].ToString()))
                            dic.Add(dr["Group"].ToString(), dic.Keys.Count);
                    }

                    if (!String.IsNullOrWhiteSpace(ChartAuxDisplay))
                    {
                        dr["AuxDisplay"] = ObjectHelper.GetPropertyValue(item, ChartAuxDisplay);

                        if (!dic.ContainsKey(dr["AuxDisplay"].ToString()))
                            dic.Add(dr["AuxDisplay"].ToString(), dic.Keys.Count);
                    }

                    if (!String.IsNullOrWhiteSpace(ChartValue))
                        dr["Value"] = ObjectHelper.GetPropertyValue(item, ChartValue);

                    dt.Rows.Add(dr);
                    rowIndex++;
                }

                var labels = dt.AsEnumerable().Select(p => p.Field<string>("Label")).Distinct().OrderBy(p => p);
                var groups = dt.AsEnumerable().Select(p => p.Field<string>("Group")).Distinct().OrderBy(p => p);

                List<string> labelData = new List<string>();
                foreach (var label in labels)
                {
                    if (TipoChart == TipoChartEnum.Line)
                    {
                        string data = $"{{ \"group\":\"{label}\"";
                        foreach (var group in groups)
                        {
                            var total = dt.AsEnumerable().Where(p => p.Field<string>("Label") == label && p.Field<string>("Group") == group).Sum(p => p.Field<string>("Value").ToDecimal()).ToString("G", CultureInfo.InvariantCulture);
                            data += $",\"{group}\":{total}";
                        }
                        data += "}";
                        labelData.Add(data);
                    }
                    else if (TipoChart == TipoChartEnum.Bar && !string.IsNullOrWhiteSpace(ChartGroup))
                    {
                        string data = $"{{ \"label\":\"{label}\"";
                        foreach (var group in groups)
                        {
                            var total = dt.AsEnumerable().Where(p => p.Field<string>("Label") == label && p.Field<string>("Group") == group).Sum(p => p.Field<string>("Value").ToDecimal()).ToString("G", CultureInfo.InvariantCulture);
                            data += $",\"{group}\":{total}";
                        }

                        var linePoint = FixedLines.FirstOrDefault(p => p.Group == label);
                        if (linePoint != null)
                        {
                            data += $",\"{linePoint.Title}\":{linePoint.Value.ToString("G", CultureInfo.InvariantCulture)}";
                        }

                        data += "}";
                        labelData.Add(data);
                    }
                    else
                    {
                        labelData.Add(
                        string.Format("{{ \"label\": \"{0}\", \"value\": {1}{2} }}", label,
                            String.IsNullOrWhiteSpace(ChartValue) ? dt.AsEnumerable().Count(p => p.Field<string>("Label") == label).ToString() :
                                FormatValue == FormatChartValue.Integer ? dt.AsEnumerable().Where(p => p.Field<string>("Label") == label).Sum(p => p.Field<string>("Value").ToInt32()).ToString() :
                                dt.AsEnumerable().Where(p => p.Field<string>("Label") == label).Sum(p => p.Field<string>("Value").ToDecimal()).ToString("G", CultureInfo.InvariantCulture),
                            string.IsNullOrWhiteSpace(ChartAuxDisplay) ? "" : $", \"auxDisplay\": \"{string.Join(" ", dt.AsEnumerable().Where(p => p.Field<string>("Label") == label).Select(p => p.Field<string>("AuxDisplay")).ToList())}\""
                        ));
                    }
                }
                sbData.Append(string.Join(",", labelData.ToArray()));
            }

            if (FixedLines.Count > 0)
            {
                List<string> aux = new List<string>();
                foreach (var line in FixedLines)
                    aux.Add($"{{ \"dashLength\":{line.DashLenght},\"inside\": {line.Inside.ToString().ToLower()},\"lineAlpha\": {line.LineAlpha}, \"label\": \"{line.Title}\", \"value\": {line.Value.ToString("G", CultureInfo.InvariantCulture)} }}");

                jsonFixedLines = string.Join(",", aux.ToArray());
            }
        }
        public string GetClickedValue()
        {
            return ((System.Web.UI.WebControls.HiddenField)this.FindControl("hdnArea")).Value;
        }
        public string GetDataSourceJson { get { return $"{{ \"value\": [{sbData.ToString()}], \"axesGuides\":[{jsonFixedLines.ToString()}]}}"; } }

        protected override void OnInit(EventArgs e)
        {
            StringBuilder styleValueInner = new StringBuilder();

            if (string.IsNullOrEmpty(width))
                width = "300";

            width = width.Replace("px", "");

            styleValueInner.AppendFormat("width: 99%; ");

            if (string.IsNullOrEmpty(height))
                height = "300";

            height = height.Replace("px", "");

            styleValueInner.AppendFormat("height:{0}px; ", int.Parse(height) - 80);
            styleValueInner.Append("margin-top: 5px;");

            var bodyDiv = new HtmlGenericControl("div");
            bodyDiv.Attributes.Add("class", "chartcontainer");
            bodyDiv.Attributes.Add("style", styleValueInner.ToString());
            this.Controls.Add(bodyDiv);

            var footP = new HtmlGenericControl("p");
            footP.Attributes.Add("class", "footerInfo");
            footP.Attributes.Add("style", "height: 20px; margin: 10 0 0;");
            this.Controls.Add(footP);

            var hdnArea = new System.Web.UI.WebControls.HiddenField();
            hdnArea.ID = "hdnArea";
            hdnArea.Value = "";
            this.Controls.Add(hdnArea);

            if (showtableonclick)
            {
                var modal = new Modal();
                modal.ID = string.Concat(this.ID, "_modal");
                modal.BtnFechar = true;

                this.Controls.Add(modal);
            }
        }
        protected virtual void OnClick(ClickEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div id={0} data-control='Chart' data-charttype='{1}' data-charvalueformat='{2}' data-chartname='{3}' data-chart-showLegend='{4}'{5}{6}{7}{8}{9}{10}{11}>",
                UniqueID, tipoChart.ToString(), FormatValue.ToString(), ID, ShowLegend,
                Click != null ? $" data-chart-serverclick=\"{Page.ClientScript.GetPostBackEventReference(this, RaiseEvents.OnClickEvent.ToString())}\" " : "",
                !string.IsNullOrWhiteSpace(ClientClick) ? $" data-chart-clientclick=\"{ClientClick}\" " : "",
                $" data-chart-angle='{Angle}' ",
                $" data-chart-innerRadius='{InnerRadius}' ",
                $" data-chart-depth3D='{Depth3D}' ",
                $" data-chart-hasAuxDisplay='{!string.IsNullOrWhiteSpace(ChartAuxDisplay)}'",
                !string.IsNullOrWhiteSpace(LoadJSFunction) ? $"data-chart-loadfunction={LoadJSFunction}" : "");
            writer.Write("<div class='value' style='display:none;'>[{0}]</div>", sbData);
            writer.Write("<div class='xaxis' style='display:none;'>[{0}]</div>", String.Join(",", dic.Select(p => "[" + p.Value + ",\"" + p.Key + "\"]")));
            writer.Write("<div class='axesGuides' style='display:none;'>[{0}]</div>", jsonFixedLines);
            base.Render(writer);

            writer.Write("</div>");
        }
        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/amcharts/amcharts.js") + "'></script>"));
            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/amcharts/themes/light.js") + "'></script>"));

            switch (TipoChart)
            {
                case TipoChartEnum.Pie:
                    listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/amcharts/pie.js") + "'></script>"));
                    break;
                case TipoChartEnum.Line:
                case TipoChartEnum.Bar:
                    listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/amcharts/serial.js") + "'></script>"));
                    break;
                case TipoChartEnum.NaoDefinido:
                default:
                    break;
            }

            return listScriptsAndStyles;
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            OnClick(new ClickEventArgs());
        }
        private enum RaiseEvents
        {
            OnClickEvent
        }
    }

    [Serializable]
    public class ClickEventArgs : EventArgs
    {

    }

    [Serializable]
    public class FixedLine
    {
        private int dashlenght = 6;
        private bool inside = true;
        private int lineAlpha = 1;

        public int DashLenght { get { return dashlenght; } set { dashlenght = value; } }
        public bool Inside { get { return inside; } set { inside = value; } }
        public int LineAlpha { get { return lineAlpha; } set { lineAlpha = value; } }

        public string Title { get; set; }
        public decimal Value { get; set; }
        public string Group { get; set; }
    }
    public enum TipoChartEnum
    {
        NaoDefinido,

        [StringValueText("Pie")]
        Pie,

        [StringValueText("Bar")]
        Bar,

        [StringValueText("Line")]
        Line,
    }
    public enum FormatChartValue
    {
        [StringValueText("Integer")]
        Integer,

        [StringValueText("Decimal")]
        Decimal,

        [StringValueText("Currency")]
        Currency,
    }
}
