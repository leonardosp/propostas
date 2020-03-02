using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Web;
using Celebre.Web.Utils;
using Celebre.Frame.FrameWork;
using System.IO;
using Celebre.Web.Util;
using static Celebre.FrameWork.Util;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class FileUpload : System.Web.UI.WebControls.FileUpload, IPostBackEventHandler
    {
        private int numeroMaximoArquivos = 5; // Define o número máximo de arquivos permitidos
        private int tamanhoMaximoArquivos = 5; // Em MB. Ex: tamanhoMaximoArquivos = 5 => 5MB
        private TiposArquivoCollection tiposArquivo = null; // Os tipos de arquivos permitidos
        private string updatepanelassociado;
        private string label;
        private bool montagroup = true;
        private List<Button> downloads;
        private bool campoobrigatorio;
        public bool CampoObrigatorio
        {
            get
            {
                return this.campoobrigatorio;
            }
            set
            {
                this.campoobrigatorio = value;
            }
        }
        public enumDisplayType DisplayType { get; set; }
        [Browsable(false)]
        public UploadFileS Files
        {
            get
            {
                if (ViewState[this.ID + "uploaded-files"] == null)
                {
                    ViewState[this.ID + "uploaded-files"] = new UploadFileS();
                }

                return (UploadFileS)ViewState[this.ID + "uploaded-files"];
            }
            set { ViewState[this.ID + "uploaded-files"] = value; }
        }
        public bool MontaGroup
        {
            get
            {
                return this.montagroup;
            }
            set
            {
                this.montagroup = value;
            }
        }
        public string Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }


        public bool NaoPossibilitarUpload
        {
            get
            {
                if (ViewState[this.ID + "NaoPossibilitarUpload"] == null)
                    return false;
                else
                {
                    bool status;
                    bool.TryParse(ViewState[this.ID + "NaoPossibilitarUpload"].ToString(), out status);
                    return status;
                }
            }

            set { ViewState[this.ID + "NaoPossibilitarUpload"] = value; }

        }


        public bool NaoPermitirExclusao
        {
            get
            {
                if (ViewState[this.ID + "NaoPermitirExclusao"] == null)
                    return false;
                else
                {
                    bool status;
                    bool.TryParse(ViewState[this.ID + "NaoPermitirExclusao"].ToString(), out status);
                    return status;
                }
            }

            set { ViewState[this.ID + "NaoPermitirExclusao"] = value; }

        }




        public bool HasFile { get { return Files.Count > 0; } }

        public string TituloPainel
        {
            get
            {
                if (ViewState[this.ID + "tituloPainel"] == null)
                    return "";
                else
                {

                    return ViewState[this.ID + "tituloPainel"].ToString();

                }
            }

            set { ViewState[this.ID + "tituloPainel"] = value; }

        }


        public string UpdatePanelAssociado
        {
            get { return updatepanelassociado; }
            set { updatepanelassociado = value; }
        }

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public TiposArquivoCollection TiposArquivo
        {
            get
            {
                if (tiposArquivo == null)
                {
                    tiposArquivo = new TiposArquivoCollection();
                }
                return tiposArquivo;
            }
        }

        [Browsable(true)]
        public event FileDownloadEventHandler FileDownload;
        public delegate void FileDownloadEventHandler(Object sender, FileDownloadEventArgs e);

        [Browsable(false)]
        private string ArrayOfFiles
        {
            get
            {
                object o = ViewState["ArrayOfFiles"];
                return o == null ? "" : (string)o;
            }
            set { ViewState["ArrayOfFiles"] = value; }
        }


        [Browsable(false)]
        private string ArrayOfKeys
        {
            get
            {
                object o = ViewState["ArrayOfKeys"];
                return o == null ? "" : (string)o;
            }
            set { ViewState["ArrayOfKeys"] = value; }
        }


        public int NumeroMaximoArquivos
        {
            get { return numeroMaximoArquivos; }
            set { numeroMaximoArquivos = value; }
        }

        public int TamanhoMaximoArquivos
        {
            get { return tamanhoMaximoArquivos; }
            set { tamanhoMaximoArquivos = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            // Cria um campo hidden para gerenciar os arquivos que foram inseridos/deletados
            var hdnFiles = FindControl(this.ClientID + "_hdnFiles");
            if (hdnFiles == null)
            {
                hdnFiles = new System.Web.UI.WebControls.HiddenField();
                hdnFiles.ID = this.ClientID + "_hdnFiles";
                ((System.Web.UI.WebControls.HiddenField)hdnFiles).Value = "";
                this.Controls.Add(hdnFiles);
            }

            var errorStatus = FindControl(this.ClientID + "_errorStatus");
            if (errorStatus == null)
            {
                errorStatus = new System.Web.UI.WebControls.HiddenField();
                errorStatus.ID = this.ClientID + "_errorStatus";
                ((System.Web.UI.WebControls.HiddenField)errorStatus).Value = "false";
                this.Controls.Add(errorStatus);
            }

            if (NaoPossibilitarUpload == false)
            {
                //Cria o Botão de Upload do Componentte
                var btnUpload = FindControl(this.ID + "_btnUpload");
                if (btnUpload == null)
                {
                    btnUpload = new Button();
                    btnUpload.ID = String.Format("{0}_btnUpload", this.ID);
                    ((Button)btnUpload).Attributes["Style"] = "display:none; background:transparent;";
                    ((Button)btnUpload).Click += btnUpload_Click;
                    this.Controls.Add(btnUpload);
                }
            }

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            if (NaoPossibilitarUpload == false)
            {

                Button btnUpload = ((Button)this.FindControl(String.Format("{0}_btnUpload", this.ID)));

                //Adiciona a Trigger ao UpdatePanel Associciado
                PostBackTrigger trigger = new PostBackTrigger();
                trigger.ControlID = btnUpload.UniqueID;

                ((UpdatePanel)Page.Form.FindControl(UpdatePanelAssociado)).Triggers.Add(trigger);
                ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnUpload);
            }



            if (((UpdatePanel)Page.Form.FindControl(UpdatePanelAssociado)) == null)
                throw new CelebreException("Update Panel vinculado ao Upload File não existe na página.");

            if (Page.Request.Form["__EVENTTARGET"] != null && Page.Request.Form["__EVENTTARGET"].Contains(String.Format("{0}_btnUpload", ID)))
                SetUploadedFiles();
            else if (Page.IsPostBack) //Alteração porque o armor não usa onprepareform (problemas com o page_load)
            {
                var hdnFiles = (System.Web.UI.WebControls.HiddenField)this.FindControl(this.ClientID + "_hdnFiles");
                if (hdnFiles.Value != "")
                {

                    var filesName = JsonConvert.DeserializeObject<List<JsonFileAux>>(hdnFiles.Value);
                    Files.RemoveAll(p => !filesName.Any(x => x.name == p.name.ReplaceSpecialCaracterFromFileName()));

                    CarregaFiles(Files);
                }

            }

            base.OnLoad(e);
        }

        private void carregaLinksDownload()
        {

            downloads = new List<Button>();

            for (int cont = 0; cont < Files.Count; cont++)
            {
                var btn = FindControl(Files[cont].name);
                if (btn == null)
                {
                    btn = new Button();
                    btn.ID = Files[cont].name;
                    ((Button)btn).Name = Files[cont].name;
                    ((Button)btn).Click += btnHdnDownloadClick;
                    ((Button)btn).Text = Files[cont].name;
                    ((Button)btn).Style.Add("display", "none");
                    ((Button)btn).CssClass += " btnDownloadItemFUP";

                    Controls.Add((Button)btn);
                }

                downloads.Add((Button)btn);

                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(btn);
            }

            if (downloads.Count > 0)
            {
                if (downloads[downloads.Count - 1].Text != null)
                {
                    Controls.Add(downloads[downloads.Count - 1]);

                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.RegisterPostBackControl(downloads[downloads.Count - 1]);
                }
            }
            else
            {
                Button btn = new Button();
                btn.Style.Add("display", "none");

                downloads.Add(btn);

                Controls.Add(downloads[0]);

                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(downloads[0]);
            }


        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //Remove os arquivos que foram exclusos da tela no viewstate
            var hdnFiles = (System.Web.UI.WebControls.HiddenField)this.FindControl(this.ClientID + "_hdnFiles");
            var filesName = JsonConvert.DeserializeObject<List<JsonFileAux>>(hdnFiles.Value);
            Files.RemoveAll(p => !filesName.Any(x => x.name == p.name.ReplaceSpecialCaracterFromFileName()));

            List<string> names = Files.ConvertAll(x => x.name).ToList();
            List<int> keys = Files.ConvertAll(x => x.key).ToList();

            ArrayOfFiles = ConvertToJSArray(JsonConvert.SerializeObject(ToJsonFile(names, keys)));

            writer.Write("<div class='boxPanel' style='background-color:#f5f5f5;'>");

            if (TituloPainel != "")
                writer.Write("<div class='boxPanel-header'><h2 class='h2Panel'><i class='fas fa-cloud-upload-alt'></i><span style='float: right;'>" + TituloPainel + "</span></h2></div>");
            else if (NaoPossibilitarUpload == false)
                writer.Write("<div class='boxPanel-header'><h2 class='h2Panel'><i class='fas fa-cloud-upload-alt'></i><span style='float: right;'>Clique para adicionar arquivos</span></h2></div>");
            else
                writer.Write("<div class='boxPanel-header'><h2 class='h2Panel'></h2></div>");

            writer.Write("<div class='boxPanel-content' style='padding:0;'>");





            if (NaoPossibilitarUpload == false)
            {
                this.Attributes.Add("data-control", "fileupload");
                writer.WriteLine("<div class='inputFileContainer'>");
            }
            else
                writer.WriteLine("<div class='inputFileContainer' style='background: #f5f5f5;cursor:default;'>");

            this.Attributes.Add("multiple", "");
            this.CssClass = "form-control " + this.CssClass;

            base.Render(writer);

            writer.WriteLine(@"<ul id='{0}_fupItens' class='dropzone' data-files='{1}' data-maxfiles='{2}' data-maxFileSize='{3}' data-supportedFiles='{4}' data-supportedFilesShow='{5}'>",
                this.ClientID,
                ArrayOfFiles,
                numeroMaximoArquivos,
                1048576 * tamanhoMaximoArquivos,
                MontaArrayTipos(tiposArquivo),
                MontaArrayErrorNames(tiposArquivo));

            for (int cont = 0; cont < Files.Count; cont++)
            {
                switch (DisplayType)
                {
                    case enumDisplayType.List:
                        writer.WriteLine(@"
                        <li style='display:block;'>
                            <i style='margin-right: 10px;' class='fas fa-file'></i>");

                        downloads[cont].Style.Add("display", "inline");
                        downloads[cont].Style.Add("background", "transparent");
                        downloads[cont].RenderControl(writer);

                        if (NaoPermitirExclusao == false)
                        {
                            writer.WriteLine(@"
                            <button style='margin-left: 5px;' type='button' class='btn btn-danger btn-xs fup_btnRemove' data-hdnupload='{0}' data-fileupload='{1}' data-value='{2}'>
                                <i class='fas fa-times'></i>
                            </button>", hdnFiles.ClientID, this.ClientID, Files[cont].name);
                        }

                        writer.WriteLine("</li>");
                        break;
                    case enumDisplayType.SmallIcons:
                    case enumDisplayType.LargeIcons:

                        string sizeClass = "";
                        if (DisplayType == enumDisplayType.SmallIcons)
                            sizeClass = "small";
                        else if (DisplayType == enumDisplayType.SmallIcons)
                            sizeClass = "large";

                        writer.WriteLine(@"<li>");

                        downloads[cont].RenderControl(writer);

                        string srcAttribute = "";
                        if (Files[cont].type.ToLower().Contains("image"))
                            srcAttribute = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(Imagem.MakeThumbnail(Files[cont].dados, 100, 100)));
                        else if (Files[cont].type.ToLower().Contains("pdf"))
                            srcAttribute = "../../Includes/Icons/FileUpload/PDFIcon.png";
                        else if (Files[cont].type.ToLower().Contains("application") && Files[cont].name.Split('.').Last().ToLower() == "zip")
                            srcAttribute = "../../Includes/Icons/FileUpload/ZIPIcon.png";
                        else if (Files[cont].type.ToLower().Contains("spreadsheet") || Files[cont].type.ToLower().Contains("application/vnd.ms-excel"))
                            srcAttribute = "../../Includes/Icons/FileUpload/ExcelIcon.png";
                        else if (Files[cont].type.ToLower().Contains("word"))
                            srcAttribute = "../../Includes/Icons/FileUpload/WordIcon.png";
                        else if (Files[cont].type.ToLower().Contains("xml"))
                            srcAttribute = "../../Includes/Icons/FileUpload/XMLIcon.png";
                        else if (Files[cont].type.ToLower().Contains("text"))
                            srcAttribute = "../../Includes/Icons/FileUpload/NotepadIcon.png";

                        string htmlImage = string.Format("<img data-dz-thumbnail alt='{0}' src='{1}' > ", Files[cont].name, srcAttribute);

                        writer.WriteLine(@"
                            <div class='dz-preview dz-file-preview dz-processing'> 
                                <div class='dz-details {4}'> 
                                    <div class='dz-filename'>
                                        <span data-dz-name>{0}</span>
                                    </div> 
                                    <div class='dz-size' data-dz-size>
                                        <strong>{1}</strong> KB
                                    </div> 
                                    {2}
                                </div> 
                                {3}
                            </div>",
                            Files[cont].name,
                            Files[cont].size,
                            htmlImage,
                            NaoPermitirExclusao ? "" : string.Format("<div class='dz-error-mark fup_btnRemove' data-hdnupload='{0}' data-fileupload='{1}' data-value='{2}'><i class='fas fa-times'></i></div>", hdnFiles.ClientID, this.ClientID, Files[cont].name),
                            sizeClass
                        );

                        writer.WriteLine("</li>");
                        break;
                    default:
                        break;
                }
            }

            writer.WriteLine("</ul></div></div>");

            if (NaoPossibilitarUpload == false)
                writer.WriteLine("<span class='observacaoFileUpload'>Limite de " + numeroMaximoArquivos + "  arquivos sendo que cada arquivo deve possuir o tamanho máximo de " + tamanhoMaximoArquivos + " MB</span>");
            else
                writer.WriteLine("<span class='observacaoFileUpload' style='display:none;'>Limite de " + numeroMaximoArquivos + "  arquivos sendo que cada arquivo deve possuir o tamanho máximo de " + tamanhoMaximoArquivos + " MB</span>");

            writer.WriteLine("</div>");
        }

        public void btnHdnDownloadClick(object sender, EventArgs e)
        {

            Button button = (Button)sender;

            UploadFile arq = Files.Where(x => x.name == button.Name).First();

            var response = this.Page.Response;
            // response.Clear();

            response.ContentType = "application/octet-stream";

            response.AddHeader("Content-Disposition", "attachment; filename=" + button.ID);
            response.AddHeader("Content-Length", arq.dados.Length.ToString());
            //response.Buffer = true;

            MemoryStream ms = new MemoryStream(arq.dados);
            ms.WriteTo(response.OutputStream);

            response.Flush();
            //response.End();
        }

        public void ClearSession()
        {
            Files = null;
            ((System.Web.UI.WebControls.HiddenField)this.FindControl(this.ClientID + "_hdnFiles")).Value = null;
        }

        // <summary>
        /// Seta os dados já existentes
        /// </summary>
        /// <param name="files"></param>
        public void CarregaFiles(UploadFileS files)
        {
            List<string> names = files.ConvertAll(x => x.name).ToList();
            List<int> keys = files.ConvertAll(x => x.key).ToList();

            ArrayOfFiles = ConvertToJSArray(JsonConvert.SerializeObject(ToJsonFile(names, keys)));
            Files = files;
            ((System.Web.UI.WebControls.HiddenField)this.FindControl(this.ClientID + "_hdnFiles")).Value = ArrayOfFiles;

            carregaLinksDownload();
        }

        /// <summary>
        /// Método que retorna os arquivos upados
        /// </summary>
        /// <returns></returns>
        public UploadFileS GetFiles()
        {
            var hdnFiles = (System.Web.UI.WebControls.HiddenField)this.FindControl(this.ClientID + "_hdnFiles");
            var filesName = JsonConvert.DeserializeObject<List<JsonFileAux>>(hdnFiles.Value);

            var returnfiles = new UploadFileS();
            foreach (var file in Files)
            {
                if (!filesName.Any(x => x.name.Equals(file.name.ReplaceSpecialCaracterFromFileName()))) continue;

                returnfiles.Add(file);
            }
            return returnfiles;
        }

        /// <summary>
        /// Método que retorna os arquivos upados
        /// </summary>
        /// <returns></returns>
        public void SetUploadedFiles()
        {
            var hiddenFileKey = Page.Request.Form.AllKeys.FirstOrDefault(p => p.Contains(this.ClientID + "_hdnFiles"));
            var filesName = JsonConvert.DeserializeObject<List<JsonFileAux>>(Page.Request.Form[hiddenFileKey]);
            Files.RemoveAll(p => !filesName.Any(x => x.name == p.name.ReplaceSpecialCaracterFromFileName()));

            var errorStatusKey = Page.Request.Form.AllKeys.FirstOrDefault(p => p.Contains(this.ClientID + "_errorStatus"));
            var isError = Convert.ToBoolean(Page.Request.Form[errorStatusKey]);

            if (isError) return;

            var addedfiles = new UploadFileS();

            for (int i = 0; i < Page.Request.Files.Count; i++)
            {
                if (Page.Request.Files.GetKey(i) == this.ClientID.Replace("_", "$"))
                {

                    HttpPostedFile file = Page.Request.Files[i];
                    String[] filename = file.FileName.Split(new char[] { '\\' });

                    //Verifica o arquivo adicionado
                    if (filesName != null && Files.Any(x => x.name.Equals(filename[filename.Length - 1])))
                        continue;

                    byte[] bytes = new byte[file.ContentLength];
                    file.InputStream.Read(bytes, 0, file.ContentLength);
                    addedfiles.Add(new UploadFile
                    {
                        name = filename[filename.Length - 1],
                        type = file.ContentType,
                        size = file.ContentLength,
                        dados = bytes
                    });
                }
            }

            Files.AddRange(addedfiles);

            CarregaFiles(Files);
        }
        public void SetUploadedFiles(UploadFile file)
        {
            Files.Add(file);

            CarregaFiles(Files);
        }

        /// <summary>
        /// Converte uma string em um array javascript
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        private string ConvertToJSArray(string jsonObject)
        {
            return string.Concat("[", jsonObject.Split('[')[1].Split(']')[0], "]");
        }

        /// <summary>
        /// Monta o array com os tipos permitidos
        /// </summary>
        /// <param name="tiposArquivo"></param>
        /// <returns></returns>
        private string MontaArrayTipos(TiposArquivoCollection tiposArquivo)
        {
            var stringArray = "";
            foreach (var tipo in tiposArquivo ?? new TiposArquivoCollection())
            {
                stringArray = string.Concat(stringArray, ObjectHelper.GetAttributeValue<MimeTypeAttribute, string>(tipo.Type), ",");
            }

            if (stringArray.Length > 0)
                return stringArray.Remove(stringArray.Length - 1, 1);
            else
                return "";
        }

        /// <summary>
        /// Monta o array com os error name
        /// </summary>
        /// <param name="tiposArquivo"></param>
        /// <returns></returns>
        private string MontaArrayErrorNames(TiposArquivoCollection tiposArquivo)
        {
            var stringArray = "";
            foreach (var tipo in tiposArquivo ?? new TiposArquivoCollection())
            {
                stringArray = string.Concat(stringArray, ObjectHelper.GetAttributeValue<ErrorNameAttribute, string>(tipo.Type), ",");
            }

            if (stringArray.Length > 0)
                return stringArray.Remove(stringArray.Length - 1, 1);
            else
            return "";
        }

        private List<JsonFileAux> ToJsonFile(List<string> files, List<int> keys)
        {
            var jsonfiles = new List<JsonFileAux>();

            keys.ForEach(x => jsonfiles.Add(new JsonFileAux { nrseq = x }));

            for (int cont = 0; cont < files.Count; cont++)
                jsonfiles[cont].name = files[cont].ReplaceSpecialCaracterFromFileName();

            return jsonfiles;
        }

        private List<JsonFileAux> ToJsonNrseq(List<string> files)
        {
            var jsonfiles = new List<JsonFileAux>();
            files.ForEach(x => jsonfiles.Add(new JsonFileAux { name = x }));
            return jsonfiles;
        }



        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            //SetUploadedFiles();
            if (this.FileDownload != null)
                FileDownload(this, new FileDownloadEventArgs(eventArgument));
        }
        public List<LiteralControl> GetScriptsAndStyles()
        {
            List<LiteralControl> listScriptsAndStyles = new List<LiteralControl>();

            listScriptsAndStyles.Add(new LiteralControl("<script type='text/javascript' src='" + ResolveUrl("~/Includes/Scripts/Controls/FileUpload.js") + "'></script>"));

            return listScriptsAndStyles;
        }
    }
    [Serializable]
    public class FileDownloadEventArgs : EventArgs
    {
        public FileDownloadEventArgs(string nome)
        {
            this.Nome = nome;
        }
        public String Nome { get; set; }
    }
    [Serializable]
    public class TiposArquivoCollection : List<FileType>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion
    }
    [Serializable]
    public class FileType : StateManagedItem
    {
        private Types type;
        //private string nameShowError;

        public Types Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}

