using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Clientes.Events
{
    public class ClienteAtualizadoEvent : BaseClienteEvent
    {
        public ClienteAtualizadoEvent(Guid id, string nome, string cpf, string rg, DateTime dataexpedicao, DateTime datanascimento,DateTime datacadastro,
           string orgaoexp, string fone, string celular, string email,string sexo,string estadocivil,int codigoprofissao,string ocupacao,string ppe)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            Sexo = sexo;
            EstadoCivil = estadocivil;
            RG = rg;
            DataExpedicao = dataexpedicao;
            DataNasc = datanascimento;
            DataCadastro = datacadastro;
            OrgaoExpedidor = orgaoexp;
            Fone = fone;
            Celular = celular;
            Email = email;
            Ocupacao = ocupacao;
            CodigoProf = codigoprofissao;
            PPE = ppe;
            AggregateId = Id;
        }

    }
}
