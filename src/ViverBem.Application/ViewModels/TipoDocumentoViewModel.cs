using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class TipoDocumentoViewModel
    {
        public string TipoDoc { get; set; }
        public string NomeDoc { get; set; }

        public static List<TipoDocumentoViewModel> ListarDocumentos()
        {
            return new List<TipoDocumentoViewModel>()
            {
                new TipoDocumentoViewModel() {TipoDoc="PROPOSTA", NomeDoc="Proposta"},
            };
        }
    }
}
