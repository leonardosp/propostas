using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Documentos.Commands
{
    public class ExcluirDocumentoConfiguracaoCommand : BaseDocumentoConfiguracaoCommand
    {
        public ExcluirDocumentoConfiguracaoCommand(Guid id)
        {
            NrSeqDocumentoConfiguracao = id;
            AggregateId = id;
        }
    }
}
