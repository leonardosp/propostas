using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Documentos.Commands
{
    public class ExcluirDocumentoConfiguracaoItemCommand : BaseDocumentoConfiguracaoItemCommand
    {
        public ExcluirDocumentoConfiguracaoItemCommand(Guid id)
        {
            NrseqDocumentoConfiguracaoItem = id;
            AggregateId = id;
        }
    }
}
