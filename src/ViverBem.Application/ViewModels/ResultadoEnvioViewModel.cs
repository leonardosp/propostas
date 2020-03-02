using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class ResultadoEnvioViewModel
    {
        [JsonProperty(PropertyName = "msg")]
        public string msg { get; set; }

        [JsonProperty(PropertyName = "pronum")]
        public string pronum { get; set; }

        [JsonProperty(PropertyName = "assmatrc")]
        public string assmatrc { get; set; }

        [JsonProperty(PropertyName = "prodtpag")]
        public string prodtpag { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string error { get; set; }

        public List<string> Error { get; set; }

    }
    public class listaErros{
        [JsonProperty(PropertyName = "error")]
        public List<itemErro> error { get; set; }
        }

    public class itemErro
    {
        [JsonProperty(PropertyName = "msg")]
        public string msg { get; set; }

        [JsonProperty(PropertyName = "pronum")]
        public string pronum { get; set; }

        [JsonProperty(PropertyName = "assmatrc")]
        public string assmatrc { get; set; }

        [JsonProperty(PropertyName = "prodtpag")]
        public string prodtpag { get; set; }

        [JsonProperty(PropertyName = "ttError")]
        public string ttErrror { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "descr")]
        public string descr { get; set; }
    }
}
