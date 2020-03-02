using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Domain.Clientes.Commands
{
    public class RegistrarClienteCommand : BaseClienteCommand
    {
        public RegistrarClienteCommand(string nome, string cpf, string rg, DateTime dataexpedicao, DateTime datanascimento,DateTime datacadastro,
           string orgaoexp, string fone, string celular, string email,string sexo,string estadocivil,string ocupacao,int codigoprofissao,string ppe, Guid funcionarioId, IncluirEnderecoClienteCommand endereco)
        {
            Nome = nome;
            CPF = cpf;
            DataExpedicao = dataexpedicao;
            DataNasc = datanascimento;
            DataCadastro = datacadastro;
            RG = rg;
            OrgaoExpedidor = orgaoexp;
            Fone = fone;
            Celular = celular;
            Email = email;
            Sexo = sexo;
            EstadoCivil = estadocivil;
            Ocupacao = ocupacao;
            CodigoProf = codigoprofissao;
            PPE = ppe;
            FuncionarioId = funcionarioId;
            Endereco = endereco;
        }

        public IncluirEnderecoClienteCommand Endereco { get; private set; }

    }
}
