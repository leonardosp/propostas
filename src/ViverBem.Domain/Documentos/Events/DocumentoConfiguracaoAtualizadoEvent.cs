using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Documentos.Events
{
    public class DocumentoConfiguracaoAtualizadoEvent : BaseDocumentoConfiguracaoEvent
    {
        public DocumentoConfiguracaoAtualizadoEvent(Guid nrseqdocumentoconfiguracao, string cddocumentoconfiguracao, Int32 nrordem, Int32 nrcoluna, string nobackgroundcolor, string nohorizontalaligment
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
    }
}
