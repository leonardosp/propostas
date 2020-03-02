using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Documentos.Events
{
    public class DocumentoConfiguracaoItemExcluidoEvent : BaseDocumentoConfiguracaoItemEvent
    {
        public DocumentoConfiguracaoItemExcluidoEvent(Guid id)
        {
            NrseqDocumentoConfiguracaoItem = id;
            AggregateId = id;
        }
    }
}
