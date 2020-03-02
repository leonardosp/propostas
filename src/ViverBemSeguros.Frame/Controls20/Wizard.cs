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
    public class Wizard : StateManagedCompositeControl
    {

        private WizardTemplateCollection wizardTemplateCollection = null;
        public bool EnableServerNextClick { get; set; }

        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public WizardTemplateCollection WizardTemplateCollection
        {
            get { return wizardTemplateCollection ?? (wizardTemplateCollection = new WizardTemplateCollection()); }
        }

        private WizardTemplate wizardTemplate = null;

        [MergableProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public WizardTemplate WizardTemplate
        {
            get { return wizardTemplate ?? (wizardTemplate = new WizardTemplate()); }
        }

        protected override void OnInit(EventArgs e)
        {
            HiddenField hiddenField = new HiddenField();
            hiddenField.ID = "hdnTemplateActive";

            if (Page.IsPostBack)
                hiddenField.Value = Page.Request.Form[this.ID + "$hdnTemplateActive"].ToString();

            this.Controls.Add(hiddenField);

            int count = 1;
            bool foundActive = false;
            foreach (var item in WizardTemplateCollection)
            {
                item.Index = count;

                if (EnableServerNextClick && item.HasServerClick())
                {
                    Button btnNext = new Button();
                    btnNext.ID = item.ID + "btnNextStep";
                    btnNext.CssClass = "btn-next";
                    btnNext.ButtonIcon = FontAwesomeIcons.ArrowRight;
                    btnNext.Text = "Próximo";
                    btnNext.ButtonStyle = ButtonStyle.Success;
                    btnNext.Click += item.OnNextClick;
                    btnNext.DataField = item.ID;
                    this.Controls.Add(btnNext);
                }

                if (String.IsNullOrWhiteSpace(hiddenField.Value))
                {
                    if (item == WizardTemplateCollection.First())
                    {
                        item.Etapa = EnumWizardEtapa.Ativo;
                    }
                }
                else
                {
                    if (item.ID == hiddenField.Value.Replace("#", ""))
                    {
                        item.Etapa = EnumWizardEtapa.Ativo;
                        foundActive = true;
                    }
                    else
                    {
                        if (!foundActive)
                            item.Etapa = EnumWizardEtapa.Concluido;
                    }
                }
                count++;
            }


            foreach (var item in WizardTemplateCollection)
                this.Controls.Add(item);

            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div class='wizard' data-control='Wizard'>");

            HiddenField hiddenField = (HiddenField)this.FindControl("hdnTemplateActive");
            hiddenField.RenderControl(writer);

            writer.Write("<div class='header'>");
            writer.Write("<ul class='steps'>");


            foreach (var item in WizardTemplateCollection)
            {
                writer.Write("<li data-target='#{0}'{1} data-step='{2}' data-post='{5}' data-next-click-fn='{6}'>{3}{4}</li>",
                    item.ID,
                    item.Etapa == EnumWizardEtapa.Ativo ? " class='active'" : item.Etapa == EnumWizardEtapa.Concluido ? " class='completed'" : "",
                    item.Index,
                    item != WizardTemplateCollection.Last() ? "<span class='chevron'></span>" : "",
                    item.NoEtapa,
                    EnableServerNextClick && item.HasServerClick() ? ((Button)this.FindControl(item.ID + "btnNextStep")).UniqueID : "",
                    item.OnClientNextClick == null ? "" : item.OnClientNextClick.Replace("'", "\""));


            }
            writer.Write("</ul>");

            writer.Write("<div class='actions'>");
            writer.Write("<button type='button' class='btn btn-prev'> <i class='fa fa-arrow-left'></i> Anterior</button>");


            writer.Write("<button type='button' class='btn btn-success btn-next' data-text='Próximo' data-last='Finalizar'>Próximo<i class='fa fa-arrow-right'></i></button>");


            writer.Write("</div>");
            writer.Write("</div>");

            writer.Write("<div class='steps-content'>");

            foreach (var wizardTemplate in WizardTemplateCollection)
            {
                writer.Write("<div class='step-pane{1}' id='{0}' data-step='{2}'>",
                    wizardTemplate.ID,
                    wizardTemplate.Etapa == EnumWizardEtapa.Ativo ? " active" : "",
                    wizardTemplate.Index);

                wizardTemplate.RenderControl(writer);

                writer.Write("</div>");
            }

            writer.Write("</div>");

            writer.Write("</div>");
        }

        public void SetActiveStep(string idStep)
        {
            ((HiddenField)this.FindControl("hdnTemplateActive")).Value = idStep.Contains("#") ? idStep : "#" + idStep;

            idStep = idStep.Replace("#", "");

            bool foundActive = false;
            WizardTemplateCollection.ForEach(wiz =>
            {
                if (wiz.ID == idStep)
                {
                    wiz.Etapa = EnumWizardEtapa.Ativo;
                    foundActive = true;
                }
                else if (!foundActive)
                    wiz.Etapa = EnumWizardEtapa.Concluido;
            });
        }

        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/wizard.min.js") + "'></script>"));

            return listScriptsAndStyles;
        }
    }
    [Serializable]
    [ParseChildren(false), PersistChildren(false)]
    public class WizardTemplate : StateManagedCompositeControl
    {
        public String NoEtapa { get; set; }
        public EnumWizardEtapa Etapa { get; set; }
        public string NoPaginaWeb { get; set; }
        internal int Index { get; set; }
        public string OnClientNextClick { get; set; }

        [Browsable(true)]
        public event EventHandler NextClick;
        public delegate void EventHandler(Button sender, EventArgs e);
        internal virtual void OnNextClick(object sender, EventArgs e)
        {
            if (this.NextClick != null && Etapa == EnumWizardEtapa.Ativo)
                NextClick((Button)sender, e);
        }
        public bool HasServerClick()
        {
            return this.NextClick != null;
        }
    }
    [Serializable]
    public class WizardTemplateCollection : List<WizardTemplate>
    {

    }

    public enum EnumWizardEtapa
    {
        Ativo = 1,
        Concluido = 2
    }
}
