using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Token
{
    public class LoginTokenResult : Entity<LoginTokenResult>
    {
        public override string ToString()
        {
            return AccessToken;
        }

        public override bool EhValido()
        {
            return true;
        }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }

        public DateTime LimiteDoToken { get; set; }
        public string RETORNOTOTAL { get; set; }


        public static class TokenFactory
        {
            public static LoginTokenResult NovoToken(Guid id, string accestoken, string error, string errordescription, string retornototal,DateTime limiteToken)
            {
                var token = new LoginTokenResult()
                {
                    Id = id,
                    AccessToken = accestoken,
                    Error = error,
                    ErrorDescription = errordescription,
                    RETORNOTOTAL = retornototal,
                    LimiteDoToken = limiteToken
                };

                return token;
            }
        }
    }
}
