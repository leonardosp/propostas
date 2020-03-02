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
using Celebre.Web.Util;
using Celebre.Frame.FrameWork;
using System.Collections;
using static Celebre.FrameWork.Util;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;




namespace Celebre.Web.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public class TableView : StateManagedCompositeControl, IPostBackEventHandler

    {
        private TableViewCollection columnsCollection = null;
        private Control[,] arrayControles;
        private bool isExpandoObject = false;


        private Control[] arrayFooterControl;
        List<int> pageSizeItems;
        String indexColumn, rowvisiblefield;
        DataTable dtFiltered;
        int totalPage, firstPage, lastPage;
        bool showfooter = true, tablestriped = true, tablebordered = true, tablehover = true, tablecondensed = true, hideremovedstatus;
        private Nullable<Boolean> resetControls;

        [Browsable(true)]
        public event EventHandler RowClick;
        public delegate void EventHandler(Object sender, RowClickEventArgs e);

        /// <summary>
        ///     String contendo a propriedade chave primaria.
        ///     Quando mais de uma deve ser separada por ; (ponto e virgula)
        /// </summary>
        public String DataKeys
        {
            get
            {
                object o = ViewState["CTableViewDataKeys"];
                return o == null ? String.Empty : o.ToString();
            }
            set { ViewState["CTableViewDataKeys"] = value; }
        }

        public List<String> IDs_TableColumnInput
        {
            get
            {
                List<String> o = (List<String>)ViewState["IDs_TableColumnInput"];
                if (o == null)
                {

                    o = new List<String>();
                    ViewState["IDs_TableColumnInput"] = o;
                }

                return (List<String>)ViewState["IDs_TableColumnInput"];
            }
            set { ViewState["IDs_TableColumnInput"] = value; }
        }



        public bool DisableRowClick
        {
            get
            {
                object o = ViewState["CTableDisableRowClickGrid"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["CTableDisableRowClickGrid"] = value; }
        }

        public bool KeepPage
        {
            get
            {
                return keeppage;
            }

            set
            {
                keeppage = value;
            }
        }

        private bool keeppage;

        public String GroupKeysProperties
        {
            get
            {
                object o = ViewState["CTableViewGroupKeysProperties"];
                return o == null ? String.Empty : o.ToString();
            }
            set { ViewState["CTableViewGroupKeysProperties"] = value; }
        }

        public Dictionary<String, String> Filters
        {
            get
            {
                object o = ViewState["CTableViewFilters"];
                return o == null ? new Dictionary<String, String>() : (Dictionary<String, String>)o;
            }
            set { ViewState["CTableViewFilters"] = value; }
        }

        public Tuple<String, String> SortingColumn
        {
            get
            {
                object o = ViewState["CTableViewSortingColumn"];
                return o == null ? new Tuple<String, String>("", "") : (Tuple<String, String>)o;
            }
            set { ViewState["CTableViewSortingColumn"] = value; }
        }

        public List<String> CheckboxFieldsIDCheckeds
        {
            get
            {
                if (ViewState["CTableViewCheckboxFieldsIDCheckeds"] == null)
                    ViewState["CTableViewCheckboxFieldsIDCheckeds"] = new List<String>();

                return (List<String>)ViewState["CTableViewCheckboxFieldsIDCheckeds"];
            }
            set { ViewState["CTableViewCheckboxFieldsIDCheckeds"] = value; }
        }

        public String SelectedPageSize
        {
            get
            {
                object o = ViewState["CTableViewSelectedPageSize"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CTableViewSelectedPageSize"] = value; }
        }

        public Int32 PageIndex
        {
            get
            {
                object o = ViewState["CTableViewPageIndex"];
                return o == null ? 1 : (Int32)o;
            }
            set { ViewState["CTableViewPageIndex"] = value; }
        }

        public Int32 PageIndexOld
        {
            get
            {
                object o = ViewState["CTableViewPageIndexOld"];
                return o == null ? 1 : (Int32)o;
            }
            set { ViewState["CTableViewPageIndexOld"] = value; }
        }

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

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public TableViewCollection ColumnsCollection
        {
            get
            {
                if (columnsCollection == null)
                {
                    columnsCollection = new TableViewCollection();
                }
                return columnsCollection;
            }
            set { columnsCollection = value; }
        }

        public bool LimparIDs
        {
            get
            {
                if (ViewState[this.ID + " - limparIDs"] == null)
                    ViewState[this.ID + " - limparIDs"] = false;

                return (bool)ViewState[this.ID + " - limparIDs"];
            }
            set { ViewState[this.ID + " - limparIDs"] = value; }
        }

        public int PageSize { get; set; }
        public bool ShowFooter { get { return showfooter; } set { showfooter = value; } }
        public bool TableStriped { get { return tablestriped; } set { tablestriped = value; } }
        public bool TableBordered { get { return tablebordered; } set { tablebordered = value; } }
        public bool TableHover { get { return tablehover; } set { tablehover = value; } }
        public bool TableCondensed { get { return tablecondensed; } set { tablecondensed = value; } }
        public string RowVisibleField { get { return rowvisiblefield; } set { rowvisiblefield = value; } }
        public bool HideRemovedStatus { get { return hideremovedstatus; } set { hideremovedstatus = value; } }
        public string RowCssClassDataField { get; set; }
        public String RowTooltipField { get; set; }
        public bool AutoGenerateColumns { get; set; } = false;
        public string Title { get; set; }

        private int FirstPageSet
        {
            get
            {
                object o = ViewState["CTableFirstPageSet"];
                return o == null ? 0 : (Int32)o;
            }
            set { ViewState["CTableFirstPageSet"] = value; }
        }
        private int LastPageSet
        {
            get
            {
                object o = ViewState["CTableLastPageSet"];
                return o == null ? 0 : (Int32)o;
            }
            set { ViewState["CTableLastPageSet"] = value; }
        }

        public TableView()
        {
            pageSizeItems = new List<int>();
            columnsCollection = new TableViewCollection();

            if (ShowFooter)
            {
                pageSizeItems.Add(5);
                pageSizeItems.Add(15);
                pageSizeItems.Add(50);
                pageSizeItems.Add(100);
                pageSizeItems.Add(500);

                PageSize = pageSizeItems[1];
            }
        }

        private void LoadFooter()
        {
            DropDownList ddlPageSize = new DropDownList();
            ddlPageSize.CampoObrigatorio = false;
            ddlPageSize.ID = "ddlPageSize_" + ID;
            ddlPageSize.CssClass = "form-control";
            ddlPageSize.MontaGroup = false;
            ddlPageSize.SelectedIndexChanged += OnChangePageSize;

            if (!pageSizeItems.Any(x => x == PageSize))
                pageSizeItems.Add(PageSize);

            pageSizeItems = pageSizeItems.OrderBy(x => x).ToList();

            foreach (int item in pageSizeItems)
                ddlPageSize.Items.Add(item.ToString());

            ddlPageSize.AutoPostBack = true;

            if (!string.IsNullOrEmpty(SelectedPageSize))
                PageSize = Int32.Parse(SelectedPageSize);

            ddlPageSize.SelectedValue = PageSize.ToString();
            arrayControles[0, 0] = ddlPageSize;
            this.Controls.Add(ddlPageSize);

            int totalMaxPage = CountDataSourceRows() / PageSize;
            if (CountDataSourceRows() % PageSize != 0)
                totalMaxPage++;

            arrayFooterControl = new Control[totalMaxPage + 2];

            LinkButton lnkNumber = null;

            for (int i = 1; i <= totalMaxPage; i++)
            {
                lnkNumber = new LinkButton();
                lnkNumber.ID = string.Concat("linkNumber", i);
                lnkNumber.Click += OnChangePageIndex;
                lnkNumber.DataField = i.ToString();
                lnkNumber.Text = i.ToString();
                lnkNumber.CssClass = "";
                this.Controls.Add(lnkNumber);
                arrayFooterControl[i - 1] = lnkNumber;
            }

            lnkNumber = new LinkButton();
            lnkNumber.ID = "lnkDown";
            lnkNumber.Click += OnChangePageIndex;
            lnkNumber.DataField = "lnkdown";
            lnkNumber.Text = "<<";
            lnkNumber.CssClass = "";
            this.Controls.Add(lnkNumber);
            arrayFooterControl[totalMaxPage] = lnkNumber;

            lnkNumber = new LinkButton();
            lnkNumber.ID = "lnkUp";
            lnkNumber.Click += OnChangePageIndex;
            lnkNumber.DataField = "lnkup";
            lnkNumber.Text = ">>";
            lnkNumber.CssClass = "";
            this.Controls.Add(lnkNumber);
            arrayFooterControl[totalMaxPage + 1] = lnkNumber;

        }
        private void SetDropDrowList()
        {
            if (!string.IsNullOrEmpty(SelectedPageSize))
                PageSize = Int32.Parse(SelectedPageSize);

            int totalMaxPage = ((List<object>)tbvDataSource).Count() / PageSize;
            if (((List<object>)tbvDataSource).Count() % PageSize != 0)
                totalMaxPage++;

            foreach (Control c in arrayFooterControl)
                this.Controls.Remove(c);

            arrayFooterControl = new Control[totalMaxPage + 2];
            LinkButton lnkNumber = null;

            //Page Numbers
            for (int i = 1; i <= totalMaxPage; i++)
            {
                lnkNumber = new LinkButton();
                lnkNumber.ID = string.Concat("linkNumber", i);
                lnkNumber.Click += OnChangePageIndex;
                lnkNumber.DataField = i.ToString();
                lnkNumber.Text = i.ToString();
                lnkNumber.CssClass = "";
                this.Controls.Add(lnkNumber);
                arrayFooterControl[i - 1] = lnkNumber;
            }

            lnkNumber = new LinkButton();
            lnkNumber.ID = "lnkDown";
            lnkNumber.Click += OnChangePageIndex;
            lnkNumber.DataField = "lnkdown";
            lnkNumber.Text = "<<";
            lnkNumber.CssClass = "";
            this.Controls.Add(lnkNumber);
            arrayFooterControl[totalMaxPage] = lnkNumber;

            lnkNumber = new LinkButton();
            lnkNumber.ID = "lnkUp";
            lnkNumber.Click += OnChangePageIndex;
            lnkNumber.DataField = "lnkup";
            lnkNumber.Text = ">>";
            lnkNumber.CssClass = "";
            this.Controls.Add(lnkNumber);
            arrayFooterControl[totalMaxPage + 1] = lnkNumber;
        }
        private void LoadHeader()
        {
            int column = 0;
            foreach (ITableColumn columnItem in ColumnsCollection)
            {
                if (!(columnItem is TableColumnCheck) && !(columnItem is TableColumnButton) && columnItem.Sortable)
                {
                    //Botão Sortable
                    ImageButton btnSort = new ImageButton();
                    btnSort.ID = "Ordena" + column.ToString("000");
                    btnSort.Click += OnSort;
                    btnSort.DataField = columnItem.DataField;
                    this.Controls.Add(btnSort);

                    arrayControles[1, column] = btnSort;
                }

                if (!(columnItem is TableColumnCheck) && !(columnItem is TableColumnButton) && columnItem.Filterable)
                {
                    //Textbox Filtro
                    TextBox txtFiltro = new TextBox();
                    txtFiltro.CampoObrigatorio = false;
                    txtFiltro.ID = "Filtro" + column.ToString("000");
                    txtFiltro.AutoPostBack = true;
                    txtFiltro.TextChanged += OnFilter;
                    txtFiltro.DataField = columnItem.DataField;
                    txtFiltro.DefaultValue = String.Concat("Procurar por ", columnItem.HeaderText);
                    txtFiltro.Attributes["placeholder"] = String.Concat("Procurar por ", columnItem.HeaderText);
                    this.Controls.Add(txtFiltro);

                    arrayControles[2, column] = txtFiltro;
                }

                if (columnItem is TableColumnCheck && ((TableColumnCheck)columnItem).HasCheckHeader)
                {
                    CheckBox chkFiltro = new CheckBox();
                    chkFiltro.ID = "Check" + column.ToString("000");
                    chkFiltro.DataField = columnItem.DataField;
                    chkFiltro.MontaGroup = false;
                    chkFiltro.CampoObrigatorio = false;
                    chkFiltro.Attributes["chkColumnName"] = ((TableColumnCheck)columnItem).ID;

                    if (Page.IsPostBack && Page.Request.Form[this.ID + "$" + chkFiltro.ID] != null)
                        chkFiltro.Checked = Page.Request.Form[this.ID + "$" + chkFiltro.ID].ToString().ToUpper().Equals("ON");

                    this.Controls.Add(chkFiltro);

                    arrayControles[2, column] = chkFiltro;
                }
                column++;
            }
        }
        private void RenderFooter(HtmlTextWriter output)
        {
            output.Write("<tfoot>");
            output.Write("<tr>");
            output.Write("<td colspan='{0}' align='right'>", (tbvDataSource is DataTable) ? (tbvDataSource as DataTable).Columns.Count : columnsCollection.Where(p => p.ColumnVisible).Count());
            output.Write("<div class='row-fluid'>");
            output.Write("<div class='col-sm-8 form-inline text-left'>");
            output.Write("<label class='marginright5'>Exibindo</label>");

            ((DropDownList)arrayControles[0, 0]).Style.Add("width", "auto");
            arrayControles[0, 0].RenderControl(output);

            output.Write($"<label class='marginleft5'>registros por página de um total de ");
            output.Write($"<label class='bold text-info fontsize14'>{dtFiltered.Rows.Count}</label>");
            output.Write($"<label style='margin-left: 3px;'>registros.</label>");
            output.Write("</div>");
            output.Write("<div class='col-sm-4 text-right'>");
            output.Write("<div class='tableview-paginator'>");
            output.Write("<ul class='pagination pagination-sm'>");

            int indexControlDown = this.Controls.IndexOf(((LinkButton)arrayFooterControl[arrayFooterControl.Length - 2]));
            LinkButton btnDown = ((LinkButton)this.Controls[indexControlDown]);

            if (arrayFooterControl.Length > 7 && FirstPageSet > firstPage)
            {
                output.Write("<li>");
                btnDown.RenderControl(output);
            }
            else
            {
                output.Write("<li class='disabled'>");
                output.Write(string.Format("<span>{0}<span class='sr-only'>(current)</span></span>", btnDown.Text));
            }

            output.Write("</li>");

            if (FirstPageSet == 0)
                FirstPageSet = firstPage;

            if (LastPageSet == 0)
            {
                if (lastPage > 5)
                    LastPageSet = firstPage + 4;
                else
                    LastPageSet = lastPage;
            }

            for (int i = FirstPageSet; i <= LastPageSet; i++)
            {
                int indexControl = this.Controls.IndexOf(((LinkButton)arrayFooterControl[i - 1]));
                LinkButton btn = ((LinkButton)this.Controls[indexControl]);

                output.Write(string.Format("<li class='{0}'>", PageIndex == i ? "active" : ""));
                if (PageIndex == i)
                    output.Write(string.Format("<span>{0} <span class='sr-only'>(current)</span></span>", i));
                else
                    btn.RenderControl(output);

                output.Write("</li>");
            }

            int indexControlUp = this.Controls.IndexOf(((LinkButton)arrayFooterControl[arrayFooterControl.Length - 1]));
            LinkButton btnUp = ((LinkButton)this.Controls[indexControlUp]);

            if (arrayFooterControl.Length > 7 && LastPageSet < lastPage)
            {
                output.Write("<li>");
                btnUp.RenderControl(output);
            }
            else
            {
                output.Write("<li class='disabled'>");
                output.Write(string.Format("<span>{0}<span class='sr-only'>(current)</span></span>", btnUp.Text));
            }

            output.Write("</li>");
            output.Write("</ul>");
            output.Write("</div>");
            output.Write("</div>");
            output.Write("</div>");
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("</tfoot>");
        }
        private void RenderHeader(HtmlTextWriter output)
        {
            //Render Headers
            output.Write("<thead>");
            int columnIndex = 0;

            var totalColumns = tbvDataSource is DataTable ? (tbvDataSource as DataTable).Columns.Count : ColumnsCollection.Count;

            if (!string.IsNullOrWhiteSpace(Title))
                output.Write($"<tr><th colspan='{totalColumns}'>{Title}</th></tr>");


            output.Write("<tr>");

            if (tbvDataSource is DataTable)
            {
                foreach (DataColumn item in (tbvDataSource as DataTable).Columns)
                {
                    if (item.ColumnName != indexColumn)
                        output.Write($"<th>{item.ColumnName}</th>");
                }
            }
            else
            {
                //Renderiza o Nome da Coluna e os Botões de Sort
                columnIndex = 0;
                foreach (ITableColumn columnItem in ColumnsCollection)
                {
                    if (columnItem.ColumnVisible)
                    {
                        var stylewidth = columnItem.Width > 0 ? $"style='width:{columnItem.Width}px'" : "";

                        output.Write($"<th {stylewidth} class='{((TableColumn)columnItem).HeaderAlign.GetStringValueText()}'>");

                        if (!string.IsNullOrWhiteSpace(columnItem.HeaderTooltip))
                            output.Write("<div data-toggle='tooltip' data-placement='bottom' title='{0}'>", columnItem.HeaderTooltip);

                        //CheckBox Header
                        if (columnItem is TableColumnCheck && ((TableColumnCheck)columnItem).HasCheckHeader)
                        {
                            Control ctrlHeader = arrayControles[2, columnIndex];
                            if (ctrlHeader != null)
                                ctrlHeader.RenderControl(output);
                        }

                        Control ctrl = arrayControles[1, columnIndex];
                        if (ctrl != null)
                        {
                            ((ImageButton)ctrl).Text = columnItem.HeaderText;

                            StringBuilder sb = new StringBuilder();
                            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
                            HtmlTextWriter wr = new HtmlTextWriter(sw);
                            ctrl.RenderControl(wr);

                            string component = sb.ToString();
                            int imgstart = component.IndexOf("<img src=");
                            int imgstop = component.IndexOf("/>", imgstart) + 2;
                            string cmp1 = component.Substring(0, imgstart);
                            string cmp2 = component.Substring(imgstop);
                            string sorting = "<i class='fas fa-sort'></i> ";

                            //Verifica se a coluna renderizada é a coluna ordenada e coloca a classe ASC ou DESC
                            if (columnItem.DataField == SortingColumn.Item1)
                            {
                                if (SortingColumn.Item2 == "ASC")
                                    sorting = "<i class='fas fa-sort-alpha-down'></i> ";
                                else
                                    sorting = "<i class='fas fa-sort-alpha-up'></i> ";
                            }

                            output.Write(string.Concat(cmp1, sorting, cmp2));
                        }
                        else
                            output.Write(columnItem.HeaderText);

                        if (!string.IsNullOrWhiteSpace(columnItem.HeaderTooltip))
                            output.Write("<i class='fas fa-question-circle marginLeft5'></i></div>");

                        output.Write("</th>");
                    }

                    columnIndex++;
                }
            }

            output.Write("</tr>");

            if (columnsCollection.Exists(p => p.Filterable && !(p is TableColumnButton) && !(p is TableColumnCheck)))
            {
                output.Write("<tr>");
                for (columnIndex = 0; columnIndex < columnsCollection.Count; columnIndex++)
                {
                    if (columnsCollection[columnIndex].ColumnVisible)
                    {
                        output.Write("<th>");

                        if (!(columnsCollection[columnIndex] is TableColumnCheck))
                        {
                            Control ctrl = arrayControles[2, columnIndex];

                            if (ctrl != null)
                                ctrl.RenderControl(output);
                        }

                        output.Write("</th>");
                    }
                }
                output.Write("</tr>");
            }

            output.Write("</thead>");
        }
        private DataTable ApplyFilters()
        {
            DataTable dtOriginal = this.DataSourceToDataTable();
            DataTable dtFiltered = dtOriginal.Clone();

            StringBuilder filterExpression = new StringBuilder();

            bool first = true;
            foreach (var item in this.Filters)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    if (!first)
                        filterExpression.Append(" AND ");
                    else
                        first = false;

                    Type colType = dtOriginal.Columns[item.Key].DataType;

                    if (colType == typeof(String) || colType == typeof(DateTime))
                    {
                        filterExpression.AppendFormat("({0} LIKE '%{1}%')", item.Key, item.Value);
                    }
                    else
                    {
                        filterExpression.AppendFormat("({0} = {1})", item.Key, item.Value);
                    }
                }
            }

            DataRow[] filteredRows = dtOriginal.Select(filterExpression.ToString());

            foreach (var row in filteredRows)
                dtFiltered.ImportRow(row);

            if (!String.IsNullOrWhiteSpace(SortingColumn.Item1))
            {
                dtFiltered.DefaultView.Sort = String.Format("{0} {1}", SortingColumn.Item1, SortingColumn.Item2);
                return dtFiltered.DefaultView.ToTable();
            }

            return dtFiltered;
        }
        private void LoadAuxValues()
        {

            if (!string.IsNullOrEmpty(SelectedPageSize))
                PageSize = Int32.Parse(SelectedPageSize);

            //Aplica os Filtros no DataSource
            dtFiltered = ApplyFilters();

            totalPage = dtFiltered.Rows.Count / PageSize;
            if (dtFiltered.Rows.Count % PageSize != 0)
                totalPage++;

            firstPage = 1;
            lastPage = totalPage;
        }
        private void BindControls()
        {
            //Carrega as variaveis auxiliares utilizadas
            LoadAuxValues();

            IDs_TableColumnInput.Clear();

            //Adiciona os controles ao componente gridview (necesário para a chamada event handler)
            arrayControles = new Control[CountDataSourceRows() + 3, CountDataSourceColumns()];

            //Carrega os componentes do Header
            LoadHeader();

            DataTable dtOriginal = this.DataSourceToDataTable();

            int totalRows = PageIndex * PageSize > dtOriginal.Rows.Count ? dtOriginal.Rows.Count : (PageIndex * PageSize);
            int initRow = ((PageIndex - 1) * PageSize);

            //Carrega os componentes do Body
            int row = 3, column = 0;
            foreach (DataRow rowItem in dtOriginal.Rows)
            {
                column = 0;
                foreach (ITableColumn columnItem in ColumnsCollection)
                {
                    if (columnItem is TableColumnCheck && !String.IsNullOrEmpty(((TableColumnCheck)columnItem).DataChecked))
                        ((TableColumnCheck)columnItem).DataCheckedValue = rowItem[((TableColumnCheck)columnItem).DataChecked].ToString();
                    else if (columnItem is TableColumnButton && !String.IsNullOrEmpty(((TableColumnButton)columnItem).DataEnabled))
                        ((TableColumnButton)columnItem).DataEnabledValue = rowItem[((TableColumnButton)columnItem).DataEnabled].ToString();

                    var dataSourceElement = ((List<object>)tbvDataSource).ElementAt(rowItem["UniqueObjectID"].ToString().ToInt32());
                    Control ctrl = columnItem.OnLoad(rowItem, dataSourceElement);
                    ctrl.ID = string.Format("{0}{1}{2}{3}", this.ID, ctrl.ID, row, column);

                    if (!(columnItem is TableColumnText))
                        this.Controls.Add(ctrl);

                    //Se é um buttom grava o indice da linha no buttom
                    if (columnItem is TableColumnButton)
                    {
                        if ((columnItem as TableColumnButton).RegisterPostBack)
                            ScriptManager.GetCurrent(Page).RegisterPostBackControl(((Button)ctrl));

                        ((Button)ctrl).TableColumnButtonRowIndex = (row - 3);
                    }

                    else if (columnItem is TableColumnCheck)
                    {


                        //ARISTEU.20/11/2015 - ESTE BLOCO LIMPA OS ATUAIS IDS MARCADOS COMO CHECADOS, CASO TENHA SE EXECUTADO O MÉTODO SETDATASOURCE
                        if (LimparIDs)
                        {
                            CheckboxFieldsIDCheckeds.RemoveAll(x => x == this.ClientID.Replace("_", "$") + "$" + ctrl.ID);
                        }


                        //ARISTEU.20/11/2015 - ESTE BLOCO ALIMENTA A COLEÇÃO DE IDS CHECADOS COM BASE NO DATASOURCE, EXECUTADO APENAS SE O MÉTODO SETDATASOURCE FOI DISPARADO
                        if (LimparIDs && resetControls.HasValue && resetControls.Value && !String.IsNullOrWhiteSpace(((TableColumnCheck)columnItem).DataChecked))
                        {
                            if (rowItem[((TableColumnCheck)columnItem).DataChecked].ToString().ToBoolean())
                                CheckboxFieldsIDCheckeds.Add(this.ClientID.Replace("_", "$") + "$" + ctrl.ID);
                            else
                                CheckboxFieldsIDCheckeds.RemoveAll(x => x == this.ClientID.Replace("_", "$") + "$" + ctrl.ID);
                        }

                        //ARISTEU.20/11/2015 - CASO SETDATASOURCE NÃO TENHA SIDO EXECUTADO, VERIFICA SE TEM ALGUM ID CHECADO DO POSTABACK DA PÁGINA
                        else if (!LimparIDs)
                        {
                            if (Page.Request.Form.AllKeys.FirstOrDefault(p => p != null && p.Equals(this.ClientID.Replace("_", "$") + "$" + ctrl.ID)) != null && !CheckboxFieldsIDCheckeds.Contains(this.ClientID.Replace("_", "$") + "$" + ctrl.ID))
                            {
                                CheckboxFieldsIDCheckeds.Add(this.ClientID.Replace("_", "$") + "$" + ctrl.ID);

                                //Altera o DataSource
                                if (!String.IsNullOrWhiteSpace(((TableColumnCheck)columnItem).DataChecked))
                                    ObjectHelper.SetPropertyValue(dataSourceElement, ((TableColumnCheck)columnItem).DataChecked, true.ToString());

                            }
                            else if (row - 3 >= initRow && row - 3 < totalRows && Page.Request.Form.AllKeys.FirstOrDefault(p => p != null && p.Equals(this.ClientID.Replace("_", "$") + "$" + ctrl.ID)) == null && CheckboxFieldsIDCheckeds.Contains(this.ClientID.Replace("_", "$") + "$" + ctrl.ID))
                            {
                                CheckboxFieldsIDCheckeds.RemoveAll(x => x == this.ClientID.Replace("_", "$") + "$" + ctrl.ID);

                                //Altera o DataSource
                                if (!String.IsNullOrWhiteSpace(((TableColumnCheck)columnItem).DataChecked))
                                    ObjectHelper.SetPropertyValue(dataSourceElement, ((TableColumnCheck)columnItem).DataChecked, false.ToString());
                            }
                        }
                    }
                    else if (columnItem is TableColumnInput)
                    {
                        ((TextBox)ctrl).DataFieldName = (columnItem as TableColumnInput).DataTextField;

                        if ((!resetControls.HasValue || !resetControls.Value) && Page.IsPostBack && Page.Request.Form[ctrl.UniqueID] != null)
                        {
                            ((TextBox)ctrl).Text = Page.Request.Form[ctrl.UniqueID].ToString();

                        }

                        if (Page.Request.Form[ctrl.UniqueID] != null && !String.IsNullOrWhiteSpace((columnItem as TableColumnInput).DataTextField))
                        {
                            string value = Page.Request.Form[ctrl.UniqueID].ToString();
                            if ((columnItem as TableColumnInput).Mask == TextBox.MaskTextBox.Decimal && string.IsNullOrWhiteSpace(value))
                                value = 0.ToString();

                            ObjectHelper.SetPropertyValue(dataSourceElement, (columnItem as TableColumnInput).DataTextField, value);


                        }
                    }
                    else if (columnItem is TableColumnFiles)
                    {
                        foreach (var item in ctrl.GetAllChildren().OfType<Button>().ToList())
                        {
                            if ((!resetControls.HasValue || resetControls.Value) && Page.IsPostBack)
                                RegisterPostBack(item);

                        }

                    }

                    //Verifica se o controle ficará visivel ou não, baseado na propriedade do datasource
                    if (!String.IsNullOrWhiteSpace(columnItem.VisibleField))
                        ctrl.Visible = Boolean.Parse(rowItem[columnItem.VisibleField].ToString());

                    String aux;
                    arrayControles[row, column] = ctrl;


                    if (columnItem is TableColumnInput)
                    {
                        TableColumnInput controle = (TableColumnInput)columnItem;

                        if (controle.EventoAssincrono)
                        {
                            aux = ((TextBox)ctrl).ClientID;

                            IDs_TableColumnInput.Add(aux);
                        }

                    }



                    column++;
                }
                row++;
            }

            LimparIDs = false;

            //Carrega os componentes do Footer
            if (showfooter)
                LoadFooter();
        }

        private int CountDataSourceRows()
        {
            if (tbvDataSource is DataTable)
                return (tbvDataSource as DataTable).Rows.Count;
            else
                return ((List<object>)tbvDataSource).Count();
        }
        private int CountDataSourceColumns()
        {
            if (tbvDataSource is DataTable)
                return (tbvDataSource as DataTable).Columns.Count;
            else
                return ColumnsCollection.Count;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            indexColumn = this.ID + "_index";

            AtualizaDataSource_TableColumnInput();
        }
        protected override void OnLoad(EventArgs e)
        {
            if (Page.IsPostBack)
            {

                if (tbvDataSource is DataTable)
                    SetDataSource(tbvDataSource as DataTable);
                else
                    SetDataSource(((List<object>)tbvDataSource), false);

                DataBind();
            }
            else if (arrayControles == null)
                DataBind();
            else
                RegisterPostBack();
        }
        protected override void Render(HtmlTextWriter output)
        {

            if (keeppage)
            {
                PageIndex = PageIndexOld;
                keeppage = false;

            }
            else
            {
                PageIndexOld = PageIndex;
            }

            if (PageIndex==1)
            {
                FirstPageSet = 1;
            }

            output.Write("<table cellspacing='0' border='0' class='TableView table{0}{1}{2}{3}{4}' ID='{5}'>",
                tablestriped ? " table-striped" : string.Empty,
                tablebordered ? " table-bordered" : string.Empty,
                tablehover ? " table-hover" : string.Empty,
                tablecondensed ? " table-condensed" : string.Empty,
                " " + CssClass,
                this.ID);

            RenderHeader(output);

            Dictionary<string, string> groupKeysProperties = null;
            string[] arrayGroupKeysProperties = string.IsNullOrWhiteSpace(GroupKeysProperties) ? null : GroupKeysProperties.Split(',');
            if (arrayGroupKeysProperties != null && arrayGroupKeysProperties.Length > 0)
            {
                groupKeysProperties = new Dictionary<string, string>();
                foreach (var item in arrayGroupKeysProperties)
                    groupKeysProperties.Add(item, "");

                string sortExpression = string.Join(",", arrayGroupKeysProperties);
                dtFiltered.DefaultView.Sort = sortExpression;

                dtFiltered = dtFiltered.DefaultView.ToTable();
            }

            output.Write("<tbody>");
            if (dtFiltered != null && dtFiltered.Rows.Count > 0)
            {
                // Render Columns.    
                int row = 0, column = 0;
                int totalRows = PageIndex * PageSize > dtFiltered.Rows.Count ? dtFiltered.Rows.Count : (PageIndex * PageSize);

                for (int i = ((PageIndex - 1) * PageSize); i < totalRows; i++)
                {
                    row = Int32.Parse(dtFiltered.Rows[i][indexColumn].ToString());

                    var cssrow = "";
                    if (!String.IsNullOrWhiteSpace(RowCssClassDataField))
                        cssrow = dtFiltered.Rows[i]["RowCssClassDataField"].ToString();

                    if (!String.IsNullOrWhiteSpace(RowTooltipField))
                        output.Write("<tr data-toggle='tooltip' data-placement='bottom' data-container='body' title='{0}'>", HttpUtility.HtmlEncode(dtFiltered.Rows[i]["RowTooltipField"].ToString()));
                    else
                        output.Write("<tr>");

                    column = 0;

                    if (tbvDataSource is DataTable)
                    {
                        foreach (DataColumn item in (tbvDataSource as DataTable).Columns)
                        {
                            if (item.ColumnName != indexColumn)
                                output.Write($"<td>{(tbvDataSource as DataTable).Rows[row][item.ColumnName]}</td>");
                        }
                    }
                    else
                    {
                        foreach (TableColumn columnItem in ColumnsCollection)
                        {
                            if (columnItem.ColumnVisible)
                            {
                                Control ctrl = arrayControles[row + 3, column];

                                string rowSpan = "";
                                if (columnItem is TableColumnText && groupKeysProperties != null && groupKeysProperties.ContainsKey(columnItem.DataField))
                                {
                                    string valueGroupKey = groupKeysProperties[columnItem.DataField];
                                    if (valueGroupKey != (ctrl.Controls[0] as Label).Text)
                                    {
                                        groupKeysProperties[columnItem.DataField] = (ctrl.Controls[0] as Label).Text;

                                        var keys = groupKeysProperties.Keys.ToList();
                                        var indexOFKey = keys.IndexOf(columnItem.DataField);

                                        List<string> selectDT = new List<string>();
                                        for (int auxIndex = 0; auxIndex < keys.Count; auxIndex++)
                                        {
                                            if (auxIndex <= indexOFKey)
                                                selectDT.Add($"{ keys[auxIndex]} = '{groupKeysProperties[keys[auxIndex]]}'");
                                            else
                                                groupKeysProperties[keys[auxIndex]] = "";
                                        }


                                        var countRowSpan = dtFiltered.Select(string.Join(" AND ", selectDT)).Length;
                                        rowSpan = $"rowspan='{countRowSpan}'";
                                    }
                                    else
                                    {
                                        column++;
                                        continue;
                                    }
                                }
                                else
                                    rowSpan = "";

                                var cssdatafield = "";
                                if (!String.IsNullOrWhiteSpace(columnItem.CssClassDataField))
                                    cssdatafield = dtFiltered.Rows[i][column + "CssClassDataField"].ToString();

                                if (columnItem.Align != TableColumn.HorizontalAlign.None && !columnItem.CssClass.Contains(columnItem.Align.GetStringValueText()))
                                    columnItem.CssClass += $" {columnItem.Align.GetStringValueText()}";

                                string style = "";
                                if (columnItem is TableColumnText && !string.IsNullOrWhiteSpace((columnItem as TableColumnText).BackgroudColorProperty))
                                    style = $"style='background-color: {dtFiltered.Rows[i][(columnItem as TableColumnText).BackgroudColorProperty]}'";

                                //Se existe evento e as colunas não são buttons e checks, cria a linha com os argumentos passados na Propriedade DataKeys
                                if (this.RowClick != null && this.RowClick.GetInvocationList().Length > 0 &&
                                    !(columnItem is TableColumnCheck || columnItem is TableColumnButton || columnItem is TableColumnInput))
                                {
                                    string[] keys = DataKeys.Split(new char[] { ';' });
                                    string[] values = new string[keys.Length];

                                    for (int j = 0; j < keys.Length; j++)
                                    {
                                        int indexDataSourceOriginal = dtFiltered.Rows[i]["UniqueObjectID"].ToString().ToInt32();
                                        values[j] = ObjectHelper.GetPropertyValue(((List<object>)tbvDataSource).ElementAt(indexDataSourceOriginal), keys[j]).ToString();
                                    }

                                    if (DisableRowClick == false)
                                    {
                                        output.Write($"<td {rowSpan} class='clickable {columnItem.CssClass} {cssrow} {cssdatafield}' onclick=\"{Page.ClientScript.GetPostBackEventReference(this, String.Join(";", values) + "*" + column)}\">");
                                    }
                                    else
                                    {
                                        output.Write($"<td {rowSpan} class='clickable {columnItem.CssClass} {cssrow} {cssdatafield}'\">");
                                    }
                                }
                                else
                                    output.Write($"<td {rowSpan} class='{columnItem.CssClass} {cssrow} {cssdatafield} '>");



                                if (columnItem is TableColumnCheck)
                                {
                                    ((CheckBox)ctrl).Checked = CheckboxFieldsIDCheckeds.Contains(this.ClientID.Replace("_", "$") + "$" + ctrl.ID);

                                    //if ((!resetControls.HasValue || !resetControls.Value) && Page.IsPostBack)
                                    //{
                                    //    if (!(Page.Request.Form.AllKeys.FirstOrDefault(p => p != null && p.Contains(ctrl.ID)) != null) && CheckboxFieldsIDCheckeds.Contains(this.ID + "$" + ctrl.ID))                                    
                                    //        CheckboxFieldsIDCheckeds.Remove(this.ID + "$" + ctrl.ID);
                                    //}                            
                                }

                                ctrl.RenderControl(output);

                                output.Write("</td>");
                            }

                            column++;
                        }
                    }
                    output.Write("</tr>");
                }
            }
            else
            {
                output.Write("<tr class='info'>");
                output.Write("<td style='text-align:center;font-weight:bold;' colspan='" + ColumnsCollection.Count + "'>Sem dados disponíveis</td>");
                output.Write("</tr>");
            }

            output.Write("</tbody>");

            if (showfooter)
                RenderFooter(output);

            output.Write("</table>");


        }
        public override void DataBind()
        {
            base.OnDataBinding(EventArgs.Empty);

            //11-08-2017 Alterei aqui pois não limpava os filtros, se der erro pra alguem, me avisa que criamos um método (estou mechendo no importar da previa no associado)
            Filters.Clear();         
            PageIndex = 1;
            
            Controls.Clear();

            BindControls();

            //Grava no datasource o objeto com as propriedades aninhadas carregadas
            if (!(tbvDataSource is DataTable))
            {
                IEnumerable<Object> a = ((List<object>)tbvDataSource);
                tbvDataSource = null;
                if (a != null)
                    tbvDataSource = a.Select(p => p).ToList();
            }
        }
        private void RegisterPostBack(Control ctrl = null)
        {
            if (ctrl == null)
            {
                var fileColumns = ColumnsCollection.Where(p => p is TableColumnFiles);
                if (fileColumns != null && fileColumns.Count() > 0)
                {
                    DataTable dtOriginal = DataSourceToDataTable();
                    int row = 3;
                    foreach (DataRow rowItem in dtOriginal.Rows)
                    {
                        foreach (ITableColumn columnItem in fileColumns)
                        {
                            ctrl = arrayControles[row, ColumnsCollection.IndexOf(columnItem as TableColumn)];
                            foreach (var item in ctrl.GetAllChildren().OfType<Button>().ToList())
                                RegisterPostBack(item);
                        }
                        row++;
                    }
                }
            }
            else
            {
                //Adiciona a Trigger ao UpdatePanel Associciado
                PostBackTrigger trigger = new PostBackTrigger();
                trigger.ControlID = ctrl.ClientID;

                UpdatePanel upp = (UpdatePanel)ctrl.GetParentOfType(typeof(UpdatePanel));
                upp.Triggers.Add(trigger);
                ScriptManager.GetCurrent(Page).RegisterPostBackControl(ctrl);
            }
        }

        protected virtual void OnFilter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            Dictionary<String, String> filt = Filters;

            //Verifica se ja existe algum filtro da coluna e remove
            if (filt.ContainsKey(txt.DataField))
                filt.Remove(txt.DataField);

            //Se o usuario filtrou, adiciona o filtro
            if (txt.Text != txt.DefaultValue)
                filt.Add(txt.DataField, txt.Text);

            Filters = filt;

            LoadAuxValues();
        }
        protected virtual void OnSort(object sender, EventArgs e)
        {
            Tuple<String, String> sort = SortingColumn;

            ImageButton btn = sender as ImageButton;
            if (sort.Item1.Equals(btn.DataField))
            {
                if (sort.Item2.Equals("ASC"))
                    sort = new Tuple<String, String>(btn.DataField, "DESC");
                else
                    sort = new Tuple<String, String>(btn.DataField, "ASC");
            }
            else
                sort = new Tuple<String, String>(btn.DataField, "ASC");

            SortingColumn = sort;

            LoadAuxValues();
        }
        protected virtual void OnChangePageSize(object sender, EventArgs e)
        {
            FirstPageSet = 0;
            LastPageSet = 0;
            PageIndex = 1;
            PageIndexOld = 1;

            DropDownList ddl = sender as DropDownList;
            SelectedPageSize = ddl.SelectedValue;
            SetDropDrowList();
            LoadAuxValues();
        }
        protected virtual void OnChangePageIndex(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = sender as LinkButton;

                if (btn.DataField == "lnkup")
                {
                    keeppage = true;

                    if (FirstPageSet == 0)
                        FirstPageSet = 2;
                    else
                        FirstPageSet++;

                    LastPageSet = FirstPageSet + 4;
                }
                else if (btn.DataField == "lnkdown")
                {
                    keeppage = true;

                    FirstPageSet--;
                    LastPageSet = FirstPageSet + 4;
                }
                else
                {
                    PageIndex = Int32.Parse(btn.DataField);
                    PageIndexOld = PageIndex;
                    keeppage = false;
                }

                LoadAuxValues();
            }
            catch (Exception)
            {
            }
        }

        public void SetDataSource(IEnumerable<Object> DataSource)
        {
            SetDataSource(DataSource, true);
        }
        public void SetDataSource(DataTable DataSource)
        {
            this.tbvDataSource = DataSource;
        }

        private void SetDataSource(IEnumerable<Object> DataSource, bool externo = false)
        {

            resetControls = true;

            if (externo) LimparIDs = true;

            if (DataSource != null)
            {
                isExpandoObject = DataSource.Count() > 0 && DataSource.First() is System.Dynamic.ExpandoObject;
                this.tbvDataSource = DataSource.Select(p => p).ToList();
            }
            else
                this.tbvDataSource = null;

            dtFiltered = ApplyFilters();
        }

        /// <summary>
        /// Método utilizado para retornar um controle dentro do TableView
        /// </summary>
        /// <param name="row">indice da linha desejada</param>
        /// <param name="name">nome do controle desejado</param>
        /// <returns>Objeto com o controle desejado, retornando NULL quando o mesmo não for encontrado</returns>
        public Control GetControl(int row, string name)
        {
            int size = arrayControles.Length;
            Control targetControl = null;
            for (int i = 0; i < size; i++)
            {
                if (arrayControles[row + 3, i].ID.Contains(name))
                {
                    targetControl = arrayControles[row + 3, i];
                    break;
                }
            }
            return targetControl;
        }

        private DataTable DataSourceToDataTable()
        {
            try
            {
                //Para TableViews Dinamicos Criados em tempo de execução
                if (indexColumn == null)
                    indexColumn = this.ID + "_index";

                if (tbvDataSource is DataTable)
                {
                    DataTable dt = (tbvDataSource as DataTable).Copy();
                    dt.Columns.Add(indexColumn);

                    int rowindex = 0;
                    foreach (DataRow item in dt.Rows)
                    {
                        item[indexColumn] = rowindex;
                        rowindex++;
                    }

                    return dt;
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Object");
                    dt.Columns.Add("UniqueObjectID");
                    dt.Columns.Add(indexColumn);

                    if (!String.IsNullOrWhiteSpace(RowCssClassDataField))
                        dt.Columns.Add("RowCssClassDataField");

                    if (!String.IsNullOrWhiteSpace(RowTooltipField))
                        dt.Columns.Add("RowTooltipField");

                    if (tbvDataSource is DataTable || (AutoGenerateColumns && ((List<object>)tbvDataSource).Count > 0 && columnsCollection.Count == 0))
                    {
                        if (tbvDataSource is DataTable)
                        {

                        }
                        else
                        {
                            foreach (var item in ((List<object>)tbvDataSource).First().GetType().GetProperties())
                                columnsCollection.Add(new TableColumnText() { DataField = item.Name, HeaderText = item.Name });
                        }
                    }

                    //Adiciona as colunas no DataTable
                    int countIndexColumn = 0;
                    foreach (var column in columnsCollection)
                    {
                        if (!String.IsNullOrWhiteSpace(column.CssClassDataField))
                            dt.Columns.Add(countIndexColumn + "CssClassDataField");

                        if ((!String.IsNullOrWhiteSpace(column.DataField) && !dt.Columns.Contains(column.DataField)))
                        {
                            if (column is TableColumnText)
                            {
                                String strType = ((TableColumnText)column).DataType.ToString();

                                Type type = Type.GetType(String.Format("System.{0}", strType == "Date" || strType == "Hour" ? "DateTime" : strType));
                                dt.Columns.Add(column.DataField, type);
                            }
                            else
                            {
                                foreach (var item in column.DataField.Split(';'))
                                    dt.Columns.Add(item);
                            }
                        }

                        if (column is TableColumnText)
                        {
                            TableColumnText text = (TableColumnText)column;
                            if (!String.IsNullOrWhiteSpace(text.BackgroudColorProperty))
                                dt.Columns.Add(text.BackgroudColorProperty);
                        }
                        else if (column is TableColumnCheck)
                        {
                            TableColumnCheck check = (TableColumnCheck)column;
                            if (!String.IsNullOrWhiteSpace(check.DataChecked) && !dt.Columns.Contains(check.DataChecked))
                            {
                                dt.Columns.Add(check.DataChecked);
                            }
                        }
                        else if (column is TableColumnButton)
                        {
                            TableColumnButton button = (TableColumnButton)column;
                            if (!String.IsNullOrWhiteSpace(button.DataEnabled) && !dt.Columns.Contains(button.DataEnabled))
                            {
                                dt.Columns.Add(button.DataEnabled);
                            }
                        }
                        else if (column is TableColumnInput)
                        {
                            TableColumnInput input = (TableColumnInput)column;
                            if (!String.IsNullOrWhiteSpace(input.DataTextField) && !dt.Columns.Contains(input.DataTextField))
                            {
                                dt.Columns.Add(input.DataTextField);
                            }
                        }

                        if ((!String.IsNullOrWhiteSpace(column.VisibleField) && !dt.Columns.Contains(column.VisibleField)))
                        {
                            if (column is TableColumnText)
                            {
                                Type type = Type.GetType(String.Format("System.{0}", ((TableColumnText)column).DataType.ToString()));
                                dt.Columns.Add(column.VisibleField, type);
                            }
                            else
                                dt.Columns.Add(column.VisibleField);
                        }
                        countIndexColumn++;
                    }

                    if (tbvDataSource != null)
                    {
                        //Preenche o DataTable com os valores
                        DataRow dr;
                        int rowIndex = 0;
                        for (int i = 0; i < CountDataSourceRows(); i++)
                        {
                            var item = ((List<object>)tbvDataSource).ElementAt(i);

                            bool rowvisible = true;
                            if (!String.IsNullOrWhiteSpace(this.RowVisibleField))
                            {
                                string propertyValue = ObjectHelper.GetPropertyValue(item, this.RowVisibleField).ToString();
                                if (!String.IsNullOrWhiteSpace(propertyValue))
                                    rowvisible = Boolean.Parse(propertyValue);
                            }

                            if (HideRemovedStatus)
                            {
                                string propertyValue = ObjectHelper.GetPropertyValue(item, "Status").ToString();
                                if (!String.IsNullOrWhiteSpace(propertyValue) && propertyValue == Enums.enumStatus.Remover.ToString())
                                    rowvisible = false;
                            }

                            if (rowvisible)
                            {
                                dr = dt.NewRow();
                                dr["Object"] = item;
                                dr["UniqueObjectID"] = i.ToString();
                                dr[indexColumn] = rowIndex.ToString();

                                if (!String.IsNullOrWhiteSpace(RowCssClassDataField))
                                {
                                    string propertyValue = ObjectHelper.GetPropertyValue(item, RowCssClassDataField).ToString();

                                    if (!String.IsNullOrWhiteSpace(propertyValue))
                                        dr["RowCssClassDataField"] = propertyValue;
                                }

                                if (!String.IsNullOrWhiteSpace(RowTooltipField))
                                {
                                    string propertyValue = ObjectHelper.GetPropertyValue(item, RowTooltipField).ToString();

                                    if (!String.IsNullOrWhiteSpace(propertyValue))
                                        dr["RowTooltipField"] = propertyValue;
                                }

                                int countColumnIndex = 0;
                                foreach (var column in columnsCollection)
                                {
                                    if (!String.IsNullOrWhiteSpace(column.CssClassDataField))
                                    {
                                        string propertyValue = ObjectHelper.GetPropertyValue(item, column.CssClassDataField).ToString();

                                        if (!String.IsNullOrWhiteSpace(propertyValue))
                                            dr[countColumnIndex + "CssClassDataField"] = propertyValue;
                                    }

                                    //Verifica se o nome da coluna não é vazio e se o Valor da Coluna não é vazio (para não haver erros de conversão)
                                    if (!String.IsNullOrWhiteSpace(column.DataField))
                                    {
                                        foreach (var dataFieldSplit in column.DataField.Split(';'))
                                        {
                                            string propertyValue = ObjectHelper.GetPropertyValue(item, dataFieldSplit).ToString();
                                            //string propertyValue = ObjectHelper.GetPropertyValueObj(item, column.DataField).ToString();

                                            if (column.GetType().Name == "TableColumnText")
                                            {
                                                if ((column as TableColumnText).DataType == TableColumnText.TypeColumn.Decimal)
                                                {
                                                    decimal valor = 0;
                                                    decimal.TryParse(propertyValue, out valor);
                                                    dr[dataFieldSplit] = valor;
                                                }

                                                else if ((column as TableColumnText).DataType == TableColumnText.TypeColumn.Date || (column as TableColumnText).DataType == TableColumnText.TypeColumn.DateTime)
                                                {
                                                    DateTime ldatetime = DateTime.MinValue;
                                                    DateTime.TryParse(propertyValue, out ldatetime);
                                                    dr[dataFieldSplit] = ldatetime;
                                                }
                                                else if (((column as TableColumnText).DataType == TableColumnText.TypeColumn.Boolean))
                                                {
                                                    Boolean bln = false;
                                                    Boolean.TryParse(propertyValue, out bln);
                                                    dr[dataFieldSplit] = bln;
                                                }
                                                else
                                                {
                                                    dr[dataFieldSplit] = propertyValue;
                                                }
                                            }
                                            else
                                            {
                                                if (!String.IsNullOrWhiteSpace(propertyValue))
                                                    dr[dataFieldSplit] = propertyValue;
                                            }
                                        }
                                    }

                                    // Se for um checkbox verifica se o mesmo tem a propriedade DataChecked preenchida
                                    if (column is TableColumnText)
                                    {
                                        TableColumnText text = (TableColumnText)column;
                                        if (!String.IsNullOrWhiteSpace(text.BackgroudColorProperty))
                                        {
                                            string propertyValue = ObjectHelper.GetPropertyValue(item, text.BackgroudColorProperty).ToString();

                                            if (!String.IsNullOrWhiteSpace(propertyValue))
                                                dr[text.BackgroudColorProperty] = propertyValue;
                                        }
                                    }// Se for um button verifica se o mesmo tem a propriedade DataEnabled preenchida
                                    else if (column is TableColumnCheck)
                                    {
                                        TableColumnCheck check = (TableColumnCheck)column;
                                        if (!String.IsNullOrWhiteSpace(check.DataChecked))
                                        {
                                            string propertyValue = ObjectHelper.GetPropertyValue(item, check.DataChecked).ToString();

                                            if (!String.IsNullOrWhiteSpace(propertyValue))
                                                dr[check.DataChecked] = propertyValue;
                                        }
                                    }// Se for um button verifica se o mesmo tem a propriedade DataEnabled preenchida
                                    else if (column is TableColumnButton)
                                    {
                                        TableColumnButton button = (TableColumnButton)column;
                                        if (!String.IsNullOrWhiteSpace(button.DataEnabled))
                                        {
                                            string propertyValue = ObjectHelper.GetPropertyValue(item, button.DataEnabled).ToString();

                                            if (!String.IsNullOrWhiteSpace(propertyValue))
                                                dr[button.DataEnabled] = propertyValue;
                                        }
                                    }
                                    else if (column is TableColumnInput)
                                    {
                                        TableColumnInput input = (TableColumnInput)column;
                                        if (!String.IsNullOrWhiteSpace(input.DataTextField))
                                        {
                                            string propertyValue = string.Empty;
                                            if (input.Mask == TextBox.MaskTextBox.Decimal)
                                            {
                                                decimal value = ObjectHelper.GetPropertyValue(item, input.DataTextField).ToString().ToDecimal();
                                                if (value < 0)
                                                    propertyValue = 0.ToString("0.00");
                                                else
                                                    propertyValue = value.ToString("0.00");
                                            }
                                            else
                                                propertyValue = ObjectHelper.GetPropertyValue(item, input.DataTextField).ToString();

                                            if (!String.IsNullOrWhiteSpace(propertyValue))
                                                dr[input.DataTextField] = propertyValue;
                                        }
                                    }

                                    if (!String.IsNullOrWhiteSpace(column.VisibleField))
                                    {
                                        string propertyValue = ObjectHelper.GetPropertyValue(item, column.VisibleField).ToString();

                                        if (!String.IsNullOrWhiteSpace(propertyValue))
                                            dr[column.VisibleField] = propertyValue;
                                    }
                                    countColumnIndex++;
                                }

                                dt.Rows.Add(dr);
                                rowIndex++;
                            }
                        }
                    }

                    return dt;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Dictionary<string, string>> GetSelectedKeys()
        {
            string[] keys = this.DataKeys.Split(new char[] { ';' });
            List<Dictionary<string, string>> values = new List<Dictionary<string, string>>();
            DataTable dtOriginal = this.DataSourceToDataTable();

            //int column = 0;
            for (int row = 0; row < dtOriginal.Rows.Count; row++)
            {
                if (ColumnsCollection[0] is TableColumnCheck)
                {
                    CheckBox ctrl = (CheckBox)arrayControles[row + 3, 0];
                    if (CheckboxFieldsIDCheckeds.Contains(this.ClientID.Replace("_", "$") + "$" + ctrl.ID))
                    {
                        Dictionary<string, string> entry = new Dictionary<string, string>();
                        for (int i = 0; i < keys.Length; i++)
                        {
                            entry.Add(keys[i], dtOriginal.Rows[row][keys[i]].ToString());
                        }

                        values.Add(entry);
                    }
                }
            }

            return values;
        }
        /// <summary>
        /// Retorna todos os valores bindados a propriedade DataField
        /// dos campos checkbox
        /// </summary>
        /// <param name="index">indice da coluna de checkbox</param>
        /// <returns>Lista com valores dos checkbox marcados</returns>
        public List<String> GetSelectedValues(int index)
        {
            List<String> values = new List<String>();
            DataTable dtOriginal = this.DataSourceToDataTable();

            //int column = 0;
            for (int row = 0; row < dtOriginal.Rows.Count; row++)
            {
                if (ColumnsCollection[index] is TableColumnCheck)
                {
                    CheckBox ctrl = (CheckBox)arrayControles[row + 3, 0];
                    if (CheckboxFieldsIDCheckeds.Contains(this.ClientID.Replace("_", "$") + "$" + ctrl.ID))
                        values.Add(dtOriginal.Rows[row][ctrl.DataField].ToString());
                }
                else
                    throw new CelebreException("O Indíce específicado não é de uma coluna de checks.");
            }

            return values;
        }

        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/Controls/TableView.js") + "'></script>"));

            foreach (var column in this.columnsCollection.DistinctBy(p => p.GetType().ToString()))
            {
                if (column is TableColumnInput)
                {
                    var campo = new TextBox() { Mask = (column as TableColumnInput).Mask };
                    listScriptsAndStyles.AddRange(campo.GetScriptsAndStyles());
                }
            }


            return listScriptsAndStyles;
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            RowClick?.Invoke(this, new RowClickEventArgs(eventArgument));
        }

        public string GetDataSourceJson
        {
            get
            {
                StringWriter stringwriter = new StringWriter();
                HtmlTextWriter html = new HtmlTextWriter(stringwriter);
                Render(html);
                return html.InnerWriter.ToString();
            }
        }

        public void Reset()
        {
            SetDataSource(null, true);
            DataBind();
        }

        public void AtualizaDataSource_TableColumnInput()
        {

            for (int i = 0; i < IDs_TableColumnInput.Count; i++)
            {

                var ctrl = IDs_TableColumnInput[i];

                var controls = this.Controls;

                Celebre.Web.Controls.TextBox controle_encontrado = (Celebre.Web.Controls.TextBox)procurar_controle(this.Page, ctrl);

                var item = ((List<object>)tbvDataSource).ElementAt(i);

                string key = DataKeys.Split(new char[] { ';' })[0];

                decimal setvalue = 0;
                if (controle_encontrado.Text != "")
                {
                    setvalue = decimal.Parse(controle_encontrado.Text);
                }
                item.GetType().GetProperty(controle_encontrado.DataFieldName).SetValue(item, setvalue);

            }

        }


        private System.Web.UI.Control procurar_controle(System.Web.UI.Control controle, string id)
        {
            if (controle.ClientID == id)
            {
                return controle;
            }
            else
            {
                int cont = 0;
                foreach (System.Web.UI.Control item in controle.Controls)
                {

                    var controle_encontrado = procurar_controle(item, id);

                    if (controle_encontrado != null)
                    {
                        return controle_encontrado;
                    }
                    cont++;
                }

                return null;
            }
        }





    }

    public interface ITableColumn
    {
        String HeaderText { get; set; }
        Boolean Sortable { get; set; }
        Boolean Filterable { get; set; }
        String DataField { get; set; }

        /// <summary>
        /// Propriedade Boolean do datasource que diz se a coluna ficará visivel ou não
        /// </summary>
        String VisibleField { get; set; }
        int Width { get; set; }
        Control OnLoad(DataRow row, Object item);
        String CssClass { get; set; }
        String CssClassDataField { get; set; }
        Boolean ColumnVisible { get; set; }
        string HeaderTooltip { get; set; }
    }

    public class TableViewCollection : List<TableColumn>
    {
        #region Fields
        #endregion

        #region Properties

        #endregion

        #region Methods
        #endregion
    }

    public abstract class TableColumn : StateManagedItem
    {
        #region Properties
        public Boolean Sortable
        {
            get
            {
                object o = ViewState["CTableColumnSortable"];
                return o == null ? true : (Boolean)o;
            }
            set { ViewState["CTableColumnSortable"] = value; }
        }

        public Boolean Filterable
        {
            get
            {
                object o = ViewState["CTableColumnFilterable"];
                return o == null ? true : (Boolean)o;
            }
            set { ViewState["CTableColumnFilterable"] = value; }
        }

        public String HeaderText
        {
            get
            {
                object o = ViewState["CTableColumnHeaderText"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CTableColumnHeaderText"] = value; }
        }
        public HorizontalAlign HeaderAlign
        {
            get
            {
                object o = ViewState["CTableColumnHeaderAlign"];
                return o == null ? HorizontalAlign.None : (HorizontalAlign)o;
            }
            set { ViewState["CTableColumnHeaderAlign"] = value; }
        }
        public String ID
        {
            get
            {
                object o = ViewState["CTableColumnID"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CTableColumnID"] = value; }
        }

        public String DataField
        {
            get
            {
                object o = ViewState["CTableColumnDataField"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CTableColumnDataField"] = value; }
        }
        public enum HorizontalAlign
        {
            None,
            [StringValueText("leftAlign")]
            Left,
            [StringValueText("centerAlign")]
            Center,
            [StringValueText("rightAlign")]
            Rigth
        }
        public HorizontalAlign Align
        {
            get
            {
                object o = ViewState["CTableColumnAlign"];
                return o == null ? HorizontalAlign.None : (HorizontalAlign)o;
            }
            set { ViewState["CTableColumnAlign"] = value; }
        }

        public int Width
        {
            get
            {
                object o = ViewState["CTableColumnWidth"];
                return o == null ? 0 : (int)o;
            }
            set { ViewState["CTableColumnWidth"] = value; }
        }

        public string HeaderTooltip { get; set; }
        [Description("Propriedade Boolean do datasource que diz se a coluna ficará visivel ou não")]
        public string VisibleField { get; set; }

        public String CssClass
        {
            get
            {
                object o = ViewState["CTableColumnCssClass"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CTableColumnCssClass"] = value; }
        }
        public String CssClassDataField
        {
            get
            {
                object o = ViewState["CTableColumnCssClassDataField"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CTableColumnCssClassDataField"] = value; }
        }

        public bool ColumnVisible
        {
            get
            {
                object o = ViewState["CTableColumnColumnVisible_" + ID];
                return o == null ? true : (bool)o;
            }
            set { ViewState["CTableColumnColumnVisible_" + ID] = value; }
        }
        #endregion

        public TableColumn()
        {
            ColumnVisible = true;
        }
    }

    public class TableColumnText : TableColumn, ITableColumn
    {
        #region Properties
        [Description("Quebra as Linhas procurando por <br/>")]
        public bool BreakLine { get; set; }
        public enum TypeColumn { String, Int32, Date, DateTime, Hour, Boolean, Decimal }
        public TypeColumn DataType
        {
            get
            {
                object o = ViewState["CTableColumnDataType"];
                return o == null ? TypeColumn.String : (TypeColumn)o;
            }
            set { ViewState["CTableColumnDataType"] = value; }
        }
        public Nullable<int> DecimalPlaceNumber { get; set; }
        public string ClassIcon { get; set; }
        public string BackgroudColorProperty { get; set; }
        #endregion

        #region Methods

        public Control OnLoad(DataRow row, Object item)
        {
            String values = !String.IsNullOrWhiteSpace(this.DataField) ? row[this.DataField].ToString() : "";
            string[] breaks = { values };

            if (BreakLine)
                breaks = values.Split(new string[] { "&lt;br/&gt;" }, StringSplitOptions.RemoveEmptyEntries);

            System.Web.UI.WebControls.Panel panel = new System.Web.UI.WebControls.Panel();
            bool first = true;
            foreach (var value in breaks)
            {
                if (!first)
                    panel.Controls.Add(new LiteralControl("<br/>"));

                first = false;

                Label lbl = new Label();
                lbl.ID = this.ID;

                if (Align != HorizontalAlign.None && !CssClass.Contains(Align.GetStringValueText()))
                    CssClass += $" {Align.GetStringValueText()}";

                //Não renderiza datetime.minvalue
                DateTime dt;
                if ((DataType == TypeColumn.DateTime || DataType == TypeColumn.Date || DataType == TypeColumn.Hour) &&
                    !String.IsNullOrWhiteSpace(value) &&
                    DateTime.TryParse(value, out dt))
                {
                    if (dt > DateTime.MinValue)
                    {
                        if (DataType == TypeColumn.DateTime)
                            lbl.Text = value;
                        else if (DataType == TypeColumn.Hour)
                            lbl.Text = dt.ToString("HH:mm:ss");
                        else
                            lbl.Text = dt.ToShortDateString();
                    }
                }
                else if (DataType == TypeColumn.Boolean)
                {
                    if (value.ToBoolean())
                        lbl.Text = "Sim";
                    else
                        lbl.Text = "Não";
                }
                else if (DataType == TypeColumn.Decimal)
                {
                    if (Align == HorizontalAlign.None && !CssClass.Contains(HorizontalAlign.Rigth.GetStringValueText()))
                        CssClass += $" {HorizontalAlign.Rigth.GetStringValueText()}";

                    if (!String.IsNullOrWhiteSpace(value) && DecimalPlaceNumber.HasValue && decimal.Parse(value) != decimal.MinValue && double.Parse(value) != double.MinValue)
                    {
                        lbl.Text = decimal.Parse(value).ToString("0." + new string('0', DecimalPlaceNumber.Value));
                        if (this.DataField == "VlrTotalRecalculo")
                        {
                            Decimal z = Decimal.Parse(lbl.Text);
                        }

                    }

                    else if (!String.IsNullOrWhiteSpace(value) && decimal.Parse(value) != decimal.MinValue && double.Parse(value) != double.MinValue)
                        lbl.Text = decimal.Parse(value).ToString("0.00");
                }
                else if (DataType == TypeColumn.Int32)
                {
                    if (!String.IsNullOrWhiteSpace(value) && int.Parse(value) != int.MinValue)
                        lbl.Text = int.Parse(value).ToString("0");
                }
                else
                    lbl.Text = value;

                lbl.CampoObrigatorio = false;

                if (!string.IsNullOrWhiteSpace(ClassIcon))
                    lbl.Controls.Add(new LiteralControl($"<i class='{ClassIcon}'></i>"));

                panel.Controls.Add(lbl);

            }

            return panel;
        }
        #endregion
    }

    public class TableColumnImageButton : TableColumn, ITableColumn
    {
        #region Fields
        [Browsable(true)]
        public event EventHandler Click;
        public delegate void EventHandler(ImageButton sender, EventArgs e);
        #endregion

        #region Properties
        /// <summary>
        ///     Url da Imagem que será motrada
        /// </summary>
        public String ImageUrl
        {
            get
            {
                object o = ViewState["CTableColumnImageUrl"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CTableColumnImageUrl"] = value; }
        }
        public bool DataValueAsImageUrl
        {
            get
            {
                object o = ViewState["CTableColumnDataValueAsImageUrl"];
                return o == null ? false : (bool)o;
            }
            set { ViewState["CTableColumnDataValueAsImageUrl"] = value; }
        }

        #endregion

        #region Methods

        public Control OnLoad(DataRow row, Object item)
        {
            String value = !String.IsNullOrWhiteSpace(this.DataField) ? row[this.DataField].ToString() : "";

            //Create Control
            ImageButton btn = new ImageButton();
            btn.ImageUrl = DataValueAsImageUrl ? value : ImageUrl;
            btn.ID = ID;
            btn.Click += OnClick;
            btn.DataValue = new Dictionary<string, string>();

            //Resolve Data
            foreach (string arg in this.DataField.Split(';'))
            {
                btn.DataValue.Add(arg, value);
            }

            return btn;
        }

        protected virtual void OnClick(object sender, EventArgs e)
        {
            if (this.Click != null)
                Click((ImageButton)sender, e);
        }
        #endregion
    }

    public class TableColumnButton : TableColumn, ITableColumn
    {
        #region Fields
        [Browsable(true)]
        public event EventHandler Click;
        public delegate void EventHandler(Button sender, EventArgs e);
        public event AddValueReturnFunctionEventHandler AddValueReturnFunction;
        #endregion

        #region Properties

        // DataEnabled se informado implica em habilitar ou desabilitar o botão
        [Browsable(false)]
        public String DataEnabledValue
        {
            get
            {
                var o = ViewState["TableColumnButtonDataEnabledValue"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["TableColumnButtonDataEnabledValue"] = value; }
        }
        public String DataEnabled
        {
            get
            {
                object o = ViewState["TableColumnDataEnabled"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["TableColumnDataEnabled"] = value; }
        }
        public ButtonStyle ButtonStyle { get; set; }
        public string Tooltip { get; set; }
        public string DataTooltip { get; set; }
        public string ModalName { get; set; }
        public string Text { get; set; }
        public bool ValueAsIcon { get; set; }
        public FontAwesomeIcons ButtonIcon { get; set; }
        public FontAwesomeType ButtonIconType { get; set; }
        public String PathAddValue { get; set; }
        public String PathEditValue { get; set; }
        public bool RegisterPostBack { get; set; }
        public string OnClientClick { get; set; }

        #endregion

        #region Methods

        public Control OnLoad(DataRow row, Object item)
        {
            //Create Control
            Button btn = new Button();
            btn.ID = ID;
            btn.Text = Text;

            if (this.Click != null)
                btn.Click += OnClick;

            if (this.AddValueReturnFunction != null)
                btn.AddValueReturnFunction += this.AddValueReturnFunction;

            btn.PathAddValue = this.PathAddValue;
            btn.DataValue = new Dictionary<string, string>();
            btn.ButtonStyle = this.ButtonStyle;
            btn.ButtonIcon = ButtonIcon;
            btn.ButtonIconType = ButtonIconType;

            if (!string.IsNullOrWhiteSpace(DataTooltip))
            {
                btn.ToolTip = ObjectHelper.GetPropertyValue(item, DataTooltip).ToString();
            }
            else
                btn.ToolTip = this.Tooltip;
            btn.ModalName = this.ModalName;

            if (!string.IsNullOrWhiteSpace(OnClientClick))
                btn.OnClientClick += OnClientClick;

            if (!String.IsNullOrWhiteSpace(PathEditValue))
            {
                String value = !String.IsNullOrWhiteSpace(this.DataField) ? row[this.DataField].ToString() : "";

                String parameters = String.Format("?FormMode=Edit&PKey={0}&{0}={1}", this.DataField, value);
                btn.OnClientClick += String.Format("OpenViewModal(undefined,\"{0}\",\"../{1}{2}\",\"{3}\");return false;", btn.ClientID, PathEditValue, parameters, btn.ResolveClientUrl("~/Includes/images/loader.gif"));
            }

            // Se tiver valor no campo de Enabled atribui o valor
            if (!String.IsNullOrEmpty(DataEnabledValue))
            {
                btn.Enabled = Convert.ToBoolean(DataEnabledValue);
            }

            //Resolve Data
            if (!ValueAsIcon)
            {
                foreach (string arg in this.DataField.Split(';'))
                {
                    string value = !String.IsNullOrWhiteSpace(arg) ? row[arg].ToString() : "";
                    btn.DataValue.Add(arg, value);
                }

            }
            else
            {
                String value = !String.IsNullOrWhiteSpace(this.DataField) ? row[this.DataField].ToString() : "";

                var valueSplited = value.Split('|');
                if (valueSplited.Length > 1) btn.Attributes.Add("CustomValue", valueSplited[1]);
            }


            return btn;
        }

        protected virtual void OnClick(object sender, EventArgs e)
        {
            if (this.Click != null)
                Click((Button)sender, e);
        }
        #endregion
    }

    public class TableColumnCheck : TableColumn, ITableColumn
    {
        #region Fields

        [Browsable(true)]
        public event EventHandler CheckedChanged;
        public delegate void EventHandler(CheckBox sender, EventArgs e);
        #endregion

        #region Properties

        [Browsable(false)]
        public String DataCheckedValue
        {
            get
            {
                var o = ViewState["TableColumnCheckDataCheckedValue"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["TableColumnCheckDataCheckedValue"] = value; }
        }

        /// <summary>
        ///     Propriedade do datasource que será passado por parametro ao clique do botão
        /// </summary>
        public String Argument
        {
            get
            {
                object o = ViewState["TableColumnCheckArgument"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["TableColumnCheckArgument"] = value; }
        }

        public Boolean HasCheckHeader
        {
            get
            {
                var o = ViewState["CTableColumnCheckHasCheckHeader"];
                return o == null || (Boolean)o;
            }
            set { ViewState["CTableColumnCheckHasCheckHeader"] = value; }
        }

        public Boolean Enabled
        {
            get
            {
                var o = ViewState["CTableColumnCheckDisable"];
                return o == null || (Boolean)o;
            }
            set { ViewState["CTableColumnCheckDisable"] = value; }
        }

        public String DataChecked
        {
            get
            {
                object o = ViewState["TableColumnDataChecked"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["TableColumnDataChecked"] = value; }
        }
        public string EnableField { get; set; }
        public CheckBoxTemplate Template { get; set; }
        #endregion

        #region Methods

        public Control OnLoad(DataRow row, Object item)
        {
            String value = !String.IsNullOrWhiteSpace(this.DataField) ? row[this.DataField].ToString() : "";

            bool? enableFiled = null;
            if (!String.IsNullOrWhiteSpace(EnableField))
                enableFiled = ObjectHelper.GetPropertyValue(item, EnableField).ToString().ToBoolean();

            //Create Control
            CheckBox chk = new CheckBox();
            chk.ID = ID;
            chk.Enabled = enableFiled == null ? Enabled : enableFiled.Value;
            chk.CampoObrigatorio = false;
            chk.MontaGroup = false;
            chk.DataField = this.DataField;
            chk.Attributes["chkColumnName"] = chk.ID;
            chk.Template = Template;

            if (CheckedChanged != null)
            {
                chk.AutoPostBack = true;
                chk.CheckedChanged += OnCheckedChanged;
            }

            // Se tiver valor no campo de Checked atribui o valor
            if (!String.IsNullOrEmpty(DataCheckedValue))
            {
                chk.Checked = Convert.ToBoolean(DataCheckedValue);
            }

            //Resolve Data
            foreach (string arg in this.DataField.Split(';'))
            {
                chk.DataValue.Add(arg, value);
            }

            return chk;
        }

        protected virtual void OnCheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckedChanged != null)
                CheckedChanged((CheckBox)sender, e);
        }
        #endregion
    }

    public class TableColumnInput : TableColumn, ITableColumn
    {
        #region Fields

        [Browsable(true)]
        public event EventHandler TextChanged;
        public delegate void EventHandler(TextBox sender, TableColumnInputEventArgs e);




        #endregion

        #region Properties
        public Boolean EventoAssincrono
        {
            get
            {
                var o = ViewState["CTableColumnInputEventoAssincrono"];
                return o == null || (Boolean)o;
            }
            set { ViewState["CTableColumnInputEventoAssincrono"] = value; }
        }

        public Boolean Enabled
        {
            get
            {
                var o = ViewState["CTableColumnInputDisable"];
                return o == null || (Boolean)o;
            }
            set { ViewState["CTableColumnInputDisable"] = value; }
        }
        public TextBox.MaskTextBox Mask
        {
            get
            {
                var o = ViewState["CTableColumnInputMask"];
                return o == null ? TextBox.MaskTextBox.NaoDefinido : (TextBox.MaskTextBox)o;
            }
            set { ViewState["CTableColumnInputMask"] = value; }
        }
        public String DataTextField { get; set; }
        #endregion

        #region Methods

        public Control OnLoad(DataRow row, Object item)
        {
            String value = !String.IsNullOrWhiteSpace(this.DataTextField) ? row[this.DataTextField].ToString() : "";

            //Create Control
            TextBox txt = new TextBox();
            txt.MontaGroup = false;
            txt.ID = ID;
            txt.Enabled = Enabled;
            txt.CampoObrigatorio = false;
            txt.MontaGroup = false;
            txt.Mask = this.Mask;
            txt.TableViewItem = item;

            if (TextChanged != null)
            {
                txt.AutoPostBack = true;
                txt.TextChanged += OnTextChanged;
            }

            if (!string.IsNullOrWhiteSpace(DataField))
                txt.TableViewEventArgs = ObjectHelper.GetPropertyValue(item, DataField).ToString();

            //Resolve Data
            txt.Text = value;

            return txt;
        }

        protected virtual void OnTextChanged(object sender, EventArgs e)
        {
            if (this.TextChanged != null)
            {
                TextChanged((TextBox)sender, new TableColumnInputEventArgs(((TextBox)sender).TableViewEventArgs));
                ScriptManager.RegisterStartupScript((HttpContext.Current.Handler as Page), (HttpContext.Current.Handler as Page).GetType(), "setFocusNextControl", "setFocusNextControl(" + ((TextBox)sender).ClientID + ");", true);
            }
        }
        public class TableColumnInputEventArgs : EventArgs
        {
            public TableColumnInputEventArgs(string datavalueeventargs)
            {
                DataValueEventArgs = datavalueeventargs;
            }
            public string DataValueEventArgs { get; set; }
        }
        #endregion
    }

    public class TableColumnDropDown : TableColumn, ITableColumn
    {
        #region Fields
        [Browsable(true)]
        public event EventHandler SelectedIndexChanged;
        public delegate void EventHandler(DropDownList sender, TableColumnDropDownEventArgs e);

        #endregion

        #region Properties

        public Boolean Enabled
        {
            get
            {
                var o = ViewState["TableColumnDropDownEnabled"];
                return o == null || (Boolean)o;
            }
            set { ViewState["TableColumnDropDownEnabled"] = value; }
        }
        [Description("Propriedade que irá habilitar ou não o combo")]
        public String EnabledField { get; set; }
        [Description("Propriedade que irá ser apresentada no combo")]
        public String DataTextField { get; set; }
        [Description("Propriedade que preenchera o valor do combo")]
        public String DataValueField { get; set; }
        public String DataFieldEventArgs { get; set; }
        #endregion

        #region Methods

        public Control OnLoad(DataRow row, Object item)
        {
            //Create Control
            DropDownList ddl = new DropDownList();
            ddl.MontaGroup = false;
            ddl.ID = ID;
            ddl.CampoObrigatorio = false;

            if (String.IsNullOrWhiteSpace(EnabledField))
                ddl.Enabled = Enabled;
            else
            {
                ddl.Enabled = (bool)ObjectHelper.GetPropertyValueObj(item, EnabledField);
            }



            object aux = ObjectHelper.GetPropertyValueObj(item, DataField);
            ddl.Carrega(DataValueField, DataTextField, aux as IList);

            object selectedValue = ObjectHelper.GetPropertyValue(item, DataValueField);
            if (selectedValue != null && !string.IsNullOrWhiteSpace(selectedValue.ToString()))
                ddl.SelectedValue = selectedValue.ToString();

            if (!string.IsNullOrWhiteSpace(DataFieldEventArgs))
                ddl.TableViewEventArgs = ObjectHelper.GetPropertyValue(item, DataFieldEventArgs).ToString();

            if (SelectedIndexChanged != null)
            {
                ddl.AutoPostBack = true;
                ddl.SelectedIndexChanged += OnSelectedIndexChanged;
            }

            return ddl;
        }

        protected virtual void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged((DropDownList)sender, new TableColumnDropDownEventArgs(((DropDownList)sender).TableViewEventArgs));
                ScriptManager.RegisterStartupScript((HttpContext.Current.Handler as Page), (HttpContext.Current.Handler as Page).GetType(), "setFocusNextControl", "setFocusNextControl(" + ((DropDownList)sender).ClientID + ");", true);
            }
        }
        #endregion

        public class TableColumnDropDownEventArgs : EventArgs
        {
            public TableColumnDropDownEventArgs(string datavalueeventargs)
            {
                DataValueEventArgs = datavalueeventargs;
            }
            public string DataValueEventArgs { get; set; }
        }
    }
    public class TableColumnRadio : TableColumn, ITableColumn
    {
        private RadioButtons radioButtons = null;

        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public RadioButtons RadioButtons
        {
            get { return radioButtons ?? (radioButtons = new RadioButtons()); }
            set { radioButtons = value; }
        }
        public RadioButtonTemplate Template { get; set; }
        #region Methods

        public Control OnLoad(DataRow row, Object item)
        {
            //Create Control
            RadioButtonGroup ctrl = new RadioButtonGroup();
            ctrl.RadioButtons = radioButtons;
            ctrl.Template = Template;
            ctrl.CampoObrigatorio = false;

            return ctrl;
        }

        #endregion
    }

    public class TableColumnFiles : TableColumn, ITableColumn
    {
        public string ByteArrayDataField { get; set; }
        public string FileNameDataField { get; set; }
        public string FileTypeDataField { get; set; }

        [Browsable(false)]
        private UploadFileS Files
        {
            get
            {
                if (ViewState[this.ID + "download-files"] == null)
                {
                    ViewState[this.ID + "download-files"] = new UploadFileS();
                }

                return (UploadFileS)ViewState[this.ID + "download-files"];
            }
            set { ViewState[this.ID + "download-files"] = value; }
        }

        public Control OnLoad(DataRow row, object item)
        {

            System.Web.UI.WebControls.Panel columnFiles = new System.Web.UI.WebControls.Panel();
            columnFiles.CssClass += "tableColumnFiles";


            object collection = ObjectHelper.GetPropertyValueObj(item, DataField);

            foreach (var colItem in collection as IEnumerable)
            {
                System.Web.UI.WebControls.Panel columnFilesItem = new System.Web.UI.WebControls.Panel();

                string fileType = "", fileName = "";
                byte[] data = (byte[])ObjectHelper.GetPropertyValueObj(colItem, ByteArrayDataField);
                fileType = ObjectHelper.GetPropertyValue(colItem, FileTypeDataField).ToString();
                fileName = ObjectHelper.GetPropertyValue(colItem, FileNameDataField).ToString();

                string srcAttribute = "";
                if (fileType.ToLower().Contains("image"))
                    srcAttribute = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(Imagem.MakeThumbnail(data, 50, 50)));
                else if (fileType.ToLower().Contains("pdf"))
                    srcAttribute = "../../Includes/Icons/FileUpload/PDFIcon.png";
                else if (fileType.ToLower().Contains("application") && fileName.Split('.').Last().ToLower() == "zip")
                    srcAttribute = "../../Includes/Icons/FileUpload/ZIPIcon.png";
                else if (fileType.ToLower().Contains("spreadsheet"))
                    srcAttribute = "../../Includes/Icons/FileUpload/ExcelIcon.png";
                else if (fileType.ToLower().Contains("word"))
                    srcAttribute = "../../Includes/Icons/FileUpload/WordIcon.png";
                else if (fileType.ToLower().Contains("xml"))
                    srcAttribute = "../../Includes/Icons/FileUpload/XMLIcon.png";
                else if (fileType.ToLower().Contains("text"))
                    srcAttribute = "../../Includes/Icons/FileUpload/NotepadIcon.png";

                LiteralControl htmlImage = new LiteralControl(string.Format("<img data-dz-thumbnail alt='{0}' src='{1}' > ", fileName, srcAttribute));

                UploadFile file = new UploadFile()
                {
                    dados = data,
                    type = fileType,
                    name = fileName
                };

                Files.Add(file);

                Button btn = new Button();
                btn.Name = file.NrObjectIdentifier;
                btn.Click += btnHdnDownloadClick;
                btn.Style.Add("display", "none");

                columnFilesItem.CssClass += "tableColumnFiles_item";

                columnFilesItem.Controls.Add(htmlImage);
                columnFilesItem.Controls.Add(btn);


                columnFiles.Controls.Add(columnFilesItem);
            }

            return columnFiles;
        }

        public void btnHdnDownloadClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            UploadFile arq = Files.Where(x => x.NrObjectIdentifier == button.Name).First();

            var response = button.Page.Response;

            response.ContentType = arq.type;

            response.AddHeader("Content-Disposition", "attachment; filename=" + arq.name);
            response.AddHeader("Content-Length", arq.dados.Length.ToString());

            MemoryStream ms = new MemoryStream(arq.dados);
            ms.WriteTo(response.OutputStream);

            response.Flush();
        }
    }
}