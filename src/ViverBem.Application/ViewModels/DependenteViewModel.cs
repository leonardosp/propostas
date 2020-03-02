using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class DependenteViewModel
    {
        public DependenteViewModel()
        {
            Id = Guid.NewGuid();
        }
            
        [Key]
        public Guid Id { get; set; }

        [MinLength(2, ErrorMessage = "O tamanho minimo do Nome é {1}")]
        [MaxLength(150, ErrorMessage = "O tamanho maximo do Nome é {1}")]
        [Display(Name = "Nome do Dependente")]
        public string Nome { get;  set; }

        public string Parentesco { get;  set; }

        [Display(Name = "Participação")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DataType(DataType.Currency, ErrorMessage = "Moeda em formato inválido")]
        public decimal Participacao { get;  set; }


        public string Id_ext_props { get; set; }
        public Guid ClienteId { get; set; }

    }
}
