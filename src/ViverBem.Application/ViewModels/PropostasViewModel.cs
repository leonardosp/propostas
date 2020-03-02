using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViverBem.Application.ViewModels
{
    public class PropostasViewModel
    {
        public PropostasViewModel()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O CPF é Requerido")]
        [MinLength(2, ErrorMessage = "O tamanho minimo do CPF é {1}")]
        [MaxLength(150, ErrorMessage = "O tamanho maximo do CPF é {1}")]
        [Display(Name = "CPF")]
        public String CPF { get;  set; }

        public bool Aprovado { get; set; }

        public bool Excluido { get; set; }

        [Display(Name = "Campo livre para informar o código da proposta no cliente")]
        public string Id_ext_props { get;  set; }

        [Display(Name = "Tipo de pagamento da proposta")]
        public string Tp_pgto { get;  set; }

        [Display(Name = "Data da Venda")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Dt_venda { get;  set; }

        [Display(Name = "Nome do Cliente")]
        public string Nome { get;  set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Dt_nasc { get;  set; }


        [Display(Name = "Sexo")]
        public string Sexo { get;  set; }

        [Display(Name = "Estado Civil")]
        public string Est_civil { get;  set; }

        [Display(Name = "Endereço")]
        public string Ender { get;  set; }

        [Display(Name = "Endereço Numero")]
        public string Endnum { get;  set; }

        [Display(Name = "Complemento")]
        public string EndComplemento { get;  set; }

        [Display(Name = "Bairro")]
        public string Bairro { get;  set; }

        [Display(Name = "Cidade")]
        public string Cidade { get;  set; }

        [Display(Name = "UF")]
        public string UF { get;  set; }

        [Display(Name = "CEP")]
        public string CEP { get;  set; }

        [Display(Name = "DDDTEL")]
        public string DDDTel { get;  set; }

        [Display(Name = "Telefone")]
        public string Tel { get;  set; }

        [Display(Name = "DDDCEL")]
        public string DDDCel { get;  set; }

        [Display(Name = "Cel")]
        public string Cel { get;  set; }

        [Display(Name = "Identidade")]
        public string Identidade { get;  set; }

        [Display(Name = "Orgão emissor")]
        public string Org_emissor { get;  set; }

        [Display(Name = "Email")]
        public string Email { get;  set; }

        [Display(Name = "Código Profissão")]
        public string Cod_prof { get;  set; }

        [Display(Name = "Ocupação")]
        public string Ocupacao { get;  set; }

        [Display(Name = "Código do Combo")]
        public string CodCombo { get;  set; }

        [Display(Name = "Valor Prêmio")]
        public string VlrPremio { get;  set; }

        [Display(Name = "Valor Capital")]
        public string VlrCapital { get;  set; }


        [Display(Name = "Código do Banco Bacen")]
        public string Banco { get;  set; }
        [Display(Name = "Código da Agencia para debito")]
        public string Agencia { get;  set; }

        [Display(Name = "Digito verificador da agência, quando aplicavel")]
        public string Agencia_dv { get;  set; }

        [Display(Name = "conta corrente para debito em conta")]
        public string CC { get;  set; }

        [Display(Name = "Digito verificador da conta ")]
        public string Cc_dv { get;  set; }

        [Display(Name = "dia para debito em conta")]
        public string Dia_debito { get;  set; }

        [Display(Name = "tipo de envio")]
        public String Tipo_envio { get;  set; }

        [Display(Name = "Endereço cobrança do associado")]
        public string Endercobr { get;  set; }

        [Display(Name = "Numero de cobrança")]
        public string Endnumcobr { get;  set; }

        [Display(Name = "Complemento do associado")]
        public string Endcomplcobr { get;  set; }

        [Display(Name = "Bairro do associado")]
        public string Bairrocobr { get;  set; }

        [Display(Name = "Cidade cobrança do associado")]
        public string Cidadecobr { get;  set; }

        [Display(Name = "UF de cobrança do associado")]
        public string Ufcobr { get;  set; }

        [Display(Name = "Cep de cobrança")]
        public string Cepcobr { get;  set; }

        public string Pronum { get;  set; }

        public Guid IdDoClienteX { get; set; }
        public string AssocMatric { get;  set; }
        public string ProdTPag { get;  set; }
        public string Corretor { get;  set; }
        public string PPE { get;  set; }

        [Display(Name = "Motivo Rejeição")]
        public string StatusConsulta { get; set; }

        [Display(Name = "Id Proposta")]
        public string StatusFinanceiro { get; set; }

        public Guid FuncionarioId { get;  set; }
        public IEnumerable<DependenteProposta> dependentes { get; set; }
        //Propriedades de naveção EF
        public virtual ClienteViewModel Cliente { get;  set; }
        public virtual DependenteViewModel Dependente { get;  set; }
        public virtual ComboViewModel Combo { get;  set; }

        public SelectList Contas()
        {
            return new SelectList(DebitoEmContaDisponivel.ListarBancos(), "bancod", "bannome");
        }
    }
}
