using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class ComboElem
    {
        public ComboElem(string valor, int codigo, string nome)
        {
            Valor = valor;
            Codigo = codigo;
            Nome = nome;
        }

        public ComboElem(string valor, int codigo)
        {
            Valor = valor;
            Codigo = codigo;
        }

        string _valor;
        int _Codigo;
        string _nome;

        public string Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

        public int Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
    }
    [Serializable]
    public class DropDownList : System.Web.UI.WebControls.DropDownList
    {
        private bool campoobrigatorio = true;
        private bool obrigatoriovazio;
        private string label;
        private string help;
        private bool montagroup = true;
        private string datafieldname;
        private string _initText = "Selecione uma Opção Abaixo";
        public OrderByDropDown OrderBy { get; set; }
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
        public bool InitItem
        {
            get
            {
                if (ViewState["InitItem"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)ViewState["InitItem"];
                }

            }
            set
            {
                ViewState["InitItem"] = value;
            }
        }
        public void ClearItens()
        {
            Items.Clear();
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
        /// <summary>
        /// Acessivel apenas dentro do Framework
        /// </summary>
        internal string TableViewEventArgs
        {
            get
            {
                if (ViewState["DDLTableViewEventArgs"] == null)
                {
                    return "";
                }
                else
                {
                    return ViewState["DDLTableViewEventArgs"].ToString();
                }

            }
            set
            {
                ViewState["DDLTableViewEventArgs"] = value;
            }
        }
        private bool SetProperty(object obj, string propertyName, object val)
        {
            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(propertyName);

            try
            {
                // get a reference to the PropertyInfo, exit if no property with that 
                // name

                if (pi == null)
                {
                    return false;
                }

                // convert the value to the expected type
                val = Convert.ChangeType(val, pi.PropertyType);

                // attempt the assignment
                pi.SetValue(obj, val, null);


                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Carrega(string PvalueMember, string PdisplayMember, object Pdata)
        {
            DataValueField = PvalueMember;
            DataTextField = PdisplayMember;
            DataSource = Pdata;

            DataBind();
        }
        public void CarregaOrdered<T>(string PvalueMember, string PdisplayMember, object Pdata)
        {
            DataValueField = PvalueMember;
            DataTextField = PdisplayMember;

            if (OrderBy == OrderByDropDown.ASC)
                DataSource = (Pdata as IEnumerable<T>).OrderBy(p => p.GetType().GetProperty(PdisplayMember));
            else if (OrderBy == OrderByDropDown.DESC)
                DataSource = (Pdata as IEnumerable<T>).OrderByDescending(p => p.GetType().GetProperty(PdisplayMember));
            else
                DataSource = Pdata;

            DataBind();
        }


        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);

            if (InitItem)
                base.Items.Insert(0, new ListItem(_initText, Int32.MinValue.ToString()));
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            CssClass = "form-control " + CssClass;
            if (montagroup)
            {
                writer.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.label : this.label, this.ID, TypeControl, false, ObrigatorioVazio, ToolTip));                
                Attributes.Add("data-control-required", campoobrigatorio.ToString());
                base.Render(writer);
                writer.Write(ComponentUtils.RenderControlClose(help, TypeControl, false));
            }
            else
            {
                base.Render(writer);
            }
        }

        public class RegList
        {
            private string _displayMember;
            private string _valueMember;

            public RegList(string PdisplayMember, string PvalueMember)
            {
                _displayMember = PdisplayMember;
                _valueMember = PvalueMember;
            }
            public string displayMember
            {
                get { return _displayMember; }
            }

            public string valueMember
            {
                get { return _valueMember; }
            }

        }
    }

    public enum OrderByDropDown
    {
        none,
        ASC,
        DESC
    }

}
