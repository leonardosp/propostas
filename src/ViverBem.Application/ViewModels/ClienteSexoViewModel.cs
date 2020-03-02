using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class ClienteSexoViewModel
    {
        public string NomSexo { get; set; }
        public string Sexo { get; set; }

        public static List<ClienteSexoViewModel> ListarSexos()
        {
            return new List<ClienteSexoViewModel>()
            {
                new ClienteSexoViewModel() {NomSexo="M", Sexo="Masculino"},
                new ClienteSexoViewModel() {NomSexo="F", Sexo="Feminino"}
            };
        }
    }
}
