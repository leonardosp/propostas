using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using System.Net.Http.Headers;
using ViverBem.Domain.Token;
using Newtonsoft.Json;
using System.Web;
using System.Threading;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Application.Envios
{
    public  class Envios
    {
        private readonly IPropostaAppService _propostaAppService;

        public Envios(IPropostaAppService propostaAppService)
        {
            _propostaAppService = propostaAppService;
        }

        public void EnviarPropostas(IEnumerable<PropostasViewModel> list)
        {
            EnvioProposta envio = new EnvioProposta();

            foreach (var x in list)
            {
        
                var cli = _propostaAppService.ObterClientePorid(x.IdDoClienteX);
                var dependentes = _propostaAppService.ObterPorCliente(cli.Id);

                envio.benef = new List<DependenteProposta>();


                string dtnasc = Convert.ToString(x.Dt_nasc);
                var xt = dtnasc.Split('/');

                string dtNascMes = xt[1];
                string dtNascDia = xt[0];
                string dtNascAno = xt[2].Substring(0, 4);

                envio.dt_nasc = dtNascAno + "-" + dtNascMes + "-" + dtNascDia;

                envio.corretor = ("FC20666");
                envio.cpf = x.CPF;
                envio.nome = x.Nome;

                string dtVendda = Convert.ToString(x.Dt_venda);
                var xtt = dtVendda.Split('/');

                string dtVendaMes = xtt[1];
                string dtVendaDia = xtt[0];
                string dtVendaAno = xtt[2].Substring(0, 4);

                envio.dt_venda = dtVendaAno + "-" + dtVendaMes + "-" + dtVendaDia;

                envio.sexo = x.Sexo;
                envio.est_civil = x.Est_civil;
                envio.ender = x.Ender;
                envio.endnum = x.Endnum;
                envio.endcompl = x.EndComplemento;
                envio.bairro = x.Bairro;
                envio.cidade = x.Cidade;
                envio.uf = x.UF;
                envio.cep = x.CEP;
                envio.dddtel = x.DDDTel;
                envio.tel = x.Tel;
                envio.dddcel = x.DDDCel;
                envio.cel = x.Cel;
                envio.identidade = x.Identidade;
                if (envio.identidade == null)
                    envio.identidade = string.Empty;

                envio.org_emissor = x.Org_emissor;
                envio.email = x.Email;
                envio.cod_prof = x.Cod_prof;
                envio.ocupacao = x.Ocupacao;
                envio.id_ext_props = x.Id_ext_props;
                envio.cod_combo = x.CodCombo;
                envio.vl_premio = x.VlrPremio.Replace(",", ".");
                envio.vl_capital = x.VlrCapital.Replace(",", ".");
                envio.tp_pgto = x.Tp_pgto;
                envio.banco = x.Banco;
                envio.agencia = x.Agencia;
                if (x.Agencia_dv == null)
                {
                    envio.agencia_dv = string.Empty;
                }
                else
                {
                    envio.agencia_dv = x.Agencia_dv;
                }
                envio.cc = x.CC;
                envio.cc_dv = x.Cc_dv;
                envio.dia_debito = x.Dia_debito;
                envio.tipo_envio = x.Tipo_envio;
                envio.endercobr = x.Endercobr;
                envio.endnumcobr = x.Endnumcobr;
                envio.endcomplcobr = x.Endcomplcobr;
                envio.bairrocobr = x.Bairrocobr;
                envio.cidadecobr = x.Cidadecobr;
                envio.ufcobr = x.Ufcobr;
                envio.cepcobr = x.Cepcobr;
                envio.emailcobr = x.Email;
                envio.ppe = x.PPE;

                var xttt = dependentes;


                foreach (var item in dependentes)
                {
                    DependenteProposta dep = new DependenteProposta();
                    dep.id_ext_props = x.Id_ext_props;
                    dep.nome_ben = item.Nome;
                    dep.grau_par = item.Parentesco;
                    dep.perc = Convert.ToString(item.Participacao);

                    envio.benef.Add(dep);
                }

                envioJsonAsync(envio, x.Id);

                Thread.Sleep(2000);
            }
        }

        private void envioJsonAsync(EnvioProposta env, Guid id)
        {
            var x = _propostaAppService.ObterPorId(id);
            string uri = "http://www.mbmapi.com.br/";
            string username = "FC20666";
            string password = "20666#mbm";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(uri);

            HttpResponseMessage response =
              client.PostAsync("wsSisPrev/Token",
                new StringContent(string.Format("grant_type=password&username={0}&password={1}",
                  HttpUtility.UrlEncode(username),
                  HttpUtility.UrlEncode(password)), Encoding.UTF8,
                  "application/x-www-form-urlencoded")).Result;

            string resultJSON = response.Content.ReadAsStringAsync().Result;
            TokenToDeseralize tokkk = JsonConvert.DeserializeObject<TokenToDeseralize>(resultJSON);

            var data = new
            {
                corretor = "FC20666",
                env.cpf,
                env.nome,
                env.dt_nasc,
                env.sexo,
                env.est_civil,
                env.ender,
                env.endnum,
                env.endcompl,
                env.bairro,
                env.cidade,
                env.uf,
                env.cep,
                env.dddtel,
                env.tel,
                env.dddcel,
                env.cel,
                env.identidade,
                env.org_emissor,
                env.email,
                env.cod_prof,
                env.ocupacao,
                env.dt_venda,
                env.id_ext_props,
                env.cod_combo,
                env.vl_premio,
                env.vl_capital,
                env.tp_pgto,
                env.banco,
                env.agencia,
                env.agencia_dv,
                env.cc,
                env.cc_dv,
                env.dia_debito,
                env.tipo_envio,
                env.endercobr,
                env.endnumcobr,
                env.endcomplcobr,
                env.bairrocobr,
                env.cidadecobr,
                env.ufcobr,
                env.cepcobr,
                env.emailcobr,
                env.ppe,
                env.benef
            };

            var myContent = JsonConvert.SerializeObject(data);
            var content = new StringContent(myContent, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokkk.AccessToken);
            var result = client.PostAsync(uri + "wsSisPrev/api/propostas", content).Result;

            if (result.IsSuccessStatusCode)
            {
                var jsonString = result.Content.ReadAsStringAsync().Result;
                var texte = JsonConvert.DeserializeObject(jsonString);

                ResultadoEnvioViewModel rtt = JsonConvert.DeserializeObject<ResultadoEnvioViewModel>(texte.ToString());
                listaErros listaErros = JsonConvert.DeserializeObject<listaErros>(texte.ToString());
                x.Pronum = rtt.pronum;
                x.ProdTPag = rtt.prodtpag;
                x.AssocMatric = rtt.assmatrc;
                x.StatusConsulta = rtt.msg;
                x.Aprovado = true;
                _propostaAppService.Atualizar(x);

            }
            else
            {
                var jsonString = result.Content.ReadAsStringAsync().Result;
                var texte = JsonConvert.DeserializeObject(jsonString);
                var desc = "";
                string teste = texte.ToString().Replace("{", "");
                var p = teste.ToString().Replace("}", "");
                var c = p.ToString().Replace(" ", "");
                //if(c.Length > 167)
                //{
                //    desc = c.Substring(167);
                //}
                //else
                //{
                //     desc = c;
                //}
                //var split = teste.Split(",");
                //if(split.Length > 2)
                //{
                //    x.StatusConsulta = split[6] + split[8];
                //}
                //else if(split.Length == 2)
                //{
                //    x.StatusConsulta = split[0] + split[1];
                //}
                //else
                //{
                //    x.StatusConsulta = split[0];
                //}
                var l = c.ToString().Replace("]", "");
                var t = l.ToString().Replace("/", "");
                var r = t.ToString().Replace(":", "");
                var lt = r.ToString().Replace(",", "");
                x.StatusConsulta = lt;
                x.Excluido = true;
                _propostaAppService.Atualizar(x);

            }

        }

    }
}
