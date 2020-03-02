using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class ComboPrincViewModel
    {
        public ComboPrincViewModel()
        {
            Id = Guid.NewGuid();
            Combos = new ComboViewModel();
        }

        public ComboViewModel Combos { get; set; }
        public IEnumerable<ComboViewModel> CombosItems { get; set; }
        [Display(Name = "Código do Corretor")]
        public String Corretor { get;  set; }

        [Display(Name = "Código do Combo ")]
        public string CodCombo { get;  set; }

        [Key]
        public Guid Id { get; set; }

        public Guid comboPrincId { get;  set; }

      
    }
}
