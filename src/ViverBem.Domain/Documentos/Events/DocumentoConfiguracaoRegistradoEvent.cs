using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Documentos.Events
{
    public class DocumentoConfiguracaoRegistradoEvent : Event
    {
        public DocumentoConfiguracaoRegistradoEvent(Guid nrseqdocumentoconfiguracao, string cddocumentoconfiguracao, Int32 nrordem, Int32 nrcoluna, string nobackgroundcolor, string nohorizontalaligment
            , Int32 nrtamanhoborda, Int32 nrmarginleft, string nopropriedadelista, int nrparagraphspacing, int nrwidth)
        {
            NrSeqDocumentoConfiguracao = nrseqdocumentoconfiguracao;
            CdDocumentoConfiguracao = cddocumentoconfiguracao;
            NrOrdem = nrordem;
            NrColuna = nrcoluna;
            NoBackGroundColor = nobackgroundcolor;
            NoHorizontalAligment = nohorizontalaligment;
            NrTamanhoBorda = nrtamanhoborda;
            NrMarginLeft = NrMarginLeft;
            NoPropriedadeLista = nopropriedadelista;
            NrParagraphspacing = nrparagraphspacing;
            NrWidth = nrwidth;
        }

        public Guid NrSeqDocumentoConfiguracao { get; private set; }
        public string CdDocumentoConfiguracao { get; private set; }
        public Int32 NrOrdem { get; private set; }
        public Int32 NrColuna { get; private set; }
        public string NoBackGroundColor { get; private set; }
        public string NoHorizontalAligment { get; private set; }
        public Int32 NrTamanhoBorda { get; private set; }
        public Int32 NrMarginLeft { get; private set; }
        public string NoPropriedadeLista { get; private set; }
        public int NrParagraphspacing { get; private set; }
        public int NrWidth { get; private set; }
    }
}
