using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Celebre.Web.Controls
{
    public delegate void AddValueReturnFunctionEventHandler(object sender, AddValueReturnFunctionEventArgs e);
    [Serializable]
    public class AddValueReturnFunctionEventArgs : EventArgs
    {
        public String Text { get; set; }
        public String Value { get; set; }
        public AddValueReturnFunctionEventArgs(String arguments)
        {
            string[] args = arguments.Split(new string[] { "***" }, StringSplitOptions.None);
            Value = args[1];
            Text = args[2];
        }
    }
    [Serializable]
    public class PublicEventHandlers
    {

    }
}
