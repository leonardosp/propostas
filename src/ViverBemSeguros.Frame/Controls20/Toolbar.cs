using Celebre.Web.Controls;
using Celebre.Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Celebre.Frame.Controls20
{
    [Serializable]
    [ParseChildren(true), PersistChildren(false)]
    public class Toolbar : System.Web.UI.WebControls.Panel
    {
        private ToolBarButtonCollection toolBarButtonCollection = null;

        [MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty)]
        public ToolBarButtonCollection ToolbarButtonCollection
        {
            get
            {
                if (toolBarButtonCollection == null)
                {
                    toolBarButtonCollection = new ToolBarButtonCollection();
                }
                return toolBarButtonCollection;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.CssClass = "btn-group col-sm-12 col-xs-12 toolbar";
            base.Render(writer);
        }
    }
    [Serializable]
    public class ToolBarButtonCollection : List<Button>
    {
    }
}
