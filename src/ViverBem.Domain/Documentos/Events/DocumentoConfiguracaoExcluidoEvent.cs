using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Documentos.Events
{
    public class DocumentoConfiguracaoExcluidoEvent : BaseDocumentoConfiguracaoEvent
    {
        public DocumentoConfiguracaoExcluidoEvent(Guid id)
        {
            NrSeqDocumentoConfiguracao = id;
            AggregateId = id;
        }
    }
}
