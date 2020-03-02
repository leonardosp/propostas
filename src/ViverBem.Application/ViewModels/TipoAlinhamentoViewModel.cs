using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class TipoAlinhamentoViewModel
    {
        public string TipoAlinhamento { get; set; }
        public string NomeAlinhamento { get; set; }

        public static List<TipoAlinhamentoViewModel> ListarAlinhamentos()
        {
            return new List<TipoAlinhamentoViewModel>()
            {
                new TipoAlinhamentoViewModel() {TipoAlinhamento="ALIGN_LEFT", NomeAlinhamento="Texto alinhado a esquerda da pagina"},
                new TipoAlinhamentoViewModel() {TipoAlinhamento="ALIGN_CENTER", NomeAlinhamento="Texto alinhado no centro da pagina"},
                new TipoAlinhamentoViewModel() {TipoAlinhamento="ALIGN_RIGHT", NomeAlinhamento="Texto alinhado a direita da pagina"},
                new TipoAlinhamentoViewModel() {TipoAlinhamento="ALIGN_JUSTIFIED", NomeAlinhamento="Alinhamento justificado"},
            };
        }
    }
}
