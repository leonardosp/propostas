using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]

    public class Image : System.Web.UI.WebControls.Image
    {
        public String Src
        {
            get
            {
                object o = ViewState["CImageSrc"];
                return o == null ? String.Empty : o.ToString();
            }
            set { ViewState["CImageSrc"] = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(Src))
            {
                writer.Write("<img src='{0}' {1} />", Src, string.IsNullOrWhiteSpace(CssClass) ? "" : "class='" + CssClass + "'");
            }
            else
                base.Render(writer);
        }
    }
}