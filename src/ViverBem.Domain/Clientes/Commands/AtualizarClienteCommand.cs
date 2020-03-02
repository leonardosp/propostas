using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Clientes.Commands
{
    public class AtualizarClienteCommand : BaseClienteCommand
    {
        public AtualizarClienteCommand(Guid id, string nome, string cpf, string rg, DateTime dataexpedicao, DateTime datanascimento, DateTime datacadastro,
           string orgaoexp, string fone, string celular, string email,string sexo,string estadocivil,string ocupacao,int codigoprofissao,string ppe,Guid funcionarioId)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            RG = rg;
            DataExpedicao = dataexpedicao;
            DataNasc = datanascimento;
            DataCadastro = datacadastro;
            OrgaoExpedidor = orgaoexp;
            Fone = fone;
            EstadoCivil = estadocivil;
            Sexo = sexo;
            Celular = celular;
            Email = email;
            CodigoProf = codigoprofissao;
            Ocupacao = ocupacao;
            PPE = ppe;
            FuncionarioId = funcionarioId;
        }
    }
}
