using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Clientes.Events
{
    public class ClienteRegistradoEvent : BaseClienteEvent
    {
        public ClienteRegistradoEvent(Guid id, string nome, string cpf, string rg, DateTime dataexpedicao, DateTime datanascimento, DateTime datacadastro,
          string orgaoexp, string fone, string celular,string sexo,string estadocivil, string email,string ocupacao,string ppe,int codigoprofissao)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            EstadoCivil = estadocivil;
            Sexo = sexo;
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
