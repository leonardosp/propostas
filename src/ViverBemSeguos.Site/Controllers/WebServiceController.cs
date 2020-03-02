using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Documentos;
using ViverBem.Domain.Interfaces;
using ViverBem.Domain.Token;

namespace ViverBemSeguos.Site.Controllers
{
    public class WebServiceController : BaseController
    {
        private readonly ITokenAppService _tokenAppService;

        public WebServiceController(ITokenAppService tokenAppService, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(notifications, user)
        {
            _tokenAppService = tokenAppService;
        }

        [Route("WebService")]
        public IActionResult Index()
        {
            var x = _tokenAppService.ObterTodos();

            if (x.Any())
            {
                var v = x.FirstOrDefault();
                if(v.LimiteDoToken > DateTime.Now)
                {
                    retornoJsonAsync(v.RETORNOTOTAL, "http://www.mbm.net.br/wsSisPrev/");
                    return View();
                }
                else
                {
                    _tokenAppService.Excluir(v.Id);

                    string uri = "http://www.mbm.net.br/wsSisPrev/";
                    string token = "";
                    string username = "FC20666";
                    string password = "20666#mbm";

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(uri);
                    HttpResponseMessage response =
                      client.PostAsync("Token",
                        new StringContent(string.Format("grant_type=password&username={0}&password={1}",
                          HttpUtility.UrlEncode(username),
                          HttpUtility.UrlEncode(password)), Encoding.UTF8,
                          "application/x-www-form-urlencoded")).Result;

                    string resultJSON = response.Content.ReadAsStringAsync().Result;


                    string[] partes = resultJSON.Split('"');
                    
                    TokenResultViewModel tk = new TokenResultViewModel();
                    TokenToDeseralize tokkk = JsonConvert.DeserializeObject<TokenToDeseralize>(resultJSON);

                    client.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", tokkk.AccessToken);

                    HttpResponseMessage xresponse = client.GetAsync(uri + "Api/dados/tipoenvio").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString =  xresponse.Content.ReadAsStringAsync();
                    }

                    tk.AccessToken = tokkk.AccessToken;
                    tk.RETORNOTOTAL = resultJSON;
                    tk.Error = "Erro";
                    tk.ErrorDescription = "Erro";
                    var t = Convert.ToDateTime(partes[21]);
                    tk.LimiteDoToken = t.ToUniversalTime();
                    _tokenAppService.Registrar(tk);
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        private static LoginTokenResult GetLoginToken(string username, string password, string baseUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            HttpResponseMessage response =
              client.PostAsync("Token",
                new StringContent(string.Format("grant_type=password&username={0}&password={1}",
                  HttpUtility.UrlEncode(username),
                  HttpUtility.UrlEncode(password)), Encoding.UTF8,
                  "application/x-www-form-urlencoded")).Result;

            string resultJSON = response.Content.ReadAsStringAsync().Result;
            string[] partes = resultJSON.Split('"');
            LoginTokenResult result = JsonConvert.DeserializeObject<LoginTokenResult>(partes[3]);

            return result;
        }

        private async void retornoJsonAsync(string token, string baseUrl)
        {          

            HttpClient client = new HttpClient();
            TokenToDeseralize tokkk = JsonConvert.DeserializeObject<TokenToDeseralize>(token);
            client.BaseAddress = new Uri("http://www.mbm.net.br/wsSisPrev/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokkk.AccessToken);
            //HttpResponseMessage response = await client.PostAsync("wsSisPrev/api/corretores/consultacpf", new StringContent(string.Format("Content-Type:application/json")));
            HttpResponseMessage response = client.GetAsync("Api/Corretores/FC20666/Debitoconta").Result;
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
            //client.DefaultRequestHeaders.Add("Authorization", string.Format(token));
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //response.Headers.Add("Authorization", "bearer " + token);
            //response = client.GetAsync("wsSisPrev/api/corretores/FC20666/combos").Result;


            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(jsonString, (typeof(DataTable)));
            }
        }

        //public class LoginTokenResult
        //{
        //    public override string ToString()
        //    {
        //        return AccessToken;
        //    }

        //    [JsonProperty(PropertyName = "access_token")]
        //    public string AccessToken { get; set; }

        //    [JsonProperty(PropertyName = "error")]
        //    public string Error { get; set; }

        //    [JsonProperty(PropertyName = "error_description")]
        //    public string ErrorDescription { get; set; }

        //}
    }
}
