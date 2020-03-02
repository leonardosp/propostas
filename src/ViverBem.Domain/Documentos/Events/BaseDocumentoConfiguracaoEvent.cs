using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Documentos.Events
{
    public class BaseDocumentoConfiguracaoEvent : Event
    {
        public Guid NrSeqDocumentoConfiguracao { get; protected set; }
        public string CdDocumentoConfiguracao { get; protected set; }
        public Int32 NrOrdem { get; protected set; }
        public Int32 NrColuna { get; protected set; }
        public string NoBackGroundColor { get; protected set; }
        public string NoHorizontalAligment { get; protected set; }
        public Int32 NrTamanhoBorda { get; protected set; }
        public Int32 NrMarginLeft { get; protected set; }
        public string NoPropriedadeLista { get; protected set; }
        public int NrParagraphspacing { get; protected set; }
        public int NrWidth { get; protected set; }
    }
}
