using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class TokenResultViewModel
    {
        public TokenResultViewModel()
        {
            Id = Guid.NewGuid();

        }

        [Key]
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string RETORNOTOTAL { get; set; }
        public DateTime LimiteDoToken { get; set; }
    }
}
