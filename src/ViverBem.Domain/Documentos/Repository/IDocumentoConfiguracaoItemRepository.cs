using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Documentos.Repository
{
    public interface IDocumentoConfiguracaoItemRepository : IRepository<DocumentoConfiguracaoItem>
    {
        IEnumerable<DocumentoConfiguracaoItem> ObterDocumentoItem(Guid nrseqDocumentoConfiguracao);
        void AdicionarDocumentoItem(DocumentoConfiguracaoItem docItem);
    }
}
