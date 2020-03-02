using Celebre.Web.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Web.UI;

namespace Celebre.Web.Controls
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class Captcha : StateManagedCompositeControl
    {
        private String Text
        {
            get
            {
                object o = ViewState["CaptchaText"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["CaptchaText"] = value; }
        }
        public int Lenght { get; set; }

        protected override void OnInit(EventArgs e)
        {
            TextBox txtCaptcha = new TextBox();
            txtCaptcha.ID = "txtCaptcha";
            txtCaptcha.CampoObrigatorio = false;
            txtCaptcha.MontaGroup = false;

            this.Controls.Add(txtCaptcha);

            Button btnRefreshCaptcha = new Button();
            btnRefreshCaptcha.ID = "btnRefreshCaptcha";
            btnRefreshCaptcha.ButtonIcon = FontAwesomeIcons.Refresh;
            btnRefreshCaptcha.ButtonSize = ButtonSize.Medium;
            btnRefreshCaptcha.ButtonStyle = ButtonStyle.Default;
            btnRefreshCaptcha.Click += btnRefreshCaptcha_Click;

            this.Controls.Add(btnRefreshCaptcha);

            base.OnInit(e);
        }

        void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        protected override void OnLoad(EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Text))
                Text = GetTextCaptcha(this.Lenght);

            if (Page.IsPostBack && Page.Request.Form[this.ID + "$" + this.Controls[0].ID] != null)
                ((TextBox)this.Controls[0]).Text = Page.Request.Form[this.ID + "$" + this.Controls[0].ID].ToString();

            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
           System.Drawing .Image imgCaptcha = DrawText(Text, new Font("Arial Black", 20f, FontStyle.Bold), Color.Green, Color.White);
            byte[] byteCaptcha = imageToByteArray(imgCaptcha);

            writer.Write("<div id='{0}' class='boxPanel'>", this.ID);            
            writer.Write("  <div class='boxPanel-header'>");
            writer.Write("      <h2 class='h2Panel'><i class='fa fa-keyboard-o'></i>Confirme a imagem</h2>");
            writer.Write("  </div>");

            writer.Write("  <div class='boxPanel-body' style='margin-bottom:10px; margin-top:5px;'>");
            writer.Write("      <div class='row'>");
            writer.Write("          <div class='col-sm-offset-1 col-sm-7 centerAlign verticalAlignCenter' style='padding-right:0;'>");
            writer.Write("              <img alt='captcha' src='data:image/jpeg;base64,{0}' />", Convert.ToBase64String(byteCaptcha));
            writer.Write("          </div>");
            writer.Write("          <div class='col-sm-2 centerAlign verticalAlignCenter' style='padding-left:0;'>");
            this.Controls[1].RenderControl(writer);
            writer.Write("          </div>");
            writer.Write("      </div>");
            writer.Write("      <div class='row'>");
            writer.Write("          <div class='col-sm-offset-1 col-sm-10'>");
            this.Controls[0].RenderControl(writer);
            writer.Write("          </div>");
            writer.Write("      </div>");

            writer.Write("  </div>");
            writer.Write("</div>");
        }

        private System .Drawing .Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            //first, create a dummy bitmap just to get a graphics object
            System.Drawing.Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;

        }
        private byte[] imageToByteArray(System .Drawing .Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }
        private String GetTextCaptcha(int length)
        {
            String[] array = new String[35]{"1","2","3","4","5","6","7","8","9",            
        "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

            Random random = new Random();
            String textCaptcha = "";
            for (int i = 0; i < length; i++)
                textCaptcha += array[random.Next(0, array.Length)];

            return textCaptcha;
        }
        public Boolean IsValid
        {
            get { return (this.Controls[0] as TextBox).Text.ToUpper() == Text; }
        }
        public void Refresh()
        {
            Text = GetTextCaptcha(this.Lenght);
        }
    }
}
