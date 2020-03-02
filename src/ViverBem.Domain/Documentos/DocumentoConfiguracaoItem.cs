using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Documentos
{
    public class DocumentoConfiguracaoItem : Entity<DocumentoConfiguracaoItem>
    {
        public string NoFonte { get; private set; }
        public string NoFonteColor { get; private set; }

        public string NoFrase { get; private set; }
        public bool FlgPropriedade { get; private set; }
        public Int32 NrColuna { get; private set; }

        [Key]
        public Guid NrseqDocumentoConfiguracaoItem { get; private set; }

        public Int32 NrFonteTamanho { get; private set; }

        public string NoTipoFonte { get; private set; }

        public Guid? NrSeqDocumentoConfiguracao { get; private set; }

        public bool FlgIdentificador { get; private set; }

        public bool FlgImage { get; private set; }

        public int NrMaxImageWidth { get; private set; }

        public bool Excluido { get; private set; }
        public int NrMaxImageHeight { get; private set; }
        public virtual DocumentoConfiguracao DocConfiguracao { get; private set; }
        public void ExcluirDocumentoItem()
        {
            Excluido = true;
        }

        public DocumentoConfiguracaoItem(string nofonte, string nofontecolor, string nofrase, bool flgpropriedade, Int32 nrcoluna, Guid nrseqdocumentoconfiguracaoitem, Int32 nrfontetamanho,
            string notipofonte, Guid? nrseqdocumentoconfiguracao, bool flgidentificador, bool flgimage, Int32 nrmaximagewidth, Int32 nrmaximageheight)
        {
            NoFonte = nofonte;
            NoFonteColor = nofontecolor;
            NoFrase = nofrase;
            FlgPropriedade = flgpropriedade;
            NrColuna = nrcoluna;
            NrseqDocumentoConfiguracaoItem = nrseqdocumentoconfiguracaoitem;
            NrFonteTamanho = nrfontetamanho;
            NoTipoFonte = notipofonte;
            NrSeqDocumentoConfiguracao = nrseqdocumentoconfiguracao;
            FlgIdentificador = flgidentificador;
            FlgImage = flgimage;
            NrMaxImageWidth = nrmaximagewidth;
            NrMaxImageHeight = nrmaximageheight;
            Id = nrseqdocumentoconfiguracaoitem;
        }

        private DocumentoConfiguracaoItem()
        {
        }

        public override bool EhValido()
        {
            return true;
        }

        public static class DocumentoConfiguracaoItemFactory
        {
            public static DocumentoConfiguracaoItem NovoDocumentoItemCompleto(string nofonte, string nofontecolor, string nofrase, bool flgpropriedade, Int32 nrcoluna, Guid nrseqdocumentoconfiguracaoitem, Int32 nrfontetamanho,
            string notipofonte, Guid? nrseqdocumentoconfiguracao, bool flgidentificador, bool flgimage, Int32 nrmaximagewidth, Int32 nrmaximageheight)
            {
                var doc = new DocumentoConfiguracaoItem()
                {
                    NoFonte = nofonte,
                    NoFonteColor = nofontecolor,
                    NoFrase = nofrase,
                    FlgPropriedade = flgpropriedade,
                    NrColuna = nrcoluna,
                    NrseqDocumentoConfiguracaoItem = nrseqdocumentoconfiguracaoitem,
                    NrFonteTamanho = nrfontetamanho,
                    NoTipoFonte = notipofonte,
                    NrSeqDocumentoConfiguracao = nrseqdocumentoconfiguracao,
                    FlgIdentificador = flgidentificador,
                    FlgImage = flgimage,
                    NrMaxImageWidth = nrmaximagewidth,
                    NrMaxImageHeight = nrmaximageheight,
                };
                return doc;
            }
        }
    }
}
