using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViverBem.Application.ViewModels
{
    public class ClienteViewModel
    {
        public ClienteViewModel()
        {
            Id = Guid.NewGuid();
            Endereco = new EnderecoViewModel();
            Dependente = new DependenteViewModel();
        }
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é Requerido")]
        [MinLength(2, ErrorMessage = "O tamanho minimo do Nome é {1}")]
        [MaxLength(150, ErrorMessage = "O tamanho maximo do Nome é {1}")]
        [Display(Name = "Nome do Cliente")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="A ocupação é Requerida")]
        [Display(Name ="Ocupação do Cliente")]
        public string Ocupacao { get; set; }

        
        [Display(Name ="Código da Ocupação")]
        public int CodigoProf { get; set; }

        [Display(Name = "CPF do Cliente")]
        public string CPF { get; set; }

        [Display(Name = "RG do cliente")]
        public string RG { get; set; }

        [Display(Name = "Data Expedição")]
        [Required(ErrorMessage = "A data é requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime DataExpedicao { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "A data é requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataNasc { get; set; }

        [Display(Name = "Orgão Emissor")]
        [Required(ErrorMessage = "O orgão emissor é requerido")]
        public string OrgaoExpedidor { get; set; }

        [Display(Name = "Fone com DDD")]
        [Required(ErrorMessage = "O fone com DDD é requerido")]
        [DataType(DataType.PhoneNumber)]
        public string  Fone { get; set; }

        [Display(Name = " Indicador de Pessoa Politicamente Exposta ")]
        [Required(ErrorMessage = "O PPE é requerido")]
        public string PPE { get; set; }

        [Display(Name = "Celular com DDD")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "O celular com DDD é requerido")]
        public string Celular { get; set; }

        [Display(Name ="Sexo do Cliente")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage ="O Sexo do cliente é requerido")]
        public string Sexo { get; set; }

        [Display(Name ="Estado civíl do Cliente")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage ="O Estado civíl do cliente é requerido")]
        public string EstadoCivil { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O email é requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public EnderecoViewModel Endereco { get; set; }
        public DependenteViewModel Dependente { get; set; }
        public IEnumerable<DependenteViewModel> dependentes { get; set; }
        public Guid FuncionarioId { get; set; }

        public SelectList Sexos()
        {
            return new SelectList(ClienteSexoViewModel.ListarSexos(), "NomSexo", "Sexo");
        }
        public SelectList GrauParentesco()
        {
            return new SelectList(GrauParentescoViewModel.ListarGrausParentesco(), "GrauCod", "GrauDesc");
        }
        public SelectList Ocupacoes()
        {
            return new SelectList(ProfissaoViewModel.ListarProfissoes(), "profcod", "profdesc");
        }   
        public SelectList Civis()
        {
            return new SelectList(EstadoCivilViewModel.ListarEstadosCivis(), "Cod", "NomeEstadoCivil");
        }

    }
}
