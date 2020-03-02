using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Celebre.Web.Controls
{
    public enum TypeControlForm
    {
        Default,
        Inline,
        None
    }

    public enum FontAwesomeIcons
    {
        NotSet,
        [StringValueClass("fa-plus"), StringValueText("Adicionar")]
        Add,
        [StringValueClass("fa-ambulance")]
        Ambulance,
        [StringValueClass("fa-angle-double-left")]
        AngleDoubleLeft,
        [StringValueClass("fa-angle-double-right")]
        AngleDoubleRight,
        [StringValueClass("fa-arrow-left")]
        ArrowLeft,
        [StringValueClass("fa-arrow-right")]
        ArrowRight,
        [StringValueClass("fa-reply"), StringValueText("Voltar")]
        Back,
        [StringValueClass("fa-balance-scale")]
        BalanceScale,
        [StringValueClass("fa-ban")]
        Ban,
        [StringValueClass("fa-barcode")]
        Barcode,
        [StringValueClass("fa-bars")]
        Bars,
        [StringValueClass("fa-bolt")] 
        Bolt,
        [StringValueClass("fa-briefcase")]
        Briefcase,
        [StringValueClass("fa-building")]
        Building,
        [StringValueClass("fa-bullhorn")]
        Bullhorn,
        [StringValueClass("fa-calculator")]
        Calculator,
        [StringValueClass("fa-calendar-alt")]
        Calendar,
        [StringValueClass("fa-car")]
        Car,
        [StringValueClass("fa-chart-bar")]
        ChartBar,
        [StringValueClass("fa-check"), StringValueText("Aceitar")]
        Check,
        [StringValueClass("fa-chevron-down")]
        ChevronDown,
        [StringValueClass("fa-chevron-up")]
        ChevronUp,
        [StringValueClass("fa-credit-card")]
        CreditCard,
        [StringValueClass("fa-clock")]
        Clock,
        [StringValueClass("fa-times"), StringValueText("Fechar")]
        Close,
        [StringValueClass("fa-cogs")]
        Cogs,
        [StringValueClass("fa-comment-dots")]
        Commenting,
        [StringValueClass("fa-comments")]
        Comments,
        [StringValueClass("fa-copy")]
        Copy,
        [StringValueClass("fa-cubes")]
        Cubes,
        [StringValueClass("fa-trash"), StringValueText("Excluir")]
        Delete,
        [StringValueClass("fa-dollar-sign")]
        Dollar,
        [StringValueClass("fa-cloud-download-alt"), StringValueText("Download")]
        Download,
        [StringValueClass("fa-edit"), StringValueText("Editar")]
        Edit,
        [StringValueClass("fa-envelope")]
        EnvelopeClosed,
        [StringValueClass("fa-envelope-open")]
        EnvelopeOpen,        
        [StringValueClass("fa-eraser")]
        Eraser,
        [StringValueClass("fa-file-excel"), StringValueText("Excel")]
        Excel,
        [StringValueClass("fa-exchange-alt")]
        Exchange,
        [StringValueClass("fa-exclamation")]
        Exclamation,
        [StringValueClass("fa-exclamation-triangle")]
        ExclamationTriangle,        
        [StringValueClass("fa-external-link-alt")]
        ExternalLink,
        [StringValueClass("fa-eye")]
        Eye,
        [StringValueClass("fa-fast-forward")]
        FastForward,
        [StringValueClass("fa-copy")]
        Files,
        [StringValueClass("fa-file-alt")]
        FileText,        
        [StringValueClass("fa-filter")]
        Filter,
        [StringValueClass("fa-search"), StringValueText("Pesquisar")]
        Find,
        [StringValueClass("fa-flag-checkered")]
        FlagCheckered,
        [StringValueClass("fa-folder-open")]
        FolderOpen,
        [StringValueClass("fa-info")]
        Info,
        [StringValueClass("fa-key")]
        Key,
        [StringValueClass("fa-link")]
        Link,
        [StringValueClass("fa-list")]
        List,
        [StringValueClass("fa-minus"), StringValueText("Remover")]
        Minus,
        [StringValueClass("fa-mobile-alt")]
        Mobile,
        [StringValueClass("fa-money-bill-alt")]
        Money,
        [StringValueClass("fa-file-alt"), StringValueText("Novo")]
        New,
        [StringValueClass("fa-question"), StringValueText("Ajuda")]
        Help,
        [StringValueClass("fa-object-group")]
        ObjectGroup,
        [StringValueClass("fa-paper-plane")]
        PaperPlane,
        [StringValueClass("fa-pencil-alt")]
        Pencil,
        [StringValueClass("fa-phone")]
        Phone,
        [StringValueClass("fa-print"), StringValueText("Imprimir")]
        Print,
        [StringValueClass("fa-random")]
        Random,
        [StringValueClass("fa-sync-alt"), StringValueText("Atualizar")]
        Refresh,
        [StringValueClass("fa-minus")]
        Remove,
        [StringValueClass("fa-reply")]
        Reply,
        [StringValueClass("fa-retweet")]
        Retweet,
        [StringValueClass("fa-road")]
        Road,
        [StringValueClass("fa-rss")]
        Rss,
        [StringValueClass("fa-save"), StringValueText("Salvar")]
        Save,
        [StringValueClass("fa-envelope"), StringValueText("Enviar")]
        Send,
        [StringValueClass("fa-shopping-basket")]
        ShoppingBasket,
        [StringValueClass("fa-stop-circle")]
        Stop,
        [StringValueClass("fa-sync-alt")]
        Sync,
        [StringValueClass("fa-tag")]
        Tag,
        [StringValueClass("fa-tasks")]
        Tasks,
        [StringValueClass("fa-th-list")]
        ThList,
        [StringValueClass("fa-thumbs-up")]
        ThumbsUp,
        [StringValueClass("fa-truck")]
        Truck,
        [StringValueClass("fa-undo")]
        Undo,
        [StringValueClass("fa-university")]
        University,
        [StringValueClass("fa-unlink")]
        Unlink,
        [StringValueClass("fa-cloud-upload-alt"), StringValueText("Upload")]
        Upload,
        [StringValueClass("fa-dollar-sign")]
        Usd,
        [StringValueClass("fa-user")]
        User,
        [StringValueClass("fa-users")]
        Users,
        [StringValueClass("fa-wifi")]
        Wifi,
        [StringValueClass("fa-wrench")]
        Wrench
    }
    public enum FontAwesomeType
    {
        [StringValueClass("fas")]
        Solid,
        [StringValueClass("far")]
        Regular,
        [StringValueClass("fal")]
        Light
    }


    [Serializable]
    public static class ComponentUtils
    {
        public static string GetStringValueClass(this Enum value)
        {
            var attribs = getParam(value);
            return attribs.Length > 0 ? attribs[0].StringValueClass : string.Empty;
        }        

        static StringValueClassAttribute[] getParam(Enum paramEnum)
        {
            Type type = paramEnum.GetType();
            FieldInfo fieldInfo = type.GetField(paramEnum.ToString());
            return fieldInfo.GetCustomAttributes(typeof(StringValueClassAttribute), false) as StringValueClassAttribute[];
        }

        
        public static string RenderControlOpen(string label, string controlID, TypeControlForm typeControl, bool multipleControls = true, bool obrigatoriovazio = false, string tooltip = "")
        {
            string control = string.Empty;
            if (typeControl == TypeControlForm.Default)
            {
                if (!obrigatoriovazio)
                {
                    control = string.Format(@"
                    <div class='form-group'>
                        <label class='control-label' for='{0}'{1}>{2}{3}</label>",
                            controlID,                            
                            string.IsNullOrWhiteSpace(tooltip) ? "" : string.Format(" data-toggle='tooltip' data-placement='bottom' title='{0}'", tooltip),
                            string.IsNullOrWhiteSpace(tooltip) ? "" : "<i class='fas fa-question-circle'></i>",
                            label);
                }
                else
                {
                    control = string.Format(@"
                    <div class='form-group has-error'>
                        <label class='control-label' for='{0}'{1}>{2}{3}</label>",
                            controlID,
                            string.IsNullOrWhiteSpace(tooltip) ? "" : string.Format(" data-toggle='tooltip' data-placement='bottom' title='{0}'", tooltip),
                            string.IsNullOrWhiteSpace(tooltip) ? "" : "<i class='fas fa-question-circle'></i>",
                            label);
                }

                if (multipleControls)
                    control += string.Format(@"<div class='controls'><div class='input-group col-sm-12'>");
            }
            else if (typeControl == TypeControlForm.Inline)
            {
                control = string.Format(@"
                <div class='form-group'>
                    <label class='col-sm-auto control-label verticalAlignCenter' for='{0}'>{1}</label>
                    <div class='col-sm-auto verticalAlignCenter'>
                        ", controlID, label);

            }
            return control;
        }

        public static string RenderControlClose(string help, TypeControlForm typeControl, bool multipleControls = true)
        {
            string control = "";
            if (control.Length > 0)
                control = "<span class='help-block'>" + help + "<span>";

            if (typeControl == TypeControlForm.Default)
            {
                control += "</div>";

                if (multipleControls)
                    control += "</div></div>";
            }
            else if (typeControl == TypeControlForm.Inline)
                control += "</div></div>";

            return control;
        }

        public static void CarregaCampos(Object item, Control parent)
        {
            foreach (var property in item.GetType().GetProperties())
            {
                if (property.CanRead)
                {
                    try
                    {
                        FindControl(parent, property, item);
                        //var control = FindControl(parent, property, item);
                        //var tipo = control.GetType().Name;
                        //switch (tipo)
                        //{
                        //    case "TextBox":
                        //        ((TextBox)control).Text = property.GetValue(item, null).ToString();
                        //        break;

                        //    case "DropDownList":
                        //        ((DropDownList)control).SelectedValue = property.GetValue(item, null).ToString();
                        //        break;
                        //}
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static void ClearAllFields(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                switch (control.GetType().Name)
                {
                    case "TextBox":
                        ((TextBox)control).Text = "";
                        break;
                    //case "DropDownList":
                    //    ((DropDownList)control).ClearItens();
                    //    break;
                    case "TableView":
                        ((TableView)control).SetDataSource(new List<object>());
                        ((TableView)control).DataBind();
                        break;
                    case "HiddenField":
                        ((HiddenField)control).Value = "";
                        break;
                }

                if (control.Controls.Count > 0)
                {
                    ClearAllFields(control);
                }
            }

            //if (ViewState.Keys != null)
            //{
            //    foreach (var key in ViewState.Keys)
            //    {
            //        ViewState[key.ToString()] = null;
            //    }    
            //}

        }

        private static void FindControl(Control parent, PropertyInfo prop, Object item)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox)
                {
                    var ctrl = ((TextBox)control);
                    if (ctrl.DataFieldName != null)
                    {
                        if (ctrl.DataFieldName.Equals(prop.Name))
                        {
                            ctrl.Text = prop.GetValue(item, null).ToString();
                        }
                    }
                }
                else if (control is DropDownList)
                {
                    var ctrl = ((DropDownList)control);
                    if (ctrl.DataFieldName != null)
                    {
                        if (ctrl.DataFieldName.Equals(prop.Name))
                        {
                            ctrl.SelectedValue = prop.GetValue(item, null).ToString();
                        }
                    }
                }
                else if (control is HiddenField)
                {
                    var ctrl = ((HiddenField)control);
                    if (ctrl.ID.Equals(prop.Name))
                    {
                        ctrl.Value = prop.GetValue(item, null).ToString();
                    }
                }
                if (control.HasControls())
                {
                    FindControl(control, prop, item);
                }
            }
        }


        public static string CompactObject(object obj)
        {
            LosFormatter formatter = new LosFormatter();
            using (StringWriter writer = new StringWriter())
            {
                formatter.Serialize(writer, obj);
                string viewStateString = writer.ToString();
                byte[] bytes = Convert.FromBase64String(viewStateString);
                // COMPACTAR VIEWSTATE
                bytes = CompactObjectByte(bytes);

                return Convert.ToBase64String(bytes);
            }
        }
        public static object DescompactObject(string obj)
        {
            LosFormatter los = new LosFormatter();

            byte[] bytes = Convert.FromBase64String(obj.ToString());

            return los.Deserialize(Convert.ToBase64String(DescompactObjectByte(bytes)));
        }
        private static byte[] CompactObjectByte(byte[] bytes)
        {
            using (MemoryStream MSsaida = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(MSsaida, CompressionMode.Compress, true))
                    gzip.Write(bytes, 0, bytes.Length);

                return MSsaida.ToArray();
            }
        }
        private static byte[] DescompactObjectByte(byte[] bytes)
        {
            using (MemoryStream MSentrada = new MemoryStream())
            {
                MSentrada.Write(bytes, 0, bytes.Length);
                MSentrada.Position = 0;
                using (GZipStream gzip = new GZipStream(MSentrada,
                                  CompressionMode.Decompress, true))
                {
                    using (MemoryStream MSsaida = new MemoryStream())
                    {
                        byte[] buffer = new byte[64];
                        int leitura = -1;
                        leitura = gzip.Read(buffer, 0, buffer.Length);
                        while (leitura > 0)
                        {
                            MSsaida.Write(buffer, 0, leitura);
                            leitura = gzip.Read(buffer, 0, buffer.Length);
                        }
                        return MSsaida.ToArray();
                    }
                }
            }
        }
    }
    [Serializable]
    public class StringValueClassAttribute : Attribute
    {
        public string StringValueClass { get; protected set; }


        public StringValueClassAttribute(string value)
        {
            this.StringValueClass = value;
        }
    }
    
    [Serializable]
    public class RowClickEventArgs : EventArgs
    {
        public RowClickEventArgs(string value)
        {
            if (value.Contains("*"))
            {
                string[] values = value.Split('*');

                this.Value = values[0];
                this.Column = values[1];
            }
            else
            {
                this.Value = value;
                this.Column = "";
            }


        }

        public String Value { get; set; }
        public String Column { get; set; }

    }    
}
