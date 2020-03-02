using System;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Propostas.Commands
{
    public class BasePropostasCommand : Command
    {
        public Guid Id { get; protected set; }
        public DateTime DataVenda { get; protected set; }
        public string Corretor { get; protected set; }
        public string Id_ext_props { get; protected set; }
        public string Tp_pgto { get; protected set; }

        public Guid IdDoClienteX { get; protected set; }
        public string PPE { get; protected set; }
        public string CPF { get; protected set; }
        public string Nome { get; protected set; }
        public DateTime Dt_nasc { get; protected set; }
        public string Sexo { get; protected set; }
        public string Est_civil { get; protected set; }
        public Guid FuncionarioId { get; protected set; }
        public string Ender { get; protected set; }
        public string Endnum { get; protected set; }
        public string EndComplemento { get; protected set; }
        public string Bairro { get; protected set; }
        public string Cidade { get; protected set; }
        public string UF { get; protected set; }
        public string CEP { get; protected set; }
        public string DDDTel { get; protected set; }
        public string Tel { get; protected set; }
        public string DDDCel { get; protected set; }
        public string Cel { get; protected set; }
        public string Identidade { get; protected set; }
        public string Pronum { get; protected set; }
        public string StatusConsulta { get; protected set; }
        public string StatusFinanceiro { get; protected set; }
        public string AssMatrc { get; protected set; }
        public string Prodtpag { get; protected set; }
        public string Org_emissor { get; protected set; }
        public string Email { get; protected set; }
        public string Cod_prof { get; protected set; }
        public string Ocupacao { get; protected set; }
        public DateTime Dt_venda { get; protected set; }
        public DateTime DataCadastro { get; protected set; }
        public string CodCombo { get; protected set; }
        public string VlrPremio { get; protected set; }
        public string VlrCapital { get; protected set; }
        public string Banco { get; protected set; }
        public string Agencia { get; protected set; }
        public string Agencia_dv { get; protected set; }
        public string CC { get; protected set; }
        public string Cc_dv { get; protected set; }
        public string Dia_debito { get; protected set; }
        public string Tipo_envio { get; protected set; }
        public string Endercobr { get; protected set; }
        public string Endnumcobr { get; protected set; }
        public string Endcomplcobr { get; protected set; }
        public string Bairrocobr { get; protected set; }
        public string Cidadecobr { get; protected set; }
        public string Ufcobr { get; protected set; }
        public string Cepcobr { get; protected set; }
        public bool Aprovado { get; protected set; }
        public bool Excluido { get; protected set; }
    }
}
