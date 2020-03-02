using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using Celebre.Web.Utils;
using System.Web.UI.WebControls;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class Tab : StateManagedCompositeControl
    {
        private TabContentCollection tabcontentcollection = null;

        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public TabContentCollection TabContentCollection
        {
            get
            {
                if (tabcontentcollection == null)
                {
                    tabcontentcollection = new TabContentCollection();
                }
                return tabcontentcollection;
            }
            set { tabcontentcollection = value; }
        }
        
        protected override void OnInit(EventArgs e)
        {
            HiddenField hiddenField = new HiddenField();
            hiddenField.ID = "hdnTabIdActive";

            if (Page.IsPostBack)
                hiddenField.Value = Page.Request.Form[this.ID + "$hdnTabIdActive"].ToString();
            
            this.Controls.Add(hiddenField);
            
            foreach (var item in TabContentCollection)
                this.Controls.Add(item);

            base.OnInit(e);
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            HiddenField hiddenField = (HiddenField)this.FindControl("hdnTabIdActive");
            hiddenField.RenderControl(writer);

            writer.WriteLine("<ul class='nav tab-menu nav-tabs{1}' id='{0}' role='tablist'>", this.ID, string.IsNullOrWhiteSpace(CssClass) ? "" : $" {CssClass}");
            foreach (TabContent tabcontent in tabcontentcollection.Where(p=>p.Visible))
            {
                string liclass = "";
                if (String.IsNullOrWhiteSpace(hiddenField.Value))
                    liclass = tabcontent == tabcontentcollection.Where(p => p.Visible).First() ? "active" : "";
                else
                    liclass = tabcontent.ID == hiddenField.Value ? "active" : "";

                writer.WriteLine("<li role='presentation' class=\"{0}\" onclick=\"$('#{3}_hdnTabIdActive').val('{1}');\"><a href=\"#{1}\" aria-controls=\"{1}\" role='tab' data-toggle='tab'>{4}{2}</a></li>",
                    liclass, 
                    tabcontent.ID, 
                    tabcontent.Header,
                    this.ID,
                    tabcontent.HeaderIcon == FontAwesomeIcons.NotSet ? "" : String.Format("<i class='fa {0} marginright5'></i>", tabcontent.HeaderIcon.GetStringValueClass())
                    
                    );
            }
            writer.WriteLine("</ul>");

            writer.WriteLine("<div id='{0}Content' class='tab-content'>", this.ID);
            foreach (TabContent tabcontent in tabcontentcollection.Where(p => p.Visible))
            {
                string liclass = "";
                if (String.IsNullOrWhiteSpace(hiddenField.Value))
                    liclass = tabcontent == tabcontentcollection.Where(p => p.Visible).First() ? "active" : "";
                else
                    liclass = tabcontent.ID == hiddenField.Value ? "active" : "";

                writer.WriteLine("<div role='tabpanel' class='tab-pane {0}' id='{1}'>",
                    liclass, 
                    tabcontent.ID);

                tabcontent.RenderControl(writer);
                writer.WriteLine("</div>");
            }
            writer.WriteLine("</div>");
        }

        /// <summary>
        /// Método utilizado para selecionar a Tab
        /// </summary>
        /// <param name="IdTab">Id da Tab a ser selecionada</param>
        public void SelectTab(string IdTab)
        {
            ((HiddenField)this.FindControl("hdnTabIdActive")).Value = IdTab;
        }
    }
    [Serializable]
    public class TabContentCollection : List<TabContent>
    {
    }
    [Serializable]
    [ParseChildren(false), PersistChildren(false)]
    public class TabContent : StateManagedCompositeControl
    {
        public string ID { get; set; }
        public string Header { get; set; }
        public FontAwesomeIcons HeaderIcon { get; set; }
    }
}
