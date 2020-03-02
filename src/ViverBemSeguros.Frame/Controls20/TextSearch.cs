using Celebre.Frame.FrameWork;
using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    public delegate void ListaDataBindEventHandler(object sender, ListDataBindEventArgs e);
    public delegate void SelectedValueEventHandler(object sender, SelectedValueEventArgs e);

    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class TextSearch : System.Web.UI.WebControls.TextBox, IPostBackEventHandler
    {
        private bool montagroup = true;
        private bool campoobrigatorio = true;
        private bool obrigatoriovazio;
        private Boolean likecompleto = true;
        private InputGroupButtons inputGroupButtons = null;


        public bool LikeCompleto
        {
            get
            {
                return this.likecompleto;
            }
            set
            {
                this.likecompleto = value;
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
        public String Text
        {
            get { return (String)ViewState["Text"] ?? String.Empty; }
            set { ViewState["Text"] = value; }
        }
        public String Value
        {
            get { return (String)ViewState["Value"] ?? String.Empty; }
            set { ViewState["Value"] = value; }
        }
        /// <summary>
        /// Caminho para a pagina de manutenção onde o regitro será criado
        /// ex: FRO/Pessoa_item.aspx
        /// </summary>
        public String PathAddValue { get; set; }
        /// <summary>
        /// Caminho para a pagina de manutenção onde o regitro será aberto
        /// ex: FRO/Pessoa_item.aspx
        /// </summary>
        public String PathOpenRegistry { get; set; }
        /// <summary>
        /// Chaves necessárias para abrir a pagina de manutenção
        /// ex: Pessoa_item.aspx -> NrSeqPessoa
        /// </summary>
        public String PathOpenRegistryKey { get; set; }
        public String Help { get; set; }
        public String Label { get; set; }
        public TypeControlForm TypeControl { get; set; }

        public List<ListDataBindEventArgs.ListDataBindItem> Itens
        {
            get;
            set;
        }

        [Browsable(true)]
        public event ListaDataBindEventHandler ListDataBinding;
        [Browsable(true)]
        public event SelectedValueEventHandler SelectedValue;
        [Browsable(true)]
        public event AddValueReturnFunctionEventHandler AddValueReturnFunction;
        [Browsable(true)]
        public event EventHandler ValueChanged;
        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public InputGroupButtons GroupButtons
        {
            get { return inputGroupButtons ?? (inputGroupButtons = new InputGroupButtons()); }
            set { inputGroupButtons = value; }
        }
        protected virtual void OnListDataBinding(ListDataBindEventArgs e)
        {
            this.Text = e.Text;

            Int64 byvalue = 0;
            if (Int64.TryParse(e.Text, out byvalue))
            {
                e.Mode = TextSearchMode.byValue;
            }
            else
            {
                e.Mode = TextSearchMode.byText;
                if (LikeCompleto)
                {
                    e.Text = String.Format("%{0}%", e.Text);
                }
                else
                {
                    e.Text = String.Format("{0}%", e.Text);
                }
            }

            if (ListDataBinding != null)
            {
                ListDataBinding(this, e);
                this.Itens = e.Itens;

                if (this.Itens.Count == 0)
                {
                    if (SelectedValue != null)
                        SelectedValue(this, new SelectedValueEventArgs(string.Empty, string.Empty));

                    Text = Value = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "setFocusNextControl", "setFocusNextControl(" + this.ClientID + ");", true);
                }
                else if (this.Itens.Count == 1)
                {
                    this.Text = this.Itens[0].Text;
                    this.Value = this.Itens[0].Value;

                    if (SelectedValue != null)
                        SelectedValue(this, new SelectedValueEventArgs(this.Value, this.Text));

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "setFocusNextControl", "setFocusNextControl(" + this.ClientID + ");", true);
                }
            }
        }
        protected virtual void OnSelectedValue(SelectedValueEventArgs e)
        {
            this.Text = e.Text;
            this.Value = e.Value;

            if (SelectedValue != null)
            {
                SelectedValue(this, e);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "setFocusNextControl", "setFocusNextControl(" + this.ClientID + ");", true);
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var item in GroupButtons)
                Controls.Add(item);
        }

        protected virtual void OnAddValueReturnFunction(AddValueReturnFunctionEventArgs e)
        {
            this.Text = e.Text;
            this.Value = e.Value;

            if (SelectedValue != null)
                SelectedValue(this, new SelectedValueEventArgs(e.Value, e.Text));
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (montagroup)
                output.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.Label : this.Label, this.ID, TypeControl, true, false, ToolTip));

            string scriptAjustaSelect = String.Format("AjustaTextSearch('{0}_select','{0}');", this.ClientID);

            String scriptClickChange = Page.ClientScript.GetPostBackEventReference(this, RaiseEvents.OnClickChangeEvent.ToString());
            scriptClickChange = scriptClickChange.Replace("'OnClickChangeEvent'", "'OnClickChangeEvent***'+$(this).val()");

            output.Write("<span class='input-group-addon controlClickable'{0}{1}><i class='{2} {3}'></i></span>",
                this.Enabled ? String.Format(" onclick=\"{0}{1};\"", scriptAjustaSelect, scriptClickChange) : "",
                this.Enabled ? "" : " disabled='disabled'",
                FontAwesomeType.Solid.GetStringValueClass(),
                FontAwesomeIcons.Find.GetStringValueClass());


            String scriptChange = Page.ClientScript.GetPostBackEventReference(this, RaiseEvents.OnChangeEvent.ToString());
            scriptChange = scriptChange.Replace("'OnChangeEvent'", "'OnChangeEvent***'+$(this).val()");

            CssClass = "form-control " + this.CssClass;
            Attributes["onkeydown"] = "KeyDownTextSearch(event,this);";
            Attributes["onchange"] = String.Format("{0}{1}", scriptAjustaSelect, scriptChange);
            Attributes["data-change"] = String.Format("{0}{1}", scriptAjustaSelect, scriptChange);
            Attributes["autocomplete"] = "off";
            Attributes["data-lasttext"] = Text;
            Attributes["data-control-required"] = campoobrigatorio.ToString();
            Attributes["data-control"] = "TextSearch";
            Attributes["data-value"] = Value;

            base.Render(output);

            if (GroupButtons.Count > 0)
            {
                if (GroupButtons.Count(p => p.ButtonPosition == InputGroupButton.InputGroupButtonPosition.Right) > 0)
                {

                    output.Write("<div class='input-group-btn'>");

                    foreach (var btn in GroupButtons.Where(p => p.ButtonPosition == InputGroupButton.InputGroupButtonPosition.Right))
                        btn.RenderControl(output);

                    output.Write("</div>");
                }

                //output.Write("</div>");
            }

            if (!String.IsNullOrWhiteSpace(PathOpenRegistry) && !String.IsNullOrWhiteSpace(this.Value))
            {
                string param = String.Format("FormMode={0}&PKey={1}&{1}={2}",
                    Celebre.Web.Pages.ManutPage.FormPageMode.Edit.ToString(),
                    PathOpenRegistryKey,
                    this.Value);

                string[] auxPath = PathOpenRegistry.Split(new char[] { '/' });
                Celebre.FrameWork.Seg.TransacaoPagina transacaoPagina = new Celebre.FrameWork.Seg.TransacaoPagina();
                transacaoPagina.NrSeqGrupoEmpresa = Celebre.FrameWork.Seg.Sessao.GetInstance().NrSeqGrupoEmpresa;
                transacaoPagina.NoPaginaweb = auxPath[auxPath.Length - 1];
                transacaoPagina.Localizar();

                if (transacaoPagina.NrSeqTransacao > 0)
                {
                    FrameWork.Seg.Transacao trans = transacaoPagina.Transacao;

                    string selecao = "";
                    string manutencao = "";

                    if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("ApplicationSubPath"))
                    {
                        selecao = string.Format("parent.$('#celebreMasterTabs').celebreTabs('addWithInner','{0}', '../Sistema/{1}/{2}','{3}','SELPAGE','Seleção');", trans.NoTransacao, trans.NoDirFonte, trans.TransacaoPaginaPrincipal.NoPaginaweb, System.Configuration.ConfigurationManager.AppSettings["ApplicationSubPath"]);
                        manutencao = string.Format("parent.$('#celebreMasterTabs').celebreTabs('addInnerTab','_SISTEMA_{0}_{1}', '../Sistema/{0}/{2}?{3}','MANUTPAGE','Manutenção',undefined,'{4}');", trans.NoDirFonte, trans.TransacaoPaginaDestino.NoPaginaweb.ToUpper().Replace("_ITEM.ASPX", ""), trans.TransacaoPaginaDestino.NoPaginaweb, param, System.Configuration.ConfigurationManager.AppSettings["ApplicationSubPath"]);
                    }
                    else
                    {
                        selecao = string.Format("parent.$('#celebreMasterTabs').celebreTabs('addWithInner','{0}', '../Sistema/{1}/{2}','{3}','SELPAGE','Seleção');", trans.NoTransacao, trans.NoDirFonte, trans.TransacaoPaginaPrincipal.NoPaginaweb, "");
                        manutencao = string.Format("parent.$('#celebreMasterTabs').celebreTabs('addInnerTab','_SISTEMA_{0}_{1}', '../Sistema/{0}/{2}?{3}','MANUTPAGE','Manutenção',undefined,'{4}');", trans.NoDirFonte, trans.TransacaoPaginaDestino.NoPaginaweb.ToUpper().Replace("_ITEM.ASPX", ""), trans.TransacaoPaginaDestino.NoPaginaweb, param, "");
                    }






                    output.Write("<span class='input-group-addon controlClickable' style='border-left: 0;'{0}{1}><i class='{2} {3}'></i></span>",
                        //Enabled ? String.Format(" onclick=\"{0}{1}\"", selecao, manutencao) : "",
                        String.Format(" onclick=\"{0}{1}\"", selecao, manutencao),
                        //Enabled ? "" : " disabled='disabled'",
                        Enabled ? "" : "",
                        FontAwesomeType.Solid.GetStringValueClass(),
                        FontAwesomeIcons.FolderOpen.GetStringValueClass());
                }
                else
                    Util.Log.Write(new CelebreException($"Transação Página não existe na base {auxPath[auxPath.Length - 1]}"));
            }

            if (!String.IsNullOrWhiteSpace(PathAddValue))
                output.Write("<span class='input-group-addon controlClickable'{0}{1}><i class='{2} {3}'></i></span>",
                    Enabled ? String.Format("onclick='OpenAddModal(\"{0}\",\"../{1}\",\"{2}\")'", ClientID, PathAddValue, Page.ResolveClientUrl("~/Includes/images/loader.gif")) : "",
                    Enabled ? "" : " disabled='disabled'",
                FontAwesomeType.Regular.GetStringValueClass(),
                FontAwesomeIcons.FileText.GetStringValueClass());

            if (this.Itens != null && this.Itens.Count > 1)
            {
                output.Write("<div id='{0}_select' name='{0}_select' class='TextSearchSelectList' style='position:absolute;z-index:99;left:0;{1}'><ul>", this.ClientID, this.Itens.Count > 6 ? "height:150px;" : "");
                int index = 1;
                foreach (var item in this.Itens)
                {
                    output.Write("<li tabindex=\"{0}\" data-val=\"{1}\" onclick=\"{3}\" >{2}</li>", index, item.Value, item.Text, Page.ClientScript.GetPostBackEventReference(this, String.Format("{0}***{1}***{2}", RaiseEvents.OnSelectValueChanged.ToString(), item.Value, item.Text)));
                    index++;
                }
                output.Write("</ul></div>");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "AjustaTextSearchAux", String.Format("AjustaTextSearchAux('{0}_select','{0}');", this.ClientID), true);
            }

            if (montagroup)
                output.Write(ComponentUtils.RenderControlClose(Help, TypeControl, true));
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            this.Value = String.Empty;

            if (eventArgument.Contains(RaiseEvents.OnClickChangeEvent.ToString()))
                OnListDataBinding(new ListDataBindEventArgs(eventArgument));
            else if (eventArgument.Contains(RaiseEvents.OnChangeEvent.ToString()))
            {
                ListDataBindEventArgs eventArgs = new ListDataBindEventArgs(eventArgument);
                if (!String.IsNullOrWhiteSpace(eventArgs.Text))
                    OnListDataBinding(eventArgs);
                else
                {
                    if (SelectedValue != null)
                        SelectedValue(this, new SelectedValueEventArgs(string.Empty, string.Empty));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "setFocusNextControl", "setFocusNextControl(" + this.ClientID + ");", true);
                }

            }
            else if (eventArgument.Contains(RaiseEvents.OnSelectValueChanged.ToString()))
                OnSelectedValue(new SelectedValueEventArgs(eventArgument));
            else if (eventArgument.Contains(RaiseEvents.OnAddValueReturnFunctionEvent.ToString()))
                OnAddValueReturnFunction(new AddValueReturnFunctionEventArgs(eventArgument));

        }

        public void Reset()
        {
            this.Text = String.Empty;
            this.Value = String.Empty;
        }

        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/Controls/TextSearch.js") + "'></script>"));

            return listScriptsAndStyles;
        }

        private enum RaiseEvents
        {
            OnChangeEvent,
            OnClickChangeEvent,
            OnSelectValueChanged,
            OnAddValueReturnFunctionEvent
        }
    }

    public enum TextSearchMode
    {
        byText,
        byValue
    }
    [Serializable]
    public class ListDataBindEventArgs : EventArgs
    {
        public String Text { get; set; }
        internal List<ListDataBindItem> Itens { get; set; }
        public TextSearchMode Mode { get; set; }

        public ListDataBindEventArgs()
        {
            Itens = new List<ListDataBindItem>();
        }

        public ListDataBindEventArgs(String arguments)
        {
            string[] args = arguments.Split(new string[] { "***" }, StringSplitOptions.None);

            if (args.Length > 1)
                this.Text = args[1];
            else
                this.Text = String.Empty;

            Itens = new List<ListDataBindItem>();
        }

        public void SetItem(String value, String text)
        {
            Itens.Add(new ListDataBindItem() { Text = text, Value = value });
        }

        [Serializable]
        public class ListDataBindItem
        {
            public String Value { get; set; }
            public String Text { get; set; }
        }
    }
    [Serializable]
    public class SelectedValueEventArgs : EventArgs
    {
        public String Text { get; set; }
        public String Value { get; set; }
        public static SelectedValueEventArgs Empty { get { return new SelectedValueEventArgs(); } }

        private SelectedValueEventArgs() { }
        public SelectedValueEventArgs(String arguments)
        {
            string[] args = arguments.Split(new string[] { "***" }, StringSplitOptions.None);
            Value = args[1];
            Text = args[2];
        }
        public SelectedValueEventArgs(String Value, String Text)
        {
            this.Value = Value;
            this.Text = Text;
        }
    }

}
