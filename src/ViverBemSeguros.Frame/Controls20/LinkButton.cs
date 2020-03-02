using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celebre.Web.Controls
{
    [Serializable]
    public class LinkButton : System.Web.UI.WebControls.LinkButton
    {
        public String DataField
        {
            get
            {
                object o = ViewState["LinkButtonDataField"];
                return o == null ? String.Empty : (String)o;
            }
            set { ViewState["LinkButtonDataField"] = value; }
        }
    }
}