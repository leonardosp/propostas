using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Propostas.Events
{
    public class PropostasAtualizadoEventHandler : BasePropostasEvent
    {
        public PropostasAtualizadoEventHandler(DateTime dataVenda, string id_ext_props, string tp_pgto, string cpf, string nome,
DateTime dt_nasc, string sexo, string est_civil, string ender, string endnum, string endcomplemento, string bairro, string cidade, string uf, string cep, string dddtel, string tel, string dddcel, string cel,
         string identidade, string org_emissor, string email, string cod_prof, string ocupacao, DateTime dt_venda,DateTime datacadastro, string idextprops, string codcombo, string vlrpremio, string vlrcapital, string tppgto, string banco, string agencia, string agenciadv, string cc,
         string cc_dv, string diadebito, string tipoenvio, string endercobr, string endnumcobr, string endcomplcobr, string bairrocobr, string cidadecobr, string ufcobr, string cepcobr, string pronum, string assmatrc, string prodtpag,string statusconsulta,string statusfinanceiro, string ppe,bool aprovado,bool excluido,Guid iddoclientex)
        {
            DataVenda = dataVenda;
            PPE = ppe;
            Id_ext_props = id_ext_props;
            Tp_pgto = tppgto;
            CPF = cpf;
            Nome = nome;
            Dt_nasc = dt_nasc;
            Sexo = sexo;
            Est_civil = est_civil;
            Ender = ender;
            Endnum = endnum;
            EndComplemento = endcomplemento;
            Bairro = bairro;
            Cidade = cidade;
            UF = uf;
            CEP = cep;
            DDDTel = dddtel;
            Tel = tel;
            DDDCel = dddcel;
            Cel = cel;
            Identidade = identidade;
            Org_emissor = org_emissor;
            Email = email;
            Cod_prof = cod_prof;
            Ocupacao = ocupacao;
            Dt_venda = dt_venda;
            Idextprops = idextprops;
            CodCombo = codcombo;
            VlrPremio = vlrpremio;
            VlrCapital = vlrcapital;
            TpPgto = tppgto;
            Banco = banco;
            Agencia = agencia;
            Agencia_dv = agenciadv;
            CC = cc;
            Cc_dv = cc_dv;
            Dia_debito = diadebito;
            DataCadastro = datacadastro;
            Tipo_envio = tipoenvio;
            Endercobr = endercobr;
            Endnumcobr = endnumcobr;
            Endcomplcobr = endcomplcobr;
            Bairrocobr = bairrocobr;
            Cidadecobr = cidadecobr;
            Ufcobr = ufcobr;
            Cepcobr = cepcobr;
            Pronum = pronum;
            AssMatrc = assmatrc;
            Prodtpag = prodtpag;
            StatusConsulta = statusconsulta;
            StatusFinanceiro = statusfinanceiro;
            Aprovado = aprovado;
            Excluido = excluido;
            IdDoClienteX = iddoclientex;
        }
    }
}
