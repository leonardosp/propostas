using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class UpdateProgress : System.Web.UI.UpdateProgress
    {
        protected override void OnLoad(EventArgs e)
        {
            ProgressTemplate = new ProgressTemplate();
            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            System.Web.UI.WebControls.Image loadImage = new System.Web.UI.WebControls.Image();
            loadImage.ImageUrl = ((Page)HttpContext.Current.Handler).ResolveClientUrl("~/Includes/Images/loader.gif");
            loadImage.AlternateText = "Processando";

            System.Web.UI.HtmlControls.HtmlGenericControl divProcessMessage = new System.Web.UI.HtmlControls.HtmlGenericControl();
            divProcessMessage.Attributes["class"] = "processMessage";
            divProcessMessage.Controls.Add(loadImage);

            System.Web.UI.HtmlControls.HtmlGenericControl divProgressBackgroundFilter = new System.Web.UI.HtmlControls.HtmlGenericControl();
            divProgressBackgroundFilter.Attributes["class"] = "progressBackgroundFilter";

            Controls.Add(divProcessMessage);
            Controls.Add(divProgressBackgroundFilter);

            base.Render(writer);
        }
    }

    public class ProgressTemplate : ITemplate
    {
        public void InstantiateIn(Control container) { }
    }
}
