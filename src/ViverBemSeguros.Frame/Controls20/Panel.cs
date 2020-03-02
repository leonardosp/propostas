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
    public class Panel : System.Web.UI.WebControls.Panel
    {
        System.Web.UI.WebControls.HiddenField hdnSelectedTab;
        private PanelHeaderTemplateCollection panelheadertemplatecollection = null;
        private PanelContentTemplateCollection panelcontenttemplatecollection = null;
        private PanelDefaultContentTemplate paneldefaultcontenttemplate = null;
        private PanelHeaderButtonCollection panelheaderbuttoncollection = null;



        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PanelDefaultContentTemplate PanelDefaultContentTemplate
        {
            get
            {
                if (paneldefaultcontenttemplate == null)
                {
                    paneldefaultcontenttemplate = new PanelDefaultContentTemplate();
                }
                return paneldefaultcontenttemplate;
            }
        }
        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PanelContentTemplateCollection PanelContentTemplateCollection
        {
            get
            {
                if (panelcontenttemplatecollection == null)
                {
                    panelcontenttemplatecollection = new PanelContentTemplateCollection();
                }
                return panelcontenttemplatecollection;
            }
        }
        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PanelHeaderTemplateCollection PanelHeaderTemplateCollection
        {
            get
            {
                if (panelheadertemplatecollection == null)
                {
                    panelheadertemplatecollection = new PanelHeaderTemplateCollection();
                }
                return panelheadertemplatecollection;
            }
        }
        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public PanelHeaderButtonCollection PanelHeaderButtonCollection
        {
            get
            {
                if (panelheaderbuttoncollection == null)
                {
                    panelheaderbuttoncollection = new PanelHeaderButtonCollection();
                }
                return panelheaderbuttoncollection;
            }
        }


        private string title;
        private bool btnClose;
        private bool btnMinimize;
        private bool initMinimized = false;
        private string keypressbutton = null;


        public string KeyPressButton
        {
            get { return keypressbutton; }
            set { keypressbutton = value; }
        }

        private bool showheader = true;

        public bool ShowHeader
        {
            get { return showheader; }
            set { showheader = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public bool BtnClose
        {
            get { return btnClose; }
            set { btnClose = value; }
        }
        public bool BtnMinimize
        {
            get { return btnMinimize; }
            set { btnMinimize = value; }
        }
        public bool InitMinimized
        {
            get { return initMinimized; }
            set { initMinimized = value; }
        }
        public string CssBoxContent { get; set; }
        public FontAwesomeIcons HeaderIcon { get; set; }
        public FontAwesomeType HeaderIconType { get; set; }
        public PanelTemplate Template { get; set; }
        public PanelStyle PanelStyle { get; set; }

        protected override void OnInit(EventArgs e)
        {
            if (paneldefaultcontenttemplate != null)
                this.Controls.Add(paneldefaultcontenttemplate);

            foreach (var item in PanelContentTemplateCollection)
                this.Controls.Add(item);

            if (btnMinimize)
                PanelHeaderButtonCollection.Add(new PanelHeaderButton() { Icon = initMinimized ? FontAwesomeIcons.ChevronDown : FontAwesomeIcons.ChevronUp, CssClass = "btn-minimize" });

            if (btnClose)
                PanelHeaderButtonCollection.Add(new PanelHeaderButton() { Icon = FontAwesomeIcons.Close, CssClass = "btn-close" });

            if (PanelHeaderTemplateCollection != null && PanelHeaderTemplateCollection.Count > 1)
            {
                hdnSelectedTab = new System.Web.UI.WebControls.HiddenField();
                hdnSelectedTab.ID = "hdnSelectedTab";
                Controls.Add(hdnSelectedTab);
            }

            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (hdnSelectedTab != null && Page.Request.Form[hdnSelectedTab.UniqueID] != null)
            {
                hdnSelectedTab.Value = Page.Request.Form[hdnSelectedTab.UniqueID];
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (Template == PanelTemplate.Default)
            {
                string keypress = "";
                string tabindex ="" ;
                string outline = "";

                if (!string.IsNullOrEmpty(keypressbutton))
                {
                    keypress = "onkeypress=\"javascript: var x= event.keyCode; if (x==13) {$('#" + keypressbutton + "').trigger('click')}  \"";
                    tabindex = "tabindex='1'";
                    outline = "style='outline:none;'";
                }

                writer.Write("<div class='boxPanel {0}' id='{1}' {2} {3} {4}>", PanelStyle.GetStringValueClass(), ID, keypress,tabindex, outline);
                string idFirstTab = string.Empty;

                if (ShowHeader)
                {
                    writer.Write("<div class='boxPanel-header'>");
                    writer.Write("<h2 class='h2Panel'>");

                    if (HeaderIcon != FontAwesomeIcons.NotSet)
                        writer.Write("<i class=\"{0} {1}\"></i>", HeaderIconType.GetStringValueClass(), HeaderIcon.GetStringValueClass());

                    writer.Write("<span style='float: right;'>{0}</span>", title);
                    writer.Write("</h2>");

                    writer.Write("<div class='boxPanel-icon'>");

                    foreach (var button in PanelHeaderButtonCollection)
                    {
                        writer.Write("<a href='javascript:{0}' class='{1}'><i class='{2} {3}'></i></a>",
                            string.IsNullOrWhiteSpace(button.ClientClick) ? "void(0)" : button.ClientClick.Replace("'", "\""),
                            button.CssClass,
                            button.IconType.GetStringValueClass(),
                            button.Icon.GetStringValueClass());
                    }

                    writer.Write("</div>");//boxPanel-icon


                    if (PanelHeaderTemplateCollection != null && PanelHeaderTemplateCollection.Count > 0)
                    {
                        hdnSelectedTab.RenderControl(writer);

                        writer.Write("<ul class='nav nav-tabs'>");

                        bool first = true;
                        foreach (var headerItem in PanelHeaderTemplateCollection)
                        {
                            string noty = "";
                            if (!string.IsNullOrWhiteSpace(headerItem.NotificationValue))
                                noty = "<span class='notification orange'>" + headerItem.NotificationValue + "</span>";

                            if (first && string.IsNullOrWhiteSpace(hdnSelectedTab.Value))
                            {
                                writer.Write(String.Format("<li class='active'><a href='#{0}' data-toggle='tab' onclick='$(\"#{3}\").val(\"{0}\")'>{1}</a>{2}</li>", headerItem.TabContentID, headerItem.Name, noty, hdnSelectedTab.ClientID));
                                first = false;
                                idFirstTab = headerItem.TabContentID;
                                hdnSelectedTab.Value = headerItem.TabContentID;
                            }
                            else if (first && !string.IsNullOrWhiteSpace(hdnSelectedTab.Value) && hdnSelectedTab.Value == headerItem.TabContentID)
                            {
                                writer.Write(String.Format("<li class='active'><a href='#{0}' data-toggle='tab' onclick='$(\"#{3}\").val(\"{0}\")'>{1}</a>{2}</li>", headerItem.TabContentID, headerItem.Name, noty, hdnSelectedTab.ClientID));
                                first = false;
                                idFirstTab = headerItem.TabContentID;
                            }
                            else
                                writer.Write(String.Format("<li><a href='#{0}' data-toggle='tab' onclick='$(\"#{3}\").val(\"{0}\")'>{1}</a>{2}</li>", headerItem.TabContentID, headerItem.Name, noty, hdnSelectedTab.ClientID));

                        }

                        writer.Write("</ul>");
                    }


                    writer.Write("</div>");//boxPanel-header
                }

                writer.Write(string.Format("<div class='boxPanel-content' {0}>", initMinimized ? "style='display:none;'" : "style='display:block;'"));

                if (PanelContentTemplateCollection != null && PanelContentTemplateCollection.Count > 0)
                {
                    writer.Write("<div class='tab-content'>");

                    foreach (var item in PanelContentTemplateCollection)
                    {
                        if (item.ID == idFirstTab)
                            writer.Write(String.Format("<div class='tab-pane active' id='{0}'>", item.ID));
                        else
                            writer.Write(String.Format("<div class='tab-pane' id='{0}'>", item.ID));

                        item.RenderControl(writer);

                        writer.Write("</div>");
                    }

                    writer.Write("</div>");
                }

                if (PanelDefaultContentTemplate != null)
                {
                    PanelDefaultContentTemplate.RenderControl(writer);
                }
                writer.Write("</div>");//box-content
                writer.Write("</div>");//box-panel
            }
            else
            {
                if (PanelDefaultContentTemplate != null)
                {
                    PanelDefaultContentTemplate.RenderControl(writer);
                }
            }
        }
    }
    [Serializable]
    public class PanelHeaderButton : StateManagedItem
    {
        public FontAwesomeIcons Icon { get; set; }
        public FontAwesomeType IconType { get; set; }
        public string ClientClick { get; set; }
        public string CssClass { get; set; }
    }
    [Serializable]
    public class PanelHeaderButtonCollection : List<PanelHeaderButton>
    {

    }
    [Serializable]
    public class PanelHeaderTemplate : StateManagedItem
    {
        public string Name { get; set; }
        public string TabContentID { get; set; }
        public string NotificationValue
        {
            get
            {
                object o = ViewState["NotificationValue"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["NotificationValue"] = value; }
        }

    }
    [Serializable]
    public class PanelHeaderTemplateCollection : List<PanelHeaderTemplate>
    {

    }
    [Serializable]
    [ParseChildren(false), PersistChildren(false)]
    public class PanelContentTemplate : StateManagedCompositeControl
    {
        public string ID { get; set; }
    }
    [Serializable]
    public class PanelContentTemplateCollection : List<PanelContentTemplate>
    {

    }
    [Serializable]
    [ParseChildren(false), PersistChildren(false)]
    public class PanelDefaultContentTemplate : StateManagedCompositeControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }

    public enum PanelTemplate
    {
        Default,
        None
    }

    public enum PanelStyle
    {
        [StringValueClass("")]
        Default,
        [StringValueClass("box-danger")]
        Danger,
        [StringValueClass("box-info")]
        Info,
        [StringValueClass("box-success")]
        Success,
        [StringValueClass("box-warning")]
        Warning
    }
}
