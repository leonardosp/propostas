using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using RestSharp;
using RestSharp.Authenticators;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Application.Envios;
using ViverBem.Application.Interfaces;
using ViverBem.Application.Util;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Interfaces;
using ViverBem.Domain.Token;

namespace ViverBemSeguos.Site.Controllers
{
    public class PropostasController : BaseController
    {
        private readonly IPropostaAppService _propostaAppService;

        public PropostasController(IPropostaAppService propostaAppService, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(notifications, user)
        {
            _propostaAppService = propostaAppService;
        }
        [Route("Propostas")]
        public IActionResult Index()
        {
            return View(_propostaAppService.ObterPorFuncionario(FuncionarioId));
        }

        [Route("Relatorios")]
        public IActionResult Relatorios()
        {
            return View();
        }

        [Route("nova-proposta")]
        public IActionResult Create()
        {
            IEnumerable<ComboPrincViewModel> combos = new List<ComboPrincViewModel>();
            combos = _propostaAppService.ObterCombos();
            ViewBag.ListDados = combos;

            IEnumerable<ClienteViewModel> cli = new List<ClienteViewModel>();
            cli = _propostaAppService.ObterClientePorFuncionario(FuncionarioId);
            ViewBag.ListCombos = cli;

            PropostasViewModel prop = new PropostasViewModel();
            prop = _propostaAppService.ObterUltimaProposta();
            decimal seq = Convert.ToDecimal(prop.StatusFinanceiro);
            decimal soma = seq + 1;
            var seque = soma;
            prop.StatusFinanceiro = Convert.ToString(seque);
            _propostaAppService.Atualizar(prop);

            ViewBag.Sequencia = seque;

            return View();
        }

        [HttpGet]
        [Route("editar-proposta/{id=guid}")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _propostaAppService.ObterPorId(id.Value);

            if (clienteViewModel == null)
            {
                return NotFound();
            }


            return View(clienteViewModel);
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-proposta/{id=guid}")]
        public IActionResult Edit(PropostasViewModel clienteViewModel)
        {
            var x = _propostaAppService.ObterPorId(clienteViewModel.Id);


            if (!ModelState.IsValid) return View(clienteViewModel);

            clienteViewModel.FuncionarioId = FuncionarioId;
            _propostaAppService.Atualizar(clienteViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Proposta atualizada com Sucesso!" : "error, Proposta não pode ser atualizado, verifique as mensagens";

            return View(clienteViewModel);
        }

        [Route("importa-propostas")]
        public IActionResult ImportaPropostas()
        {
            return View();
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [Route("importa-propostas")]
        public IActionResult ImportaPropostas(IFormFile importExcel)
        {
            PropostasViewModel prop = new PropostasViewModel();
            if (importExcel == null || importExcel.Length == 0)
            {
                return View();
            }

            MemoryStream memoryStream = new MemoryStream();

            importExcel.CopyToAsync(memoryStream).ConfigureAwait(false);
            ExcelPackage package = new ExcelPackage(memoryStream);

            for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
            {
                var totalRows = package.Workbook.Worksheets[i].Dimension?.Rows;
                var totalCollumns = package.Workbook.Worksheets[i].Dimension?.Columns;
                for (int j = 1; j <= totalRows; j++)
                {
                    for (int k = 1; k <= totalCollumns.Value; k++)
                    {
                        var xt = _propostaAppService.ObterUltimaProposta();
                        decimal seq = Convert.ToDecimal(xt.StatusFinanceiro);
                        decimal soma = seq + 1;
                        var seque = soma;
                        prop.StatusFinanceiro = Convert.ToString(seque);

                        string cpf = Util.RemoveCaracteresEspeciais(package.Workbook.Worksheets[i].Cells[j, 1].Value.ToString(), false, true, true, false, false);

                        prop.Dt_venda = Convert.ToDateTime(package.Workbook.Worksheets[i].Cells[j, 2].Value) == null ? DateTime.Now : Convert.ToDateTime(package.Workbook.Worksheets[i].Cells[j, 2].Value);
                        prop.CodCombo = package.Workbook.Worksheets[i].Cells[j, 3].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 3].Value.ToString();
                        prop.VlrPremio = package.Workbook.Worksheets[i].Cells[j, 4].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 4].Value.ToString();
                        prop.VlrCapital = package.Workbook.Worksheets[i].Cells[j, 5].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 5].Value.ToString();
                        prop.Tp_pgto = package.Workbook.Worksheets[i].Cells[j, 6].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 6].Value.ToString();
                        prop.Banco = package.Workbook.Worksheets[i].Cells[j, 7].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 7].Value.ToString();
                        prop.Agencia = package.Workbook.Worksheets[i].Cells[j, 8].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 8].Value.ToString();
                        prop.Agencia_dv = package.Workbook.Worksheets[i].Cells[j, 9].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 9].Value.ToString();
                        prop.CC = package.Workbook.Worksheets[i].Cells[j, 10].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 10].Value.ToString();
                        prop.Cc_dv = package.Workbook.Worksheets[i].Cells[j, 11].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 11].Value.ToString();
                        prop.Dia_debito = package.Workbook.Worksheets[i].Cells[j, 12].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 12].Value.ToString();
                        prop.Tipo_envio = package.Workbook.Worksheets[i].Cells[j, 13].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 13].Value.ToString();
                        prop.Id_ext_props = seque.ToString();
                        prop.Corretor = "FC20666";


                        if (prop.CodCombo == "(VB - 60 / FF - 60)")
                        {
                            prop.CodCombo = "000092";
                        }
                        else if (prop.CodCombo == "(VB - 15 / FF - 15)")
                        {
                            prop.CodCombo = "000095";
                        }
                        else if (prop.CodCombo == "(VB - 30 / FF - 30)")
                        {
                            prop.CodCombo = "000094";
                        }
                        else if (prop.CodCombo == "(VB - 45 / FF - 45)")
                        {
                            prop.CodCombo = "000093";
                        }
                        else {
                            prop.CodCombo = "000096";
                        }

                        var cliente = _propostaAppService.ObterClientePorCpf(cpf);

                        if(cliente != null)
                        {
                            prop.IdDoClienteX = cliente.Id;
                            prop.Nome = cliente.Nome;
                            prop.Dt_nasc = cliente.DataNasc;
                            prop.Sexo = cliente.Sexo;
                            prop.Est_civil = cliente.EstadoCivil;
                            prop.Ender = cliente.Endereco.Logradouro;
                            prop.Endnum = cliente.Endereco.Numero;
                            prop.EndComplemento = cliente.Endereco.Complemento;
                            prop.Bairro = cliente.Endereco.Bairro;
                            prop.Cidade = cliente.Endereco.Cidade;
                            prop.UF = cliente.Endereco.Estado;
                            prop.CEP = cliente.Endereco.CEP;
                            prop.Endercobr = cliente.Endereco.Logradouro;
                            prop.Endnumcobr = cliente.Endereco.Numero;
                            prop.Endcomplcobr = cliente.Endereco.Complemento;
                            prop.Bairrocobr = cliente.Endereco.Bairro;
                            prop.Cidadecobr = cliente.Endereco.Cidade;
                            prop.Ufcobr = cliente.Endereco.Estado;
                            prop.Cepcobr = cliente.Endereco.CEP;
                            prop.PPE = cliente.PPE;

                            if(cliente.Fone != null && cliente.Fone.Length > 4)
                            {
                                var dddTel = cliente.Fone.Substring(0, 2);
                                var tel = cliente.Fone.Substring(2, 8);
                                prop.DDDTel = dddTel;
                                prop.Tel = tel;
                            }
                            else
                            {
                                prop.DDDTel = string.Empty;
                                prop.Tel = string.Empty;
                            }
                            
                            if(cliente.Celular != null && cliente.Celular.Length > 4)
                            {
                                var dddcel = cliente.Celular.Substring(0, 2);
                                var cel = cliente.Celular.Substring(0, 2);
                                prop.DDDCel = dddcel;
                                prop.Cel = cel;
                            }
                            else
                            {
                                prop.DDDCel = string.Empty;
                                prop.Cel = string.Empty;
                            }

                            prop.Org_emissor = cliente.OrgaoExpedidor;
                            prop.Email = "dcredipoa@gmail.com";
                            prop.Cod_prof = cliente.CodigoProf.ToString();
                            prop.Ocupacao = cliente.Ocupacao;
                            prop.FuncionarioId = FuncionarioId;
                            prop.CPF = cliente.CPF;

                            var proposta = _propostaAppService.ObterPorCpf(cpf);
                            if(proposta == null)
                            {
                                _propostaAppService.Registrar(prop);
                            }

                            prop.Id = Guid.NewGuid();
                            k = 99;

                        }
                        prop.Id = Guid.NewGuid();
                        k = 99;
                    }
                }
            }



            ViewBag.RetornoPost = OperacaoValida() ? "succes, Cliente Registrado com Sucesso!" : "error, Cliente não pode ser Registrado, verifique as mensagens";

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        [Route("atualizar-propostas")]
        public IActionResult AtualizarPropostas()
        {
            return View();
        }

        [HttpPost]
        [Route("atualizar-propostas")]
        public IActionResult AtualizarPropostas(IFormFile importExcel)
        {
            if (importExcel == null || importExcel.Length == 0)
            {
                return View();
            }

            MemoryStream memoryStream = new MemoryStream();

            importExcel.CopyToAsync(memoryStream).ConfigureAwait(false);
            ExcelPackage package = new ExcelPackage(memoryStream);

            for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
            {
                var totalRows = package.Workbook.Worksheets[i].Dimension?.Rows;
                var totalCollumns = package.Workbook.Worksheets[i].Dimension?.Columns;
                for (int j = 1; j <= totalRows; j++)
                {
                    for (int k = 1; k <= totalCollumns.Value; k++)
                    {

                        string FLG = Util.RemoveCaracteresEspeciais(package.Workbook.Worksheets[i].Cells[j, 4].Value.ToString(),false,true,true,false,false);
                        var xt = _propostaAppService.ObterUltimaProposta();
                        var prop = _propostaAppService.ObterPorCpf(FLG);

                        decimal seq = Convert.ToDecimal(xt.StatusFinanceiro);
                        decimal soma = seq + 1;
                        var seque = soma;
                        prop.StatusFinanceiro = Convert.ToString(seque);

                        prop.Banco = package.Workbook.Worksheets[i].Cells[j, 1].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 1].Value.ToString();
                        prop.Agencia = package.Workbook.Worksheets[i].Cells[j, 2].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 2].Value.ToString();
                        prop.Agencia_dv = "";

                      

                        var ccDV = package.Workbook.Worksheets[i].Cells[j, 3].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 3].Value.ToString();
                        var splitConta = ccDV.Split("-");

                        prop.Excluido = false;
                        prop.Aprovado = false;
                        prop.CC = splitConta[0];
                        prop.Cc_dv = splitConta[1];

                        prop.Id_ext_props = seque.ToString();
                        prop.CEP = package.Workbook.Worksheets[i].Cells[j, 5].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 5].Value.ToString();

                        _propostaAppService.Atualizar(prop);

                        prop.Id = Guid.NewGuid();
                        k = 99;
                    }
                }
            }



            ViewBag.RetornoPost = OperacaoValida() ? "succes, Cliente Registrado com Sucesso!" : "error, Cliente não pode ser Registrado, verifique as mensagens";

            if (OperacaoValida())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //[Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("nova-proposta")]
        public IActionResult Create(PropostasViewModel propostasViewModel, string PROPSX)
        {
            propostasViewModel.Id_ext_props = PROPSX;
            Guid IdCombo = Guid.Parse(propostasViewModel.CodCombo);
            var combo = _propostaAppService.ObterCombosPorId(IdCombo);
            var codcombo = "";
            decimal vlrpremio = 0;
            decimal vlrcapital = 0;
            foreach (var item in combo)
            {
                codcombo = item.CodCombo;
                string premio = item.VlrPremio.Replace(".", ",");
                vlrpremio = vlrpremio + Convert.ToDecimal(premio);
                vlrcapital = vlrcapital + Convert.ToDecimal(item.VlrCapital);
            }
            Guid id = Guid.Parse(propostasViewModel.Nome);
            var x = _propostaAppService.ObterClientePorid(id);
            x.dependentes = _propostaAppService.ObterPorCliente(id);

            propostasViewModel.Corretor = "FC20666";
            propostasViewModel.CPF = x.CPF;
            propostasViewModel.Nome = x.Nome;
            propostasViewModel.Dt_nasc = x.DataNasc;
            propostasViewModel.Sexo = x.Sexo;
            propostasViewModel.Est_civil = x.EstadoCivil;
            propostasViewModel.Ender = x.Endereco.Logradouro;
            propostasViewModel.Endnum = x.Endereco.Numero;
            propostasViewModel.EndComplemento = x.Endereco.Complemento;
            propostasViewModel.Bairro = x.Endereco.Bairro;
            propostasViewModel.Cidade = x.Endereco.Cidade;
            propostasViewModel.UF = x.Endereco.Estado;
            propostasViewModel.CEP = x.Endereco.CEP;
            propostasViewModel.DDDTel = x.Fone.Substring(0, 2);
            propostasViewModel.Tel = x.Fone.Substring(2);
            propostasViewModel.DDDCel = x.Celular.Substring(0, 2);
            propostasViewModel.Cel = x.Celular.Substring(2);
            propostasViewModel.Identidade = x.RG;
            propostasViewModel.Org_emissor = x.OrgaoExpedidor;
            propostasViewModel.Email = x.Email;
            propostasViewModel.Cod_prof = Convert.ToString(x.CodigoProf);
            propostasViewModel.Ocupacao = x.Ocupacao;
            propostasViewModel.Dt_venda = propostasViewModel.Dt_venda;
            propostasViewModel.Id_ext_props = propostasViewModel.Id_ext_props;
            propostasViewModel.StatusFinanceiro = propostasViewModel.Id_ext_props;
            propostasViewModel.CodCombo = codcombo;
            propostasViewModel.VlrPremio = Convert.ToString(vlrpremio);
            propostasViewModel.VlrCapital = Convert.ToString(vlrcapital);
            propostasViewModel.Tp_pgto = "D";
            propostasViewModel.Banco = propostasViewModel.Banco;
            propostasViewModel.Agencia = propostasViewModel.Agencia;
            propostasViewModel.Agencia_dv = propostasViewModel.Agencia_dv;
            propostasViewModel.CC = propostasViewModel.CC;
            propostasViewModel.Cc_dv = propostasViewModel.Cc_dv;
            propostasViewModel.Dia_debito = propostasViewModel.Dia_debito;
            propostasViewModel.Tipo_envio = "B";
            propostasViewModel.Ufcobr = x.Endereco.Estado;
            propostasViewModel.Endercobr = x.Endereco.Complemento;
            propostasViewModel.Endnumcobr = x.Endereco.Numero;
            propostasViewModel.Endcomplcobr = x.Endereco.Complemento;
            propostasViewModel.Bairrocobr = x.Endereco.Bairro;
            propostasViewModel.Cidadecobr = x.Endereco.Cidade;
            propostasViewModel.Cepcobr = x.Endereco.CEP;
            propostasViewModel.PPE = x.PPE;
            propostasViewModel.FuncionarioId = FuncionarioId;

            _propostaAppService.Registrar(propostasViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Cliente Registrado com Sucesso!" : "error, Cliente não pode ser Registrado, verifique as mensagens";

            if (OperacaoValida())
            {
                return View("Index", _propostaAppService.ObterTodos());
            }
            {
                return View(propostasViewModel);
            }

        }

        public IActionResult AprovarProposta(Guid id)
        {
            var x = _propostaAppService.ObterPorId(id);
            x.Aprovado = true;

            _propostaAppService.Atualizar(x);

            return View("PropostasParaAprovar", _propostaAppService.ObterTodos());
        }

        //[Authorize(Policy = "PodeTudo")]
        public IActionResult EnviarProposta(Guid id)
        {
            var x = _propostaAppService.ObterPorId(id);
            EnvioProposta envio = new EnvioProposta();
            var cli = _propostaAppService.ObterClientePorNome(x.Nome);
            var dependentes = _propostaAppService.ObterPorCliente(cli.Id);

            envio.benef = new List<DependenteProposta>();


            string dtnasc = Convert.ToString(x.Dt_nasc);
            var xt = dtnasc.Split("/");

            string dtNascMes = xt[1];
            string dtNascDia = xt[0];
            string dtNascAno = xt[2].Substring(0, 4);

            envio.dt_nasc = dtNascAno + "-" + dtNascMes + "-" + dtNascDia;

            envio.corretor = ("FC20666");
            envio.cpf = x.CPF;
            envio.nome = x.Nome;

            string dtVendda = Convert.ToString(x.Dt_venda);
            var xtt = dtVendda.Split("/");

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






            envioJsonAsync(envio, id);

            return View("Index", _propostaAppService.ObterTodos());
        }

        public IActionResult EnviarTodasPropostas()
        {

            var propostas = _propostaAppService.ObterPropostasParaAprovar();

            Envios envios = new Envios(_propostaAppService);
            envios.EnviarPropostas(propostas);

            return View("PropostasParaAprovar", _propostaAppService.ObterTodos());
        }
        public IActionResult RecusarProposta(Guid id)
        {
            var x = _propostaAppService.ObterPorId(id);

            return View("PropostasParaAprovar", _propostaAppService.ObterTodos());
        }
        public IActionResult ConsultarPropostas()
        {
            var propostas = _propostaAppService.ObterPropostasComPronum();
            EnvioProposta envio = new EnvioProposta();

            foreach (var x in propostas)
            {
                string dtVendda = x.Dt_venda.ToString("dd/MM/yyyy");
                var xtt = dtVendda.Split("/");

                string dtVendaMes = xtt[1];
                string ano = xtt[2];

                consultaJson(x.Pronum, ano, dtVendaMes, x.Id);


            }

            return View("PropostasParaAprovar", _propostaAppService.ObterTodos());
        }

        public IActionResult ConsultarFinanceiro(Guid id)
        {
            var x = _propostaAppService.ObterPorId(id);

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

            HttpResponseMessage xresponse = client.GetAsync(uri + "Api/propostas/" + x.Pronum).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = xresponse.Content.ReadAsStringAsync();
            }

            return View("Index", _propostaAppService.ObterTodos());

        }
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _propostaAppService.ObterPorId(id.Value);

            if (clienteViewModel == null)
            {
                return NotFound();
            }

            return View(clienteViewModel);
        }
        [Route("Propostas-Para-Aprovar")]
        public IActionResult PropostasParaAprovar(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            if (searchString == null)
            {
                return View(_propostaAppService.ObterPropostasParaAprovar());
            }
            else
            {
                var dados = from s in _propostaAppService.ObterPropostasParaAprovar()
                            select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    dados = dados.Where(s => s.CPF.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        dados = dados.OrderByDescending(s => s.Nome);
                        break;
                    case "data_venda":
                        dados = dados.OrderByDescending(s => s.Dt_venda);
                        break;
                    case "data_nasc":
                        dados = dados.OrderByDescending(s => s.Dt_nasc);
                        break;
                }

                return View(dados.ToList());
            }
        }
        [Route("Propostas-Aprovadas")]
        public IActionResult PropostasAprovadas(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            if (searchString == null)
            {
                return View(_propostaAppService.ObterPropostasAprovadas());
            }
            else
            {
                var dados = from s in _propostaAppService.ObterPropostasAprovadas()
                            select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    dados = dados.Where(s => s.CPF.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        dados = dados.OrderByDescending(s => s.Nome);
                        break;
                    case "data_venda":
                        dados = dados.OrderByDescending(s => s.Dt_venda);
                        break;
                    case "data_nasc":
                        dados = dados.OrderByDescending(s => s.Dt_nasc);
                        break;
                }

                return View(dados.ToList());
            }

        }

        [Route("Propostas-Recusadas")]
        public IActionResult PropostasRecusadas(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            if (searchString == null)
            {
                return View(_propostaAppService.ObterPropostasRecusadas());
            }
            else
            {
                var dados = from s in _propostaAppService.ObterPropostasRecusadas()
                            select s;

                if (!String.IsNullOrEmpty(searchString))
                {
                    dados = dados.Where(s => s.CPF.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        dados = dados.OrderByDescending(s => s.Nome);
                        break;
                    case "data_venda":
                        dados = dados.OrderByDescending(s => s.Dt_venda);
                        break;
                    case "data_nasc":
                        dados = dados.OrderByDescending(s => s.Dt_nasc);
                        break;
                }

                return View(dados.ToList());
            }


        }

        public IActionResult GerarExcel(DateTime Inicio, DateTime Fim, string group1)
        {
            string dtinicio = Convert.ToString(Inicio);
            var xt = dtinicio.Split("/");

            string dtNascMes = xt[1];
            string dtNascDia = xt[0];
            string dtNascAno = xt[2].Substring(0, 4);

            DateTime dt_inicio = Convert.ToDateTime(dtNascAno + "-" + dtNascMes + "-" + dtNascDia);


            string dtFim = Convert.ToString(Fim);
            var xtt = dtFim.Split("/");

            string dtVendaMes = xtt[1];
            string dtVendaDia = xtt[0];
            string dtVendaAno = xtt[2].Substring(0, 4);

            DateTime dt_fim = Convert.ToDateTime(dtVendaAno + "-" + dtVendaMes + "-" + dtVendaDia);

            var DummyData = _propostaAppService.ObterPropostasPorData(dt_inicio, dt_fim);
            string name = String.Empty;
            if (group1 == "Aprovadas")
            {
                DummyData = _propostaAppService.ObterPropostasAprovadasPorData(dt_inicio, dt_fim);
                name = "Aprovadas";
            }
            else if (group1 == "Todas")
            {
                DummyData = _propostaAppService.ObterPropostasPorData(dt_inicio, dt_fim);
                name = "Todas";
            }
            else if (group1 == "Recusadas")
            {
                DummyData = _propostaAppService.ObterPropostasRecusadasPorData(dt_inicio, dt_fim);
                name = "Recusadas";
            }
            else
            {
                DummyData = _propostaAppService.ObterPropostasPorData(dt_inicio, dt_fim);
                name = "Todas";
            }


            var comlumHeadrs = new string[]
           {
                "CPF",
                "Nome",
                "Data Venda",
                "Ocupacao",
                "Resultado",
                "Pronum",
                "Valor Prêmio"
           };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook

                var worksheet = package.Workbook.Worksheets.Add("Propostas"); //Worksheet name
                using (var cells = worksheet.Cells[1, 1, 1, 5]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                //Add values
                var j = 2;
                foreach (var employee in DummyData)
                {
                    worksheet.Cells["A" + j].Value = employee.CPF;
                    worksheet.Cells["B" + j].Value = employee.Nome;
                    worksheet.Cells["C" + j].Value = employee.Dt_venda.ToString("MM/dd/yyyy");
                    worksheet.Cells["D" + j].Value = employee.Ocupacao;
                    worksheet.Cells["E" + j].Value = employee.StatusConsulta;
                    worksheet.Cells["F" + j].Value = employee.Pronum;
                    worksheet.Cells["G" + j].Value = employee.VlrPremio;

                    j++;
                }
                result = package.GetAsByteArray();
            }

            return File(result, "application/ms-excel", $"Propostas" + "De " + dt_inicio.ToString("dd/MM/yyyy") + "Até " + dt_fim.ToString("dd/MM/yyyy") + name + ".xlsx");
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
                var t = l.ToString().Replace("/","");
                var r = t.ToString().Replace(":", "");
                var lt = r.ToString().Replace(",","");
                x.StatusConsulta = lt;
                x.Excluido = true;
                _propostaAppService.Atualizar(x);

            }

        }
        private void consultaJson(string pronum, string ano, string mes, Guid id)
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


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokkk.AccessToken);
            var result = client.GetAsync(uri + "wsSisPrev/api/propostas/" + pronum + "/consultafinanceiro/" + ano + "/" + mes).Result;

            if (result.IsSuccessStatusCode)
            {
                var jsonString = result.Content.ReadAsStringAsync().Result;
                var texte = JsonConvert.DeserializeObject(jsonString);

                ResultadoEnvioViewModel rtt = JsonConvert.DeserializeObject<ResultadoEnvioViewModel>(texte.ToString());
                listaErros listaErros = JsonConvert.DeserializeObject<listaErros>(texte.ToString());
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
                if (c.Length > 167)
                {
                    desc = c.Substring(167);
                }
                else
                {
                    desc = c;
                }
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
                x.StatusConsulta = desc;
                x.Excluido = true;
                _propostaAppService.Atualizar(x);

            }

        }

        private void envioJsonAnt(EnvioProposta env, Guid id)
        {
            var x = _propostaAppService.ObterPorId(id);
            string uri = "http://www.mbm.net.br/";
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

                string teste = texte.ToString().Replace("{", "");
                var p = teste.ToString().Replace("}", "");
                var c = p.ToString().Replace(" ", "");
                var desc = c.Substring(167);
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
                x.StatusConsulta = c;
                x.Excluido = true;
                _propostaAppService.Atualizar(x);

            }

        }

    }
}