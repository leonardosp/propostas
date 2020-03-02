using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class CorFundoViewModel
    {
        public string TipoAlinhamento { get; set; }
        public string NomeAlinhamento { get; set; }

        public static List<CorFundoViewModel> ListarAlinhamentos()
        {
            return new List<CorFundoViewModel>()
            {
                new CorFundoViewModel() {TipoAlinhamento="LIGHT_GRAY", NomeAlinhamento="LIGHT_GRAY"}
            };
        }
    }
}
