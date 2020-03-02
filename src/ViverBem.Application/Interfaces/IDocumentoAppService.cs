using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Application.ViewModels;

namespace ViverBem.Application.Interfaces
{
    public interface IDocumentoAppService : IDisposable
    {
        void Registrar(DocumentoConfiguracaoViewModel documentoConfiguracao);
        IEnumerable<DocumentoConfiguracaoViewModel> ObterTodos();
        DocumentoConfiguracaoViewModel ObterPorId(Guid id);
        void Atualizar(DocumentoConfiguracaoViewModel documentoConfiguracao);
        void Excluir(Guid id);
        void AdicionarDocumentoItem(DocumentoConfiguracaoItemViewModel documentoItemViewModel);
        void AtualizarDocumentoItem(DocumentoConfiguracaoItemViewModel documentoItemViewModel);
        IEnumerable<DocumentoConfiguracaoItemViewModel> ObterPorDocumento(Guid documentoID);
    }
}
