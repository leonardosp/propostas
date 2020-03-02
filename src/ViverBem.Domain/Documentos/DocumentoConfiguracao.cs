using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Documentos
{
    public class DocumentoConfiguracao : Entity<DocumentoConfiguracao>
    {
        public DocumentoConfiguracao(Guid nrseqdocumentoconfiguracao, string cddocumentoconfiguracao, Int32 nrordem, Int32 nrcoluna, string nobackgroundcolor, string nohorizontalaligment
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

        private DocumentoConfiguracao()
        {

        }

        public virtual ICollection<DocumentoConfiguracaoItem> GetDocumentoConfiguracaoItems { get; private set; }
        public Guid NrSeqDocumentoConfiguracao { get; private set; }
        public string CdDocumentoConfiguracao { get; private set; }
        public Int32 NrOrdem { get; private set; }
        public Int32 NrColuna { get; private set; }
        public string NoBackGroundColor { get; private set; }
        public void AtribuirDocumentoItem(DocumentoConfiguracaoItem documentoItem)
        {
            if (!documentoItem.EhValido()) return;
            DocumentoItem = documentoItem;
        }
        public string NoHorizontalAligment { get; private set; }
        public Int32 NrTamanhoBorda { get; private set; }
        public Int32 NrMarginLeft { get; private set; }
        public bool Excluido { get; private set; }
        public string  NoPropriedadeLista{ get; private set; }
        public int NrParagraphspacing { get; private set; }
        public int NrWidth { get; private set; }
        public void ExcluirDocumento()
        {
            Excluido = true;
        }
        public virtual DocumentoConfiguracaoItem DocumentoItem { get; set; }

  

        public override bool EhValido()
        {
            return true;
        }

        public static class DocumentoConfiguracaoFactory
        {
            public static DocumentoConfiguracao NovoDocumentoCompleto(Guid nrseqdocumentoconfiguracao, string cddocumentoconfiguracao, Int32 nrordem, Int32 nrcoluna, string nobackgroundcolor, string nohorizontalaligment
            , Int32 nrtamanhoborda, Int32 nrmarginleft, string nopropriedadelista, int nrparagraphspacing, int nrwidth)
            {
                var doc = new DocumentoConfiguracao()
                {
                NrSeqDocumentoConfiguracao = nrseqdocumentoconfiguracao,
                CdDocumentoConfiguracao = cddocumentoconfiguracao,
                NrOrdem = nrordem,
                NrColuna = nrcoluna,
                NoBackGroundColor = nobackgroundcolor,
                NoHorizontalAligment = nohorizontalaligment,
                NrTamanhoBorda = nrtamanhoborda,
                NrMarginLeft = nrmarginleft,
                NoPropriedadeLista = nopropriedadelista,
                NrParagraphspacing = nrparagraphspacing,
                NrWidth = nrwidth,
            };
                return doc;
            }
        }
    }
}
