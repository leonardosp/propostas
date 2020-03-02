using FluentValidation;
using System;
using ViveBem.Domain.Core.Models;
using ViverBem.Domain.Funcionarios;
using System.Collections.Generic;

namespace ViverBem.Domain.Clientes
{
    public class Cliente : Entity<Cliente>
    {
        public Cliente(string nome,string cpf,string rg, DateTime dataexpedicao, DateTime datanascimento, DateTime dataCadastro,
           string orgaoexp, string fone, string celular, string email,string sexo, string estadocivil,string codigoocupacao,string ocupacao)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            DataExpedicao = dataexpedicao;
            DataNasc = datanascimento;
            DataCadastro = dataCadastro;
            OrgaoExpedidor = orgaoexp;
            Fone = fone;
            Celular = celular;
            Email = email;
            Sexo = sexo;
            EstadoCivil = estadocivil;
            CodigoProf = Convert.ToInt32(codigoocupacao);
            Ocupacao = ocupacao;
        }

        private Cliente()
        {

        }
        public string Nome { get; private set; }

        public string CPF { get; private set; }

        public String Sexo { get; private set; }

        public int CodigoProf { get; private set; }

        public string Ocupacao { get; private set; }

        public string EstadoCivil { get; private set; }

        public string RG { get; private set; }

        public DateTime DataExpedicao { get; private set; }

        public DateTime DataNasc { get; private set; }

        public DateTime DataCadastro { get; private set; }

        public string OrgaoExpedidor { get; private set; }

        public string Fone { get; private set; }

        public string Celular { get; private set; }

        public string Email { get; private set; }
        public bool Excluido { get; private set; }
        public string PPE { get; private set; }
        public Guid? EnderecoId { get; private set; }
        public Guid? FuncionarioId { get; private set; }

        //Propriedades de naveção EF
        public virtual Endereco Endereco { get; private set; }
        public virtual Funcionario Funcionario { get; private set; }
        public virtual Dependente Dependente { get; private set; }
        public ICollection<Dependente> Dependentes { get; private set; }
        //AdRock Setters
        public void AtribuirEndereco(Endereco endereco)
        {
            if (!endereco.EhValido()) return;
            Endereco = endereco;
        }
        public void AtribuirDependente(Dependente dependete)
        {
            if (!dependete.EhValido()) return;
            Dependente = dependete;
        }

        public void ExcluirCliente()
        {
            Excluido = true;
        }
        public override bool EhValido()
        {
            Validar();
            return ValidationResult.IsValid;
        }

        #region Validações

        private void Validar()
        {
            ValidarNome();
            ValidarCpf();
            ValidationResult = Validate(this);

            ValidarEndereco();
            //Validacoes Adicionais
        }

        private void ValidarNome()
        {
            RuleFor(c => c.Nome)
            .NotEmpty().WithMessage("O nome do cliente precisa ser fornecido")
            .Length(2, 150).WithMessage("O nome do cliente precisa ter entre 2 e 150 caracteres");
        }
  
        private void ValidarCpf()
        {
            //RuleFor(c => c.CPF).NotEmpty().WithMessage("O CPF do cliente precisa ser fornecido").Length(2,11).WithMessage("O Cpf precisa ter 11 caracteres");
        }

        private void ValidarEndereco()
        {
            if (Endereco.EhValido()) return;

            foreach (var error in Endereco.ValidationResult.Errors)
            {
                ValidationResult.Errors.Add(error);
            }
        }

        #endregion


        public static class ClienteFactory
        {
            public static Cliente NovoClienteCompleto(Guid id, string nome, string cpf, string rg, DateTime dataexpedicao, DateTime datanascimento, DateTime datacadastro,
           string orgaoexp, string fone, string celular, string email,string estadocivil,string sexo,string ocupacao, int codigoprofissao,string ppe, Guid? funcionariId, Endereco endereco)
            {
                var cliente = new Cliente()
                {
                    Id = id,
                    Nome = nome,
                    DataNasc = datanascimento,
                    DataExpedicao = dataexpedicao,
                    DataCadastro = datacadastro,
                    CPF = cpf,
                    Email = email,
                    RG = rg,
                    EstadoCivil = estadocivil,
                    Sexo = sexo,
                    OrgaoExpedidor = orgaoexp,
                    Fone = fone,
                    Celular = celular,
                    Ocupacao = ocupacao,
                    CodigoProf = codigoprofissao,
                    PPE = ppe,
                    Endereco = endereco
                };

                if (funcionariId.HasValue)
                    cliente.FuncionarioId = funcionariId.Value;

                return cliente;
            }
        }
    }
}
