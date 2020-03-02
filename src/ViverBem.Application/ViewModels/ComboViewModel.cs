using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class ComboViewModel
    {
        public ComboViewModel()
        {
            Id = Guid.NewGuid();
            ComboPrincId = Id;
        }
        [Key]
        public Guid Id { get; set; }

        public Guid? ComboPrincId { get; set; }

        [MinLength(2, ErrorMessage = "O tamanho minimo do Nome é {1}")]
        [MaxLength(150, ErrorMessage = "O tamanho maximo do Nome é {1}")]
        [Display(Name = "Código do Combo")]
        public string CodCombo { get;  set; }

        [Display(Name = "Plano Srv")]
        public string PlanoSrv { get;  set; }

        [Display(Name = "Plano Prin Serv")]
        public String PlaPrinServ { get;  set; }

        [Display(Name = "Valor Prêmio")]
        public string VlrPremio { get;  set; }

        [Display(Name = "Valor Capital")]
        public string VlrCapital { get;  set; }

        [Display(Name = "Código Comissão Usr")]
        public string CodComissUsr { get;  set; }
    }
}
