using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class ResultadoConsultaStatus
    {
        [JsonProperty(PropertyName = "msg")]
        public string msg { get; set; }

        [JsonProperty(PropertyName = "status_desc")]
        public string status_desc { get; set; }

    }

}
