using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class ListBox : System.Web.UI.WebControls.ListBox
    {
        private bool campoobrigatorio = true;
        private string cssimage;
        private string label;
        private string help;
        private bool montagroup = true;
        private string datafieldname;
        private string _initText = "Selecione uma Opção Abaixo";

        public TypeControlForm TypeControl { get; set; }
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

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (montagroup)
            {
                writer.Write(ComponentUtils.RenderControlOpen(this.campoobrigatorio ? "*" + this.label : this.label, this.ID, TypeControl));
                this.CssClass = "form-control " + this.CssClass;
                base.Render(writer);
                writer.Write(ComponentUtils.RenderControlClose(help,TypeControl));
            }
            else 
            {
                base.Render(writer);
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

        public void Carrega(string PvalueMember, string PdisplayMember, object Pdata)
        {
            //Creates an instance of the specified type using the constructor that best matches the specified parameters.

            System.Reflection.PropertyInfo lcount = Pdata.GetType().GetProperty("Count");

            this.DataValueField = PvalueMember;
            this.DataTextField = PdisplayMember;

            this.DataSource = Pdata;

            this.DataBind();
        }

        public void carregaComItemSelecionado(string PvalueMember, string PdisplayMember,object Pdata, string valueNewItemSelected)
        {
            List<string> keys = new List<string>();
            foreach(ListItem v in this.Items)
            {
                if (v.Selected==true)
                {
                    keys.Add(v.Value);
                }
            }

            this.Carrega(PvalueMember,PdisplayMember,Pdata);

            if (valueNewItemSelected!="")
            {
                this.Items.FindByValue(valueNewItemSelected).Selected = true;
            }
           

            foreach (string item in keys)
            {
                this.Items.FindByValue(item).Selected=true;
            }
        }


        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);

            if (InitItem)
            {
                base.Items.Insert(0, new ListItem(_initText, Int32.MinValue.ToString()));
            }

        }

        public class RegList
        {
            //SetProperty(obj, PdisplayMember, init_text)
            //SetProperty(obj, PvalueMember, Integer.MinValue)

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

        public ListBox()
            : base()
        {
            
        }
    }
}
