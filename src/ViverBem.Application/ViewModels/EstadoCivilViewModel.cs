using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class EstadoCivilViewModel
    {
        public string Cod { get; set; }
        public string NomeEstadoCivil { get; set; }

        public static List<EstadoCivilViewModel> ListarEstadosCivis()
        {
            return new List<EstadoCivilViewModel>()
            {
                new EstadoCivilViewModel() {Cod="S", NomeEstadoCivil="Solteiro"},
                new EstadoCivilViewModel() {Cod="C", NomeEstadoCivil="Casado"},
                new EstadoCivilViewModel() {Cod="V", NomeEstadoCivil="Viuvo"},
                new EstadoCivilViewModel() {Cod="D", NomeEstadoCivil="Divorciado"},
                new EstadoCivilViewModel() {Cod="A", NomeEstadoCivil="Concubinato"},
                new EstadoCivilViewModel() {Cod="U", NomeEstadoCivil="União Estavél"}
            };
        }
    }
}
