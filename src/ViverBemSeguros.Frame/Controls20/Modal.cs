using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using Celebre.Web.Pages;
using Celebre.Web.Utils;
using System.Reflection;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class Modal : StateManagedCompositeControl
    {
        private BodyCollection body = null;
        private FooterCollection footer = null;
        private bool btnFechar = true;
        System.Web.UI.WebControls.HiddenField hdnOpenedModal;
        private bool btnFecharSemPostBack = true;
        private bool alteradoInternamente = false;

        [Browsable(true)]
        public event EventHandler BtnFecharClick;

        [Browsable(true)]
        public event EventHandler Showing;


        public bool BtnFechar
        {
            get { return btnFechar; }
            set { btnFechar = value; }
        }

        //public bool BtnFecharSemPostBack
        //{
        //    get { return btnFecharSemPostBack; }
        //    set { btnFecharSemPostBack = value; }
        //}


        public bool BtnFecharSemPostBack
        {
            get
            {
                if (ViewState[this.ID + "BtnFecharSemPostBack"] == null)
                    return false;
                else
                {
                    bool status;
                    bool.TryParse(ViewState[this.ID + "BtnFecharSemPostBack"].ToString(), out status);
                    return status;
                }

            }
            set { ViewState[this.ID + "BtnFecharSemPostBack"] = value; }
        }
        public string DefaultButtonID
        {
            get
            {
                if (ViewState[ID + "DefaultButtonID"] == null)
                    return "";
                else
                    return ViewState[ID + "DefaultButtonID"].ToString();
            }
            set { ViewState[ID + "DefaultButtonID"] = value; }
        }


        /// <summary>
        ///     Título apresentado no Header do Modal
        /// </summary>
        public string Title
        {
            get
            {
                if (ViewState[this.ID + "Title"] == null)
                    return "";
                else
                    return ViewState[this.ID + "Title"].ToString();
            }
            set { ViewState[this.ID + "Title"] = value; }
        }


        private bool AbertoSemPostBack;

        public bool Aberto
        {
            get
            {
                if (ViewState[this.ID + "Aberto"] == null)
                    return false;
                else
                    return Boolean.Parse(ViewState[this.ID + "Aberto"].ToString());
            }
            set
            {
                ViewState[this.ID + "Aberto"] = value;
            }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public BodyCollection Body
        {
            get
            {
                if (body == null)
                {
                    body = new BodyCollection();
                }
                return body;
            }
            set { body = value; }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public FooterCollection Footer
        {
            get
            {
                if (footer == null)
                {
                    footer = new FooterCollection();
                }
                return footer;
            }
            set { footer = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            hdnOpenedModal = new System.Web.UI.WebControls.HiddenField();
            hdnOpenedModal.ID = "OpenedModal";
            hdnOpenedModal.Value = "false";
            Controls.Add(hdnOpenedModal);

            Button btn = new Button();

            if (BtnFecharSemPostBack)
            {

                btn.Click += null;
                AbertoSemPostBack = false;

                string ctrlName = this.Page.Request.Params.Get("__EVENTTARGET");

                if (ctrlName != null)
                {
                    if (ctrlName.Contains(this.ID + "$"))
                    {
                        AbertoSemPostBack = true;
                    }
                }


                btn.OnClientClick = string.Format("$('#{0}').hide();$('.modal-backdrop').remove();$('#{1}').val(false);return false;", this.ID, hdnOpenedModal.ClientID);
            }


            else if (BtnFecharClick == null)
                btn.Click += new System.EventHandler(btn_Click);
            else
                btn.Click += BtnFecharClick;

            btn.CssClass = "close";
            btn.Attributes.Add("data-dismiss", "modal");
            btn.Attributes.Add("aria-hidden", "true");

            if (!BtnFecharSemPostBack)
            {
                btn.Attributes.Add("runat", "server");
            }

            btn.Text = "x";
            btn.ID = "btnCloseModal";

            Controls.Add(btn);

            Controls.Add(body);

            if (footer != null)
                Controls.Add(footer);


            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!alteradoInternamente)
            {
                if (Page.Request.Form[hdnOpenedModal.UniqueID] != null)
                {
                    hdnOpenedModal.Value = Page.Request.Form[hdnOpenedModal.UniqueID].ToLower();
                }

                if (hdnOpenedModal.Value.ToLower() == "true")
                    Aberto = AbertoSemPostBack = true;
                else
                    Aberto = AbertoSemPostBack = false;
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {

            if (Aberto || (BtnFecharSemPostBack && AbertoSemPostBack))
                writer.Write("<div  class='modal fade in' style='display:block;' id='" + ID + "' tabindex='-1' aria-labelledby='" + ID + "title' role='dialog' data-keyboard='true'>");
            else
                writer.Write("<div  class='modal fade' id='" + ID + "' tabindex='-1' aria-labelledby='" + ID + "title' role='dialog' data-keyboard='true'>");

            if (!this.Width.IsEmpty && this.Width.Type == System.Web.UI.WebControls.UnitType.Percentage)
                writer.Write("<div class='modal-dialog' style='width:{0}%;' role='document'>", this.Width.Value);
            else if (!this.Width.IsEmpty && this.Width.Value < 590)
                writer.Write("<div class='modal-dialog' style='width:590px;' role='document'>");
            else if (!this.Width.IsEmpty)
                writer.Write(string.Format("<div class='modal-dialog' style='width:{0}px;margin-left:-{1}px;' role='document'>", this.Width.Value, (this.Width.Value - 590) / 2));
            else
                writer.Write("<div class='modal-dialog' role='document'>");

            if (!string.IsNullOrWhiteSpace(DefaultButtonID))
            {
                Control btnDefault = null;

                if (Footer != null)
                    btnDefault = Footer.Controls.Cast<Control>().FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ID) && p.ID.Contains(DefaultButtonID));

                if (btnDefault == null)
                    btnDefault = Body.Controls.Cast<Control>().FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ID) && p.ID.Contains(DefaultButtonID));

                if (btnDefault != null)
                    writer.Write("<div class='modal-content' onkeypress='javascript:return WebForm_FireDefaultButton(event, \"{0}\")'>", ((Button)btnDefault).ClientID);
                else
                    writer.Write("<div class='modal-content'>");
            }
            else
                writer.Write("<div class='modal-content'>");

            if (BtnFechar || !string.IsNullOrWhiteSpace(Title))
            {
                writer.Write("<div class='modal-header'>");

                //Mudar para botão close RUNAT SERVER (Cadastrar Evento)
                if (BtnFechar)
                {
                    ((Button)this.Controls.Cast<Control>().ToList().Where(x => x.ID.Equals("btnCloseModal")).First()).RenderControl(writer);
                }
                if (!string.IsNullOrWhiteSpace(Title))
                    writer.Write("<h4 class='modal-title' id='" + this.ID + "title'>" + Title + "</h4>");

                writer.Write("</div>");
            }

            writer.Write("<div class='modal-body'>");
            hdnOpenedModal.RenderControl(writer);
            Body.RenderControl(writer);

            writer.Write("</div>");
            writer.Write(String.Format("<div class='modal-footer {0}'>", Footer.CssClass));

            foreach (Control item in Footer.Controls)
            {
                if (item is LiteralControl)
                {
                    ((LiteralControl)item).Text = ((LiteralControl)item).Text.Replace("data-dismiss=\"modal\"", string.Format("data-dismiss=\"modal\" aria-hidden=\"true\" onclick=\"$('#{0}').hide();$('.modal-backdrop').remove();$('#{1}').val('false');return false;\"", this.ID, hdnOpenedModal.ClientID));
                }
            }

            Footer.RenderControl(writer);

            writer.Write("</div>");

            writer.Write("</div>");
            writer.Write("</div>");
            writer.Write("</div>");

            if (BtnFecharSemPostBack)
            {
                if (AbertoSemPostBack)
                    writer.Write("<div class='modal-backdrop fade in' style='display: block;'></div>");

            }
            else
            {
                if (Aberto)
                    writer.Write("<div class='modal-backdrop fade in' style='display: block;'></div>");
            }


            if (BtnFecharSemPostBack)
            {
                this.ClearCachedClientID();
                this.Dispose();
                this.Close();
                Aberto = false;
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            Close();

        }


        public void Show()
        {
            Aberto = true;
            AbertoSemPostBack = true;

            this.Attributes["data-closed"] = "false";
            hdnOpenedModal.Value = "true";
            if (Showing != null)
                Showing(this, EventArgs.Empty);
        }

        public void Close()
        {
            Aberto = AbertoSemPostBack = false;
            hdnOpenedModal.Value = "false";
            alteradoInternamente = true;
        }
    }
    [ParseChildren(false), PersistChildren(false)]
    [Serializable]
    public class BodyCollection : StateManagedCompositeControl
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion
    }

    [Serializable]
    [ParseChildren(false), PersistChildren(false)]
    public class FooterCollection : StateManagedCompositeControl
    {
        #region Fields
        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion
    }

}
