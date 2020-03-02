using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Combos;
using ViverBem.Domain.Funcionarios;

namespace ViverBem.Domain.Propostas
{
    public class Propostas : Entity<Propostas>
    {
        public DateTime DataVenda { get; private set; }

        public string Id_ext_props { get; private set; }
        private Propostas()
        {

        }
        public string Corretor { get; private set; }
        public string Tp_pgto { get; private set; }
        //Propriedades de naveção EF
        public Guid? FuncionarioId { get; private set; }
        public virtual Funcionario Funcionario { get; private set; }
        public virtual Cliente Cliente { get; private set; }
        public virtual Combo Combo { get; private set; }
        public string CPF { get; private set; }
        public string Nome { get; private set; }
        public DateTime Dt_nasc { get; private set; }
        public string Sexo { get; private set; }
        public string Est_civil { get; private set; }
        public string Ender { get; private set; }
        public string Endnum { get; private set; }
        public string EndComplemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }

        public Guid IdDoClienteX { get; private set; }

        public string UF { get; private set; }
        public string CEP { get; private set; }
        public string DDDTel { get; private set; }
        public string Tel { get; private set; }
        public string DDDCel { get; private set; }
        public string Cel { get; private set; }
        public string Identidade { get; private set; }
        public string Org_emissor { get; private set; }
        public string Email { get; private set; }
        public string Cod_prof { get; private set; }
        public string Ocupacao { get; private set; }
        public DateTime Dt_venda { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Idextprops { get; private set; }
        public string CodCombo { get; private set; }
        public string VlrPremio { get; private set; }
        public string VlrCapital { get; private set; }
        public string Banco { get; private set; }
        public string Agencia { get; private set; }
        public string Agencia_dv { get; private set; }
        public string CC { get; private set; }
        public string Cc_dv { get; private set; }
        public string Dia_debito { get; private set; }
        public String Tipo_envio { get; private set; }
        public string Endercobr { get; private set; }
        public string Endnumcobr { get; private set; }
        public string Endcomplcobr { get; private set; }
        public string Bairrocobr { get; private set; }
        public string Cidadecobr { get; private set; }
        public string Ufcobr { get; private set; }
        public string Cepcobr { get; private set; }
        public bool Aprovado { get; private set; }
        public bool Excluido { get; private set; }
        public string PPE { get; private set; }
        public string Pronum { get; private set; }
        public string Assmatrc { get; private set; }
        public string Prodtpag { get; private set; }

        public string StatusConsulta { get; set; }

        public string StatusFinanceiro { get; set; }

        public override bool EhValido()
        {
            return true;
        }

        public void ExcluirCliente()
        {
            Excluido = true;
        }


        public static class PropostasFactory
        {
            public static Propostas NovaPropostaCompleto(Guid id, string corretor, string cpf, string nome, DateTime dt_nasc, string sexo, string est_civil, string ender, string endnum, string endcompl, string bairro, string cidade,
        string uf, string cep, string dddtel, string tel, string dddcel, string cel, string identidade, string org_emissor, string email, string cod_prof, string ocupacao, DateTime dt_venda,DateTime datacadastro, string id_ext_props, string cod_combo,
        string vl_premio, string vl_capital, string tp_pgto, string banco, string agencia, string agencia_dv, string cc, string cc_dv, string dia_debito, string tipo_envio, string endercobr, string endnumcobr, string endcomplcobr,
        string bairrocobr, string cidadecobr, string ufcobr, string cepcob, string pronum, string assmatrc, string prodtpag, string statusconsulta, string statusfinanceiro, string ppe, bool aprovado,bool excluido, Guid? funcionariId,Guid iddoclientex)
            {
                var proposta = new Propostas()
                {
                    Id = id,
                    Corretor = corretor,
                    CPF = cpf,
                    Nome = nome,
                    Dt_nasc = dt_nasc,
                    Sexo = sexo,
                    Est_civil = est_civil,
                    Ender = ender,
                    Endnum = endnum,
                    EndComplemento = endcompl,
                    Bairro = bairro,
                    Cidade = cidade,
                    UF = uf,
                    CEP = cep,
                    DDDTel = dddtel,
                    Tel = tel,
                    DDDCel = dddcel,
                    Cel = cel,
                    Identidade = identidade,
                    Org_emissor = org_emissor,
                    Email = email,
                    Cod_prof = cod_prof,
                    Ocupacao = ocupacao,
                    DataCadastro = datacadastro,
                    Dt_venda = dt_venda,
                    DataVenda = dt_venda,
                    Id_ext_props = id_ext_props,
                    CodCombo = cod_combo,
                    VlrPremio = vl_premio,
                    VlrCapital = vl_capital,
                    Tp_pgto = tp_pgto,
                    Banco = banco,
                    Agencia = agencia,
                    Agencia_dv = agencia_dv,
                    CC = cc,
                    Cc_dv = cc_dv,
                    Dia_debito = dia_debito,
                    Tipo_envio = tipo_envio,
                    Excluido = excluido,
                    Endercobr = endercobr,
                    Endnumcobr = endnumcobr,
                    Endcomplcobr = endcomplcobr,
                    Bairrocobr = bairrocobr,
                    Cidadecobr = cidadecobr,
                    Ufcobr = ufcobr,
                    Cepcobr = cepcob,
                    Pronum = pronum,
                    Assmatrc = assmatrc,
                    Prodtpag = prodtpag,
                    StatusConsulta = statusconsulta,
                    StatusFinanceiro = statusfinanceiro,
                    PPE = ppe,
                    Aprovado = aprovado,
                    FuncionarioId = funcionariId,
                    IdDoClienteX = iddoclientex
                };

                return proposta;
            }
        }

        }
    }
