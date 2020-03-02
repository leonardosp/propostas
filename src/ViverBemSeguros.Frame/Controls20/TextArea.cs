using System.Web.UI;
using Celebre.Web.Utils;
using System.Collections.Generic;
using System;
using System.Web;
using System.Linq;

namespace Celebre.Web.Controls
{
    [ParseChildren(true), PersistChildren(false)]
    public class TextArea : System.Web.UI.WebControls.TextBox
    {
        public bool CampoObrigatorio { get; set; } = true;
        public bool MontaGroup { get; set; } = true;
        public string Label { get; set; }
        public string Help { get; set; }
        public TextAreaTemplate Template { get; set; }

        protected override void OnInit(EventArgs e)
        {
            if (Template == TextAreaTemplate.TextEditor)
            {
                var hdnArea = new System.Web.UI.WebControls.HiddenField();
                hdnArea.ID = ID + "_hdnArea";
                hdnArea.Value = "";

                Controls.Add(hdnArea);
            }

            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (Template == TextAreaTemplate.TextEditor)
            {
                if (Page.IsPostBack && Page.Request.Form.AllKeys.Any(p=> p.Contains(ID + "_hdnArea")))
                    Text = Page.Request.Form[Page.Request.Form.AllKeys.First(p => p.Contains(ID + "_hdnArea"))].ToString();
            }

            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (Template == TextAreaTemplate.TextEditor)
            {
                var hdnArea = (System.Web.UI.WebControls.HiddenField)FindControl(ID + "_hdnArea");
                hdnArea.Value = HttpUtility.HtmlDecode(Text);
                hdnArea.RenderControl(writer);

                if (MontaGroup)
                    writer.Write(ComponentUtils.RenderControlOpen(CampoObrigatorio ? "*" + Label : Label, ID, TypeControlForm.Default, true, false, ToolTip));

                writer.Write($"<textarea id='{ID}' data-control='TextEditor'></textarea>");

                if (MontaGroup)
                    writer.Write(ComponentUtils.RenderControlClose(Help, TypeControlForm.Default, true));
            }
            else
                base.Render(writer);
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            throw new System.NotImplementedException();
        }

        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            //listScriptsAndStyles.Add(new LiteralControl("<link rel='stylesheet' href='" + ResolveUrl("~/Includes/Styles/Controls/fullcalendar.css") + "' />"));

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/Controls/ckeditor/ckeditor.js") + "'></script>"));

            return listScriptsAndStyles;
        }
    }

    public enum TextAreaTemplate
    {
        Default,
        TextEditor
    }
}
