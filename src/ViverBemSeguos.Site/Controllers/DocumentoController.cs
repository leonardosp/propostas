using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Interfaces;

namespace ViverBemSeguos.Site.Controllers
{
    [Authorize]
    public class DocumentoController : BaseController
    {
        private readonly IDocumentoAppService _documentoAppService;


        public DocumentoController(IDocumentoAppService documentoAppService, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(notifications, user)
        {
            _documentoAppService = documentoAppService;
        }

        [Route("Documentos")]
        public IActionResult Index()
        {
            return View(_documentoAppService.ObterTodos());
        }


        [Route("dados-do-documento/{id:guid}")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentoViewModel = _documentoAppService.ObterPorId(id.Value);
            var documentoItemViewModel = _documentoAppService.ObterPorDocumento(id.Value);
            documentoViewModel.DocumentoConfiguracaoItems = documentoItemViewModel;
            if (documentoViewModel == null)
            {
                return NotFound();
            }

            return View(documentoViewModel);
        }

        //[Authorize(Policy = "PodeGravar")]
        [Route("novo-doc")]
        public IActionResult Create()
        {
            return View();
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-doc")]
        public IActionResult Create(DocumentoConfiguracaoViewModel documentoViewModel)
        {

            if (!ModelState.IsValid) return View(documentoViewModel);

            _documentoAppService.Registrar(documentoViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Documento Registrado com Sucesso!" : "error, Documento não pode ser Registrado, verifique as mensagens";

            return View(documentoViewModel);
        }

        // [Authorize(Policy = "PodeGravar")]
        [Route("editar-evento/{id=guid}")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentoConfiguracaoViewModel = _documentoAppService.ObterPorId(id.Value);

            if (documentoConfiguracaoViewModel == null)
            {
                return NotFound();
            }


            return View(documentoConfiguracaoViewModel);
        }

        // [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("editar-evento/{id=guid}")]
        public IActionResult Edit(DocumentoConfiguracaoViewModel documentoViewModel)
        {

            if (!ModelState.IsValid) return View(documentoViewModel);

            _documentoAppService.Atualizar(documentoViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "succes, Documento atualizado com Sucesso!" : "error, Documento não pode ser atualizado, verifique as mensagens";


            return View(documentoViewModel);
        }


        //[Authorize(Policy = "PodeGravar")]
        [Route("excluir-documento/{id=guid}")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docViewModel = _documentoAppService.ObterPorId(id.Value);


            if (docViewModel == null)
            {
                return NotFound();
            }

            return View(docViewModel);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-documento/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public IActionResult DeleteConfirmed(Guid id)
        {

            _documentoAppService.Excluir(id);
            return RedirectToAction("Index");
        }

        [Route("incluir-docItem/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public ActionResult IncluirDocumentoItem(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docViewModel = _documentoAppService.ObterPorId(id.Value);
            return PartialView("_Docs", docViewModel);
        }

        //[Authorize(Policy = "PodeGravar")]
        [Route("atualizar-docItem/{id=guid}")]
        public ActionResult AtualizarDocitem(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentoViewModel = _documentoAppService.ObterPorId(id.Value);
            return PartialView("Details", documentoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("incluir-docItem/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public IActionResult IncluirDocumentoItem(DocumentoConfiguracaoViewModel documentoConfiguracaoViewModel)
        {
            ModelState.Clear();
            documentoConfiguracaoViewModel.DocumentoConfiguracaoItem.NrSeqDocumentoConfiguracao = documentoConfiguracaoViewModel.NrSeqDocumentoConfiguracao;
            _documentoAppService.AdicionarDocumentoItem(documentoConfiguracaoViewModel.DocumentoConfiguracaoItem);

            //if (OperacaoValida())
            //{
            //    string url = Url.Action("ObterDocumentoItem", "Documentos", new { id = documentoConfiguracaoViewModel.NrSeqDocumentoConfiguracao });
            //    return Json(new { succes = true, url = url });
            //}

            return PartialView("Details", documentoConfiguracaoViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("atualizar-docItem/{id=guid}")]
        //[Authorize(Policy = "PodeGravar")]
        public IActionResult AtualizardocItem(DocumentoConfiguracaoViewModel documentoConfiguracaoViewModel)
        {
            ModelState.Clear();
            _documentoAppService.AtualizarDocumentoItem(documentoConfiguracaoViewModel.DocumentoConfiguracaoItem);

            if (OperacaoValida())
            {
                string url = Url.Action("ObterDocumentoItem", "Documentos", new { id = documentoConfiguracaoViewModel.NrSeqDocumentoConfiguracao });
                return Json(new { succes = true, url = url });
            }

            return PartialView("_AtualizarDocumentoItem", documentoConfiguracaoViewModel);
        }

        [Route("listar-docItem/{id=guid}")]
        public IActionResult ObterEndereco(Guid id)
        {
            return PartialView("_DetalhesDocItem", _documentoAppService.ObterPorId(id));
        }
    }
}