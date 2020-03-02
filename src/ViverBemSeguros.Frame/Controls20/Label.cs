using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class Label : System.Web.UI.WebControls.Label
    {
        private bool campoobrigatorio = true;
        private LabelType labeltype;

        public BadgeType BadgeStyle
        {
            get { return ViewState["LabelBadgeStyle"] == null ? BadgeType.NotSet : (BadgeType)ViewState["LabelBadgeStyle"]; }
            set { ViewState["LabelBadgeStyle"] = value; }
        }
        public bool CampoObrigatorio
        {
            get { return this.campoobrigatorio; }
            set { this.campoobrigatorio = value; }
        }

        public LabelType TipoLabel
        {
            get { return labeltype; }
            set { labeltype = value; }
        }
        public string LabelTitle { get; set; }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (campoobrigatorio)
                base.Text = "* " + base.Text;

            base.RenderContents(writer);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (BadgeStyle != BadgeType.NotSet)
                writer.Write($"<div class='label-badge {BadgeStyle.GetStringValueClass()}'>");

            if (!string.IsNullOrWhiteSpace(LabelTitle))
                writer.Write($"<div class='form-group'><label class='control-label' for='{ID}'>{LabelTitle}</label><div class='controls'>");

            if (labeltype != LabelType.NotSet)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, labeltype.GetStringValueClass());

            base.Render(writer);

            if (!string.IsNullOrWhiteSpace(LabelTitle))
                writer.Write($"</div></div>");

            if (BadgeStyle != BadgeType.NotSet)
                writer.Write($"</div>");
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            base.RenderChildren(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

        }
    }

    public enum LabelType
    {
        NotSet,
        [StringValueClass("label label-default")]
        Default,
        [StringValueClass("label label-primary")]
        Primary,
        [StringValueClass("label label-success")]
        Success,
        [StringValueClass("label label-info")]
        Info,
        [StringValueClass("label label-warning")]
        Warning,
        [StringValueClass("label label-danger")]
        Danger
    }
    public enum BadgeType
    {
        NotSet,
        [StringValueClass("badge badge-default")]
        Default,
        [StringValueClass("badge badge-primary")]
        Primary,
        [StringValueClass("badge badge-success")]
        Success,
        [StringValueClass("badge badge-info")]
        Info,
        [StringValueClass("badge badge-warning")]
        Warning,
        [StringValueClass("badge badge-danger")]
        Danger
    }
}
