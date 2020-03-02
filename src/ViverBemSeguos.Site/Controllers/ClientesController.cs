using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Interfaces;

namespace ViverBemSeguos.Site.Controllers
{

    [Authorize]
    public class ClientesController : BaseController
    {
        private readonly IClienteAppService _clienteAppService;

        public ClientesController(IClienteAppService clienteAppService, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(notifications, user)
        {
            _clienteAppService = clienteAppService;
        }

        [Route("Clientes")]
        public IActionResult Index()
        {
            return View(_clienteAppService.ObterPorFuncionario(FuncionarioId));
        }

        [Route("meus-clientes")]
        //[Authorize(Policy = "PodeLerEventos")]
        public IActionResult MeusEventos()
        {
            return View(_clienteAppService.ObterPorFuncionario(FuncionarioId));
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _clienteAppService.ObterPorId(id.Value);
            var dependentes = _clienteAppService.ObterPorCliente(id.Value);
            clienteViewModel.dependentes = dependentes;

            if (clienteViewModel == null)
            {
                return NotFound();
            }

            return View(clienteViewModel);
        }

        //[Authorize(Policy = "PodeGravar")]
        [Route("novo-cliente")]
        public IActionResult Create()
        {
            IEnumerable<ProfissaoViewModel> cli = new List<ProfissaoViewModel>();
            cli = _clienteAppService.ObterProfissoes();
            ViewBag.ListClientes = cli;

            return View();
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [Route("novo-cliente")]
        public IActionResult Create(ClienteViewModel clienteViewModel, string CPFX)
        {
            IEnumerable<ProfissaoViewModel> cli = new List<ProfissaoViewModel>();
            cli = _clienteAppService.ObterProfissoes();
            ViewBag.ListClientes = cli;
            clienteViewModel.CPF = CPFX;

            if (!ModelState.IsValid) return View(clienteViewModel);

            var cpf = _clienteAppService.ObterPorCpf(clienteViewModel.CPF);

            if (cpf != null)
            {
                cli = _clienteAppService.ObterProfissoes();
                ViewBag.ListClientes = cli;


                return View(clienteViewModel);
            }

            var profissao = _clienteAppService.ObterProfissaoPorId(Convert.ToInt32(clienteViewModel.Ocupacao));
            clienteViewModel.Ocupacao = profissao.profdesc;
            clienteViewModel.CodigoProf = Convert.ToInt32(profissao.profcod);
            clienteViewModel.FuncionarioId = FuncionarioId;
            _clienteAppService.Registrar(clienteViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Cliente Registrado com Sucesso!" : "error, Cliente não pode ser Registrado, verifique as mensagens";

            if (OperacaoValida())
            {
                return RedirectToAction("Details", new { id = clienteViewModel.Id });
            }
            else
            {
                return View(clienteViewModel);
            }

        }

        [Route("importa-cliente")]
        public IActionResult ImportaCliente()
        {
            return View();
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [Route("importa-cliente")]
        public IActionResult ImportaCliente(IFormFile importExcel)
        {
            ClienteViewModel cli = new ClienteViewModel();
            if (importExcel == null || importExcel.Length == 0)
            {
                return View();
            }

            using (var memoryStream = new MemoryStream())
            {
                importExcel.CopyToAsync(memoryStream).ConfigureAwait(false);

                using (var package = new ExcelPackage(memoryStream))
                {
                    for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
                    {
                        var totalRows = package.Workbook.Worksheets[i].Dimension?.Rows;
                        var totalCollumns = package.Workbook.Worksheets[i].Dimension?.Columns;
                        for (int j = 1; j <= totalRows; j++)
                        {
                            for (int k = 1; k <= totalCollumns.Value; k++)
                            {
                                cli.CPF = package.Workbook.Worksheets[i].Cells[j, k].Value == null ? "x" : package.Workbook.Worksheets[i].Cells[j, k].Value.ToString();
                                cli.Nome = package.Workbook.Worksheets[i].Cells[j, 2].Value == null ? "x" : package.Workbook.Worksheets[i].Cells[j, 2].Value.ToString();
                                cli.DataNasc = Convert.ToDateTime(package.Workbook.Worksheets[i].Cells[j, 3].Value) == null ? DateTime.Now : Convert.ToDateTime(package.Workbook.Worksheets[i].Cells[j, 3].Value);
                                cli.Sexo = package.Workbook.Worksheets[i].Cells[j, 4].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 4].Value.ToString();
                                cli.EstadoCivil = package.Workbook.Worksheets[i].Cells[j, 5].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 5].Value.ToString();
                                cli.Endereco.Logradouro = package.Workbook.Worksheets[i].Cells[j, 6].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 6].Value.ToString();
                                cli.Endereco.Numero = package.Workbook.Worksheets[i].Cells[j, 7].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 7].Value.ToString();
                                cli.Endereco.Complemento = package.Workbook.Worksheets[i].Cells[j, 8].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 8].Value.ToString();
                                cli.Endereco.Bairro = package.Workbook.Worksheets[i].Cells[j, 9].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 9].Value.ToString();
                                cli.Endereco.Cidade = package.Workbook.Worksheets[i].Cells[j, 10].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 10].Value.ToString();
                                cli.Endereco.Estado = package.Workbook.Worksheets[i].Cells[j, 11].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 11].Value.ToString();
                                cli.Endereco.CEP = package.Workbook.Worksheets[i].Cells[j, 12].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 12].Value.ToString();
                                var ddd = package.Workbook.Worksheets[i].Cells[j, 13].Value == null ? "00" : package.Workbook.Worksheets[i].Cells[j, 13].Value.ToString();
                                cli.Fone = package.Workbook.Worksheets[i].Cells[j, 14].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 14].Value.ToString();
                                cli.RG = package.Workbook.Worksheets[i].Cells[j, 15].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 15].Value.ToString();
                                cli.OrgaoExpedidor = package.Workbook.Worksheets[i].Cells[j, 16].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 16].Value.ToString();
                                cli.Email = package.Workbook.Worksheets[i].Cells[j, 17].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 17].Value.ToString();
                                cli.CodigoProf = Convert.ToInt32(package.Workbook.Worksheets[i].Cells[j, 18].Value) == null ? 1 : Convert.ToInt32(package.Workbook.Worksheets[i].Cells[j, 18].Value);
                                cli.PPE = package.Workbook.Worksheets[i].Cells[j, 19].Value == null ? "nao" : package.Workbook.Worksheets[i].Cells[j, 19].Value.ToString();
                                if (cli.PPE == "Não")
                                    cli.PPE = "nao";
                                cli.Ocupacao = package.Workbook.Worksheets[i].Cells[j, 20].Value == null ? "" : cli.Ocupacao = package.Workbook.Worksheets[i].Cells[j, 20].Value.ToString();
                                cli.FuncionarioId = FuncionarioId;
                                k = 21;

                                cli.Fone = ddd + cli.Fone;
                                cli.Celular = cli.Fone;

                                if (cli.CPF.Length == 10)
                                {
                                    cli.CPF = 0 + cli.CPF;
                                }

                                if (cli.Endereco.CEP.Length == 7)
                                {
                                    cli.Endereco.CEP = 0 + cli.Endereco.CEP;
                                }

                                if (cli.Endereco.CEP.Contains("-"))
                                {
                                    var x = cli.Endereco.CEP.Split("-");
                                    var cep = x[0] + x[1];
                                    cli.Endereco.CEP = cep;
                                }

                                ClienteViewModel cliente = new ClienteViewModel();
                                cliente = _clienteAppService.ObterPorCpf(cli.CPF);

                                if (cliente != null)
                                {
                                    _clienteAppService.Atualizar(cliente);
                                    cliente.Id = Guid.NewGuid();
                                }
                                else
                                {
                                    _clienteAppService.Registrar(cli);
                                }

                                cli.Id = Guid.NewGuid();
                                cli.Endereco.Id = Guid.NewGuid();
                            }
                        }
                    }
                }
            }
            //IEnumerable<ProfissaoViewModel> cli = new List<ProfissaoViewModel>();
            //cli = _clienteAppService.ObterProfissoes();
            //ViewBag.ListClientes = cli;
            //clienteViewModel.CPF = CPFX;

            //if (!ModelState.IsValid) return View(clienteViewModel);

            //var cpf = _clienteAppService.ObterPorCpf(clienteViewModel.CPF);

            //if (cpf != null)
            //{
            //    cli = _clienteAppService.ObterProfissoes();
            //    ViewBag.ListClientes = cli;


            //    return View(clienteViewModel);
            //}

            //var profissao = _clienteAppService.ObterProfissaoPorId(Convert.ToInt32(clienteViewModel.Ocupacao));
            //clienteViewModel.Ocupacao = profissao.profdesc;
            //clienteViewModel.CodigoProf = Convert.ToInt32(profissao.profcod);
            //clienteViewModel.FuncionarioId = FuncionarioId;
            //_clienteAppService.Registrar(clienteViewModel);

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

        [Route("importa-dependente")]
        public IActionResult ImportaDependentes()
        {
            return View();
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [Route("importa-dependente")]
        public IActionResult ImportaDependentes(IFormFile importExcel)
        {
            DependenteViewModel cli = new DependenteViewModel();
            if (importExcel == null || importExcel.Length == 0)
            {
                return View();
            }

            using (var memoryStream = new MemoryStream())
            {
                importExcel.CopyToAsync(memoryStream).ConfigureAwait(false);

                using (var package = new ExcelPackage(memoryStream))
                {
                    for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
                    {
                        var totalRows = package.Workbook.Worksheets[i].Dimension?.Rows;
                        var totalCollumns = package.Workbook.Worksheets[i].Dimension?.Columns;
                        for (int j = 1; j <= totalRows; j++)
                        {
                            for (int k = 1; k <= totalCollumns.Value; k++)
                            {
                                var cpf = package.Workbook.Worksheets[i].Cells[j, k].Value == null ? "x" : package.Workbook.Worksheets[i].Cells[j, k].Value.ToString();
                                cli.Nome = package.Workbook.Worksheets[i].Cells[j, 2].Value == null ? "x" : package.Workbook.Worksheets[i].Cells[j, 2].Value.ToString();
                                cli.Parentesco = package.Workbook.Worksheets[i].Cells[j, 3].Value == null ? "x" : package.Workbook.Worksheets[i].Cells[j, 3].Value.ToString();
                                cli.Participacao = package.Workbook.Worksheets[i].Cells[j, 4].Value == null ? 0 : Convert.ToDecimal(package.Workbook.Worksheets[i].Cells[j, 4].Value);

                                if (cpf.Length == 10)
                                {
                                    cpf = 0 + cpf;
                                }

                                k = 21;

                                ClienteViewModel cliente = new ClienteViewModel();
                                cliente = _clienteAppService.ObterPorCpf(cpf);

                                if (cliente != null)
                                {
                                    cli.ClienteId = cliente.Id;
                                    _clienteAppService.AdicionarDependente(cli);
                                }

                                cli.Id = Guid.NewGuid();
                            }
                        }
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

        // [Authorize(Policy = "PodeGravar")]
        [HttpGet]
        [Route("editar-cliente/{id=guid}")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _clienteAppService.ObterPorId(id.Value);

            if (clienteViewModel == null)
            {
                return NotFound();
            }


            return View(clienteViewModel);
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-cliente/{id=guid}")]
        public IActionResult Edit(ClienteViewModel clienteViewModel)
        {
            var x = _clienteAppService.ObterPorId(clienteViewModel.Id);
            clienteViewModel.Endereco = x.Endereco;

            if (!ModelState.IsValid) return View(clienteViewModel);

            clienteViewModel.FuncionarioId = FuncionarioId;
            _clienteAppService.Atualizar(clienteViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Cliente atualizado com Sucesso!" : "error, Cliente não pode ser atualizado, verifique as mensagens";

            return View(clienteViewModel);
        }


        [Route("importa-total")]
        public IActionResult ImportaClienteDep()
        {
            return View();
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [Route("importa-total")]
        public IActionResult ImportaClienteDep(IFormFile importExcel)
        {
            ClienteViewModel cli = new ClienteViewModel();
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
                        cli.CPF = package.Workbook.Worksheets[i].Cells[j, k].Value == null ? "x" : package.Workbook.Worksheets[i].Cells[j, k].Value.ToString();
                        cli.Nome = package.Workbook.Worksheets[i].Cells[j, 2].Value == null ? "x" : package.Workbook.Worksheets[i].Cells[j, 2].Value.ToString();
                        cli.DataNasc = Convert.ToDateTime(package.Workbook.Worksheets[i].Cells[j, 3].Value) == null ? DateTime.Now : Convert.ToDateTime(package.Workbook.Worksheets[i].Cells[j, 3].Value);
                        cli.Sexo = package.Workbook.Worksheets[i].Cells[j, 4].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 4].Value.ToString();
                        cli.EstadoCivil = package.Workbook.Worksheets[i].Cells[j, 5].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 5].Value.ToString();
                        cli.Endereco.Logradouro = package.Workbook.Worksheets[i].Cells[j, 6].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 6].Value.ToString();
                        cli.Endereco.Numero = package.Workbook.Worksheets[i].Cells[j, 7].Value == null ? "001" : package.Workbook.Worksheets[i].Cells[j, 7].Value.ToString();
                        cli.Endereco.Complemento = package.Workbook.Worksheets[i].Cells[j, 8].Value == null ? "Rua" : package.Workbook.Worksheets[i].Cells[j, 8].Value.ToString();
                        cli.Endereco.Bairro = package.Workbook.Worksheets[i].Cells[j, 9].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 9].Value.ToString();
                        cli.Endereco.Cidade = package.Workbook.Worksheets[i].Cells[j, 10].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 10].Value.ToString();
                        cli.Endereco.Estado = package.Workbook.Worksheets[i].Cells[j, 11].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 11].Value.ToString();
                        cli.Endereco.CEP = package.Workbook.Worksheets[i].Cells[j, 12].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 12].Value.ToString();
                        var ddd = package.Workbook.Worksheets[i].Cells[j, 13].Value == null ? "00" : package.Workbook.Worksheets[i].Cells[j, 13].Value.ToString();
                        cli.Fone = package.Workbook.Worksheets[i].Cells[j, 14].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 14].Value.ToString();
                        cli.RG = package.Workbook.Worksheets[i].Cells[j, 15].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 15].Value.ToString();
                        cli.OrgaoExpedidor = package.Workbook.Worksheets[i].Cells[j, 16].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 16].Value.ToString();
                        cli.Email = package.Workbook.Worksheets[i].Cells[j, 17].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 17].Value.ToString();
                        cli.CodigoProf = Convert.ToInt32(package.Workbook.Worksheets[i].Cells[j, 18].Value) == null ? 1 : Convert.ToInt32(package.Workbook.Worksheets[i].Cells[j, 18].Value);
                        cli.PPE = package.Workbook.Worksheets[i].Cells[j, 19].Value == null ? "nao" : package.Workbook.Worksheets[i].Cells[j, 19].Value.ToString();
                        if (cli.PPE == "Não")
                            cli.PPE = "nao";
                        cli.Ocupacao = package.Workbook.Worksheets[i].Cells[j, 20].Value == null ? "" : cli.Ocupacao = package.Workbook.Worksheets[i].Cells[j, 20].Value.ToString();
                        cli.FuncionarioId = FuncionarioId;

                        k = 99;

                        cli.Fone = ddd + cli.Fone;
                        cli.Celular = cli.Fone;

                        if(cli.Endereco.Complemento == " ")
                        {
                            cli.Endereco.Complemento = "Rua";
                        }

                        if (cli.CPF.Length == 10)
                        {
                            cli.CPF = 0 + cli.CPF;
                        }

                        if (cli.CPF.Length == 9)
                        {
                            cli.CPF = 00 + cli.CPF;
                        }

                        if (cli.CPF.Length == 8)
                        {
                            cli.CPF = 000 + cli.CPF;
                        }

                        if (cli.Endereco.CEP.Length == 7)
                        {
                            cli.Endereco.CEP = 0 + cli.Endereco.CEP;
                        }


                        if (cli.Endereco.CEP.Contains("-"))
                        {
                            var x = cli.Endereco.CEP.Split("-");
                            var cep = x[0] + x[1];
                            cli.Endereco.CEP = cep;
                        }
                        var cepSemEspaco = cli.Endereco.CEP.Replace(" ", "");
                        cli.Endereco.CEP = cepSemEspaco;

                        ClienteViewModel cliente = new ClienteViewModel();
                        cliente = _clienteAppService.ObterPorCpf(cli.CPF);

                        if (cliente != null)
                        {
                            //_clienteAppService.Atualizar(cliente);
                            cliente.Id = Guid.NewGuid();
                        }
                        else
                        {
                            cli.Id = Guid.NewGuid();
                            _clienteAppService.Registrar(cli);



                            DependenteViewModel dep1 = new DependenteViewModel();
                            DependenteViewModel dep2 = new DependenteViewModel();
                            DependenteViewModel dep3 = new DependenteViewModel();
                            DependenteViewModel dep4 = new DependenteViewModel();

                            dep1.Nome = package.Workbook.Worksheets[i].Cells[j, 21].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 21].Value.ToString();
                            dep1.Parentesco = package.Workbook.Worksheets[i].Cells[j, 22].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 22].Value.ToString();
                            dep1.Participacao = package.Workbook.Worksheets[i].Cells[j, 23].Value == null ? 0 : Convert.ToDecimal(package.Workbook.Worksheets[i].Cells[j, 23].Value);

                            dep2.Nome = package.Workbook.Worksheets[i].Cells[j, 24].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 24].Value.ToString();
                            dep2.Parentesco = package.Workbook.Worksheets[i].Cells[j, 25].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 25].Value.ToString();
                            dep2.Participacao = package.Workbook.Worksheets[i].Cells[j, 26].Value == null ? 0 : Convert.ToDecimal(package.Workbook.Worksheets[i].Cells[j, 26].Value);

                            dep3.Nome = package.Workbook.Worksheets[i].Cells[j, 27].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 27].Value.ToString();
                            dep3.Parentesco = package.Workbook.Worksheets[i].Cells[j, 28].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 28].Value.ToString();
                            dep3.Participacao = package.Workbook.Worksheets[i].Cells[j, 29].Value == null ? 0 : Convert.ToDecimal(package.Workbook.Worksheets[i].Cells[j, 29].Value);

                            dep4.Nome = package.Workbook.Worksheets[i].Cells[j, 30].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 30].Value.ToString();
                            dep4.Parentesco = package.Workbook.Worksheets[i].Cells[j, 31].Value == null ? "" : package.Workbook.Worksheets[i].Cells[j, 31].Value.ToString();
                            dep4.Participacao = package.Workbook.Worksheets[i].Cells[j, 32].Value == null ? 0 : Convert.ToDecimal(package.Workbook.Worksheets[i].Cells[j, 32].Value);

                            List<DependenteViewModel> deps = new List<DependenteViewModel>();
                            deps.Add(dep1);
                            deps.Add(dep2);
                            deps.Add(dep3);
                            deps.Add(dep4);

                            foreach (var item in deps)
                            {
                                if (item.Nome != "")
                                {
                                    item.Id = Guid.NewGuid();
                                    item.ClienteId = cliente == null ? cli.Id : cliente.Id;

                                    _clienteAppService.AdicionarDependente(item);
                                    item.Id = Guid.NewGuid();
                                }
                            }

                        }

                        cli.Id = Guid.NewGuid();
                        cli.Endereco.Id = Guid.NewGuid();
                    }
                }
            }

            //IEnumerable<ProfissaoViewModel> cli = new List<ProfissaoViewModel>();
            //cli = _clienteAppService.ObterProfissoes();
            //ViewBag.ListClientes = cli;
            //clienteViewModel.CPF = CPFX;

            //if (!ModelState.IsValid) return View(clienteViewModel);

            //var cpf = _clienteAppService.ObterPorCpf(clienteViewModel.CPF);

            //if (cpf != null)
            //{
            //    cli = _clienteAppService.ObterProfissoes();
            //    ViewBag.ListClientes = cli;


            //    return View(clienteViewModel);
            //}

            //var profissao = _clienteAppService.ObterProfissaoPorId(Convert.ToInt32(clienteViewModel.Ocupacao));
            //clienteViewModel.Ocupacao = profissao.profdesc;
            //clienteViewModel.CodigoProf = Convert.ToInt32(profissao.profcod);
            //clienteViewModel.FuncionarioId = FuncionarioId;
            //_clienteAppService.Registrar(clienteViewModel);

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
        [Route("excluir-cliente/{id=guid}")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _clienteAppService.ObterPorId(id.Value);


            if (clienteViewModel == null)
            {
                return NotFound();
            }

            return View(clienteViewModel);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-cliente/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public IActionResult DeleteConfirmed(Guid id)
        {

            _clienteAppService.Excluir(id);
            return RedirectToAction("Index");
        }

        [Route("incluir-endereco/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public ActionResult IncluirEndereco(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _clienteAppService.ObterPorId(id.Value);
            return PartialView("_IncluirEndereco", clienteViewModel);
        }

        //[Authorize(Policy = "PodeGravar")]
        [Route("atualizar-endereco/{id=guid}")]
        public ActionResult AtualizarEndereco(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _clienteAppService.ObterPorId(id.Value);
            return PartialView("_AtualizarEndereco", clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("incluir-endereco/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public IActionResult IncluirEndereco(ClienteViewModel clienteViewModel)
        {
            ModelState.Clear();
            clienteViewModel.Endereco.ClienteId = clienteViewModel.Id;
            _clienteAppService.AdicionarEndereco(clienteViewModel.Endereco);

            if (OperacaoValida())
            {
                string url = Url.Action("ObterEndereco", "Clientes", new { id = clienteViewModel.Id });
                return Json(new { succes = true, url = url });
            }

            return PartialView("_IncluirEndereco", clienteViewModel);
        }

        public ActionResult IncluirDependente(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _clienteAppService.ObterPorId(id.Value);
            return PartialView("_IncluirDependente", clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IncluirDependente(ClienteViewModel clienteViewModel)
        {
            ModelState.Clear();
            clienteViewModel.Dependente.ClienteId = clienteViewModel.Id;
            clienteViewModel.Dependente.Participacao = Convert.ToDecimal(clienteViewModel.Dependente.Participacao);
            _clienteAppService.AdicionarDependente(clienteViewModel.Dependente);

            //if (OperacaoValida())
            //{
            //    string url = Url.Action("ObterEndereco", "Clientes", new { id = dependente.Id });
            //    return Json(new { succes = true, url = url });
            //}

            return RedirectToAction("Details", new { id = clienteViewModel.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("atualizar-endereco/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public IActionResult AtualizarEndereco(ClienteViewModel clienteViewModel)
        {
            ModelState.Clear();
            _clienteAppService.AtualizarEndereco(clienteViewModel.Endereco);

            if (OperacaoValida())
            {
                string url = Url.Action("ObterEndereco", "Clientes", new { id = clienteViewModel.Id });
                return Json(new { succes = true, url = url });
            }

            return PartialView("_AtualizarEndereco", clienteViewModel);
        }

        [Route("listar-endereco/{id=guid}")]
        public IActionResult ObterEndereco(Guid id)
        {
            return PartialView("_DetalhesEndereco", _clienteAppService.ObterPorId(id));
        }

        public IActionResult VerificaCPF()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerificaCPF(string CPF)
        {
            if (!String.IsNullOrEmpty(CPF))
            {
                var cpf = _clienteAppService.ObterPorCpf(CPF);

                if (cpf != null)
                {
                    //var result = new ObjectResult(new { erro = "CPF já cadastrado" });

                    return View("VerificaCPF");
                }
                else
                {
                    IEnumerable<ProfissaoViewModel> cli = new List<ProfissaoViewModel>();
                    cli = _clienteAppService.ObterProfissoes();
                    ViewBag.ListClientes = cli;

                    ViewBag.Cpf = CPF;
                    return View("Create");
                }
            }
            else
            {
                var result = new ObjectResult(new { erro = "CPF não inserido" });
                result.StatusCode = 401;
                return View("VerificaCPF", result);
            }

        }
    }
}