public enum enumDisplayType
{
    List = 0,
    SmallIcons = 1,
    LargeIcons = 2
}

public enum Types
{
    [MimeType("TODOS")]
    TODOS,
    [MimeType("officedocument,msword,ms-word,ms-excel,ms-powerpoint,xmswrite")]
    [ErrorName("Extensões do Office")]
    OFFICE,
    [MimeType("image")]
    [ErrorName("Imagem")]
    IMAGEM,
    [MimeType("text/plain"), ErrorName("TXT"), Extension("txt")]
    TEXTO,
    [MimeType("video")]
    [ErrorName("Vídeo")]
    VIDEO,
    [MimeType("audio")]
    [ErrorName("Áudio")]
    AUDIO,
    [MimeType("application/pdf"), ErrorName("PDF"), Extension("pdf")]
    PDF,
    [MimeType("officedocument.wordprocessingml,ms-word,msword")]
    [ErrorName("Word")]
    WORD,
    [MimeType("officedocument.spreadsheetml,ms-excel,application/vnd.ms-excel,application/vnd.ms-excel.sheet.binary.macroEnabled.12,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ErrorName("Excel")]
    EXCEL,
    [MimeType("officedocument.presentationml,ms-powerpoint"), ErrorName("PowerPoint"), Extension("ppt")]
    POWERPOINT,
    [MimeType("image/jpeg"), ErrorName("JPG"), Extension("jpg")]
    JPG,
    [MimeType("image/png"), ErrorName("PNG"), Extension("png")]
    PNG,
    [MimeType("image/gif"), ErrorName("GIF"), Extension("gif")]
    GIF,
    [MimeType("text/xml"), ErrorName("XML"), Extension("xml")]
    XML,
    [MimeType("application/x-zip-compressed,application/octet-stream,application/zip"), ErrorName("ZIP"), Extension("zip")]
    ZIP,
    [MimeType("text/xml"), ErrorName("NESSUS"), Extension("nessus")]
    NESSUS
}

[Serializable]
public class UploadFile : DAOBase
{
    public string name { get; set; }
    public string type { get; set; }
    public int size { get; set; }
    public byte[] dados { get; set; }
    public int key { get; set; }

    /// <summary>
    /// Método salva o arquivo em um diretório fisico no servidor
    /// </summary>
    /// <param name="path">caminho do diretório onde o arquivo será salvo</param>
    public string Save(string path)
    {
        var extension = MyExtensions.GetValueFromMimeType<Types>(type).GetExtensionValue();

        if (!name.Contains("." + extension))
        {
            path += string.Concat(name, ".", extension);
        }
        else
        {
            path += name;
        }

        using (FileStream inputStream = new FileStream(path, FileMode.Create))
            inputStream.Write(dados, 0, dados.Length);

        return path;
    }
}

[Serializable]
public class UploadFileS : List<UploadFile> { }

[Serializable]
public class JsonFileAux
{
    public string name { get; set; }
    public int nrseq { get; set; }

}