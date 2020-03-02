using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class FonteViewModel
    {
        public string TipoFonte { get; set; }
        public string NomeFonte { get; set; }

        public static List<FonteViewModel> ListarFontes()
        {
            return new List<FonteViewModel>()
            {
                new FonteViewModel() {TipoFonte="Helvetica", NomeFonte="HELVETICA"},
                new FonteViewModel() {TipoFonte="Courier", NomeFonte="COURIER"},
                new FonteViewModel() {TipoFonte="Times Roman", NomeFonte="TIMES_ROMAN"},
                new FonteViewModel() {TipoFonte="Symbol", NomeFonte="SYMBOL"},
                new FonteViewModel() {TipoFonte="Zapfdingbats", NomeFonte="ZAPFDINGBATS"}
            };
        }
    }
}
