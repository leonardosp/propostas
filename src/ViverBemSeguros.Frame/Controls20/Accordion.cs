using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class Accordion : StateManagedCompositeControl
    {
        private AccordionTemplateCollection accordionTemplateCollection = null;

        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public AccordionTemplateCollection AccordionTemplateCollection
        {
            get
            {
                if (accordionTemplateCollection == null)
                {
                    accordionTemplateCollection = new AccordionTemplateCollection();
                }
                return accordionTemplateCollection;
            }
        }
        public String IdOpenedOnStart { get; set; }

        protected override void OnInit(EventArgs e)
        {
            foreach (var item in AccordionTemplateCollection)
                this.Controls.Add(item);

            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div class='panel-group' id='{0}' role='tablist' aria-multiselectable='true'>", this.ID);

            foreach (var item in AccordionTemplateCollection)
            {
                writer.Write("<div class='panel panel-default'>");
                writer.Write("<div class='panel-heading' role='tab' id='heading{0}'>", item.ID);
                writer.Write("<h4 class='panel-title'>");
                writer.Write("<a data-toggle='collapse' data-parent='#{0}' href='#collapse{1}' aria-expanded='true' aria-controls='collapse{1}'>{2}</a>", this.ID, item.ID, item.HeaderText);
                writer.Write("</h4>");
                writer.Write("</div>");

                writer.Write("<div id='collapse{0}' class='panel-collapse collapse{1}' role='tabpanel' aria-labelledby='heading{0}'>", item.ID, IdOpenedOnStart == item.ID ? " in" : "");
                writer.Write("<div class='panel-body'>");
                item.RenderControl(writer);
                writer.Write("</div>");
                writer.Write("</div>");

                writer.Write("</div>");
            }

            writer.Write("</div>");
        }
    }
    [Serializable]
    [ParseChildren(false), PersistChildren(false)]
    public class AccordionTemplate : StateManagedCompositeControl
    {
        public String HeaderText { get; set; }
    }
    [Serializable]
    public class AccordionTemplateCollection : List<AccordionTemplate>
    {

    }
}
