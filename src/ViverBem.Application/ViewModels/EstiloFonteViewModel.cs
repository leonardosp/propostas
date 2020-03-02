using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class EstiloFonteViewModel
    {
        public string TipoFonte { get; set; }
        public string NomeFonte { get; set; }

        public static List<EstiloFonteViewModel> ListarFontes()
        {
            return new List<EstiloFonteViewModel>()
            {
                new EstiloFonteViewModel() {TipoFonte="NORMAL", NomeFonte="Normal"},
                new EstiloFonteViewModel() {TipoFonte="BOLD", NomeFonte="Bold"},
                new EstiloFonteViewModel() {TipoFonte="ITALIC", NomeFonte="Italic"},
                new EstiloFonteViewModel() {TipoFonte="UNDERLINE", NomeFonte="Underline"},
                new EstiloFonteViewModel() {TipoFonte="STRIKETHRU", NomeFonte="Strikethru"},
                new EstiloFonteViewModel() {TipoFonte="BOLDITALIC", NomeFonte="Bolditalic"},
                new EstiloFonteViewModel() {TipoFonte="UNDEFINED", NomeFonte="Undefined"},
                new EstiloFonteViewModel() {TipoFonte="DEFAULTSIZE", NomeFonte="Defaultsize"},
            };
        }
    }
}
