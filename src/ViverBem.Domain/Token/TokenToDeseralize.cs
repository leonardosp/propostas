using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Token
{
    public class TokenToDeseralize
    {
        public override string ToString()
        {
            return AccessToken;
        }


        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }
    }
}
