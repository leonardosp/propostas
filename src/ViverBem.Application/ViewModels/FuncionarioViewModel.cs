using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class FuncionarioViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é requerido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O E-mail é requerido")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        public string Email { get; set; }
    }
}
