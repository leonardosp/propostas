using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class ClaimsViewModel
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public static List<ClaimsViewModel> ListarClaims()
        {
            return new List<ClaimsViewModel>()
            {
                new ClaimsViewModel() {ClaimType="Administrador", ClaimValue="Administrador"},
                new ClaimsViewModel() {ClaimType="Aprovador", ClaimValue="Aprovador"},
                new ClaimsViewModel() {ClaimType="Digitador", ClaimValue="Digitador"}
            };
        }
    }
}
