using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ViverBem.Application.ViewModels;

namespace ViverBem.Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {


        public string Hierarquia { get; set; }

        [MinLength(11)]
        public string CPF { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }


        public SelectList Claims()
        {
            return new SelectList(ClaimsViewModel.ListarClaims(), "ClaimType", "ClaimValue");
        }
    }
}
