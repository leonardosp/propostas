using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Documentos.Events
{
    public class BaseDocumentoConfiguracaoItemEvent : Event
    {
        public string NoFonte { get; protected set; }
        public string NoFonteColor { get; protected set; }

        public string NoFrase { get; protected set; }
        public bool FlgPropriedade { get; protected set; }
        public Int32 NrColuna { get; protected set; }

        public Guid NrseqDocumentoConfiguracaoItem { get; protected set; }

        public Int32 NrFonteTamanho { get; protected set; }

        public string NoTipoFonte { get; protected set; }

        public Guid? NrSeqDocumentoConfiguracao { get; protected set; }

        public bool FlgIdentificador { get; protected set; }

        public bool FlgImage { get; protected set; }

        public int NrMaxImageWidth { get; protected set; }

        public int NrMaxImageHeight { get; protected set; }
    }
}
