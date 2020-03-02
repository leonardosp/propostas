using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Documentos.Repository
{
    public interface IDocumentoConfiguracaoRepository : IRepository<DocumentoConfiguracao>
    {
        IEnumerable<DocumentoConfiguracao> ObterDocumento(Guid documentoId);
        IEnumerable<DocumentoConfiguracaoItem> ObterDocumentoItemPorDocumento(Guid documentoId);
        DocumentoConfiguracaoItem ObterDocumentoItemPorId(Guid id);
        void AdicionarDocumentoItem(DocumentoConfiguracaoItem documentoItem);
        void AtualizarDocumentoItem(DocumentoConfiguracaoItem documentoItem);

    }
}
