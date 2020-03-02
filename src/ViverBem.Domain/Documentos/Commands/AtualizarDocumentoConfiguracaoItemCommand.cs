using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Documentos.Commands
{
    public class AtualizarDocumentoConfiguracaoItemCommand : Command
    {
        public string NoFonte { get; private set; }
        public string NoFonteColor { get; private set; }

        public string NoFrase { get; private set; }
        public bool FlgPropriedade { get; private set; }
        public Int32 NrColuna { get; private set; }

        public Guid NrseqDocumentoConfiguracaoItem { get; private set; }

        public Int32 NrFonteTamanho { get; private set; }

        public string NoTipoFonte { get; private set; }

        public Guid? NrSeqDocumentoConfiguracao { get; private set; }

        public bool FlgIdentificador { get; private set; }

        public bool FlgImage { get; private set; }

        public int NrMaxImageWidth { get; private set; }

        public int NrMaxImageHeight { get; private set; }

        public AtualizarDocumentoConfiguracaoItemCommand(string nofonte, string nofontecolor, string nofrase, bool flgpropriedade, Int32 nrcoluna, Guid nrseqdocumentoconfiguracaoitem, Int32 nrfontetamanho,
            string notipofonte, Guid? nrseqdocumentoconfiguracao, bool flgidentificador, bool flgimage, Int32 nrmaximagewidth, Int32 nrmaximageheight)
        {
            NoFonte = nofonte;
            NoFonteColor = NoFonteColor;
            NoFrase = nofrase;
            FlgPropriedade = flgpropriedade;
            NrColuna = nrcoluna;
            NrseqDocumentoConfiguracaoItem = nrseqdocumentoconfiguracaoitem;
            NrFonteTamanho = nrfontetamanho;
            NoTipoFonte = notipofonte;
            NrSeqDocumentoConfiguracao = nrseqdocumentoconfiguracao;
            flgidentificador = FlgIdentificador;
            FlgImage = flgimage;
            NrMaxImageWidth = nrmaximagewidth;
            NrMaxImageHeight = nrmaximageheight;
        }
    }
}
