using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Interfaces;

namespace ViverBemSeguos.Site.Controllers
{
    public class CombosController : BaseController
    {
        private readonly IComboAppService _comboAppService;

        public CombosController(IComboAppService comboAppService, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(notifications, user)
        {
            _comboAppService = comboAppService;
        }

        [Route("Planos")]
        public IActionResult Index()
        {
            return View(_comboAppService.ObterTodos());
        }

        [Route("novo-combo")]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-combo")]
        public IActionResult Create(ComboPrincViewModel comboViewModel)
        {

            if (!ModelState.IsValid) return View(comboViewModel);

            _comboAppService.Registrar(comboViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Combo Registrado com Sucesso!" : "error, Combo não pode ser Registrado, verifique as mensagens";

            return View("Index",comboViewModel);
        }

        [Route("editar-combo/{id=guid}")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _comboAppService.ObterPorId(id.Value);

            if (clienteViewModel == null)
            {
                return NotFound();
            }


            return View(clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("editar-evento/{id=guid}")]
        public IActionResult Edit(ComboPrincViewModel comboViewModel)
        {

            if (!ModelState.IsValid) return View(comboViewModel);

            _comboAppService.Atualizar(comboViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Combo atualizado com Sucesso!" : "error, Combo não pode ser atualizado, verifique as mensagens";


            return View(comboViewModel);
        }

        [Route("dados-do-combo/{id:guid}")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _comboAppService.ObterPorId(id.Value);
            var dependentes = _comboAppService.ObterPorComboPrinc(id.Value);

                clienteViewModel.CombosItems = dependentes;
     


            if (clienteViewModel == null)
            {
                return NotFound();
            }

            return View(clienteViewModel);
        }

        [Route("excluir-combo/{id=guid}")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _comboAppService.ObterPorId(id.Value);


            if (clienteViewModel == null)
            {
                return NotFound();
            }

            return View(clienteViewModel);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-combo/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public IActionResult DeleteConfirmed(Guid id)
        {

            _comboAppService.Excluir(id);
            return RedirectToAction("Index");
        }

        public ActionResult IncluirCombo(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteViewModel = _comboAppService.ObterPorId(id.Value);
            return PartialView("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IncluirCombo(ComboPrincViewModel clienteViewModel)
        {
            ModelState.Clear();
            clienteViewModel.Combos.ComboPrincId = clienteViewModel.Id;
            _comboAppService.AdicionarComboItem(clienteViewModel.Combos);

            //if (OperacaoValida())
            //{
            //    string url = Url.Action("ObterEndereco", "Clientes", new { id = dependente.Id });
            //    return Json(new { succes = true, url = url });
            //}

            return View("Index",_comboAppService.ObterTodos());
        }
    }
}