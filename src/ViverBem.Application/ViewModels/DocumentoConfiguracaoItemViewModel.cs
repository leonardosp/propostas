using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class DocumentoConfiguracaoItemViewModel
    {
        public DocumentoConfiguracaoItemViewModel()
        {
            NrseqDocumentoConfiguracaoItem = Guid.NewGuid();
        }
        public string NoFonte { get;  set; }
        public string NoFonteColor { get;  set; }

        public string NoFrase { get;  set; }
        public bool FlgPropriedade { get;  set; }
        public Int32 NrColuna { get;  set; }

        public Guid NrseqDocumentoConfiguracaoItem { get;  set; }

        public Int32 NrFonteTamanho { get;  set; }

        public string NoTipoFonte { get;  set; }

        public Guid? NrSeqDocumentoConfiguracao { get;  set; }

        public bool FlgIdentificador { get;  set; }

        public bool FlgImage { get;  set; }

        public int NrMaxImageWidth { get;  set; }

        public int NrMaxImageHeight { get;  set; }

        public SelectList EstiloFonte()
        {
            return new SelectList(EstiloFonteViewModel.ListarFontes(), "TipoFonte", "NomeFonte");
        }
        public SelectList Fonte()
        {
            return new SelectList(FonteViewModel.ListarFontes(), "TipoFonte", "NomeFonte");
        }
    }
}
