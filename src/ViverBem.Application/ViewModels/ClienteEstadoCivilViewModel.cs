using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class ClienteEstadoCivilViewModel
    {
        public string NomeEstadoCivil { get; set; }
        public string EstadoCivil { get; set; }

        public static List<ClienteEstadoCivilViewModel> ListarSexos()
        {
            return new List<ClienteEstadoCivilViewModel>()
            {
                new ClienteEstadoCivilViewModel() {NomeEstadoCivil="SOLTEIRO", EstadoCivil="Solteiro(a)"},
                new ClienteEstadoCivilViewModel() {NomeEstadoCivil="CASADO", EstadoCivil="Casado(a)"},
                new ClienteEstadoCivilViewModel() {NomeEstadoCivil="VIUVO", EstadoCivil="Viuvo(a)"},
                new ClienteEstadoCivilViewModel() {NomeEstadoCivil="SEPARADO", EstadoCivil="Separado(a)"},
                new ClienteEstadoCivilViewModel() {NomeEstadoCivil="OUTRO", EstadoCivil="Outro(a)"}
            };
        }
    }
}
