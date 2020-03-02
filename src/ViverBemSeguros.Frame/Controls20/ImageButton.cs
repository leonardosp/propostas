using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class ImageButton : System.Web.UI.WebControls.LinkButton
    {
        public ImageButton()
            : base()
        {
            DataValue = new Dictionary<string, string>();
        }

        public String DataField { get; set; }
        public String ImageUrl { get; set; }
        public Dictionary<string, string> DataValue { get; set; }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Image img = new Image();
            img.ImageUrl = ImageUrl;
            img.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            this.Controls.Add(img);

            Label lbl = new Label();
            lbl.Text = this.Text;
            lbl.CampoObrigatorio = false;

            this.Controls.Add(lbl);

            this.Style["text-decoration"] = "none";
            base.Render(writer);
        }
    }
}
