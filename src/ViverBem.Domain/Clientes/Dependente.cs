using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Clientes
{
    public class Dependente : Entity<Dependente>
    {
        public Dependente(Guid id,string nome, string parentesco,decimal participacao, Guid? clienteId)
        {
            Id = id;
            Nome = nome;
            Parentesco = parentesco;
            Participacao = participacao;
            ClienteId = clienteId;
        }

        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Parentesco { get; private set; }
        public decimal Participacao { get; private set; }
        public Guid? ClienteId { get; private set; }

        //Construtor para o EF
        protected Dependente() { }
        //Propriedade de navegação do EF
    
        public virtual Cliente Cliente { get; private set; }

        public override bool EhValido()
        {
            RuleFor(c => c.Nome).NotEmpty().WithMessage("O nome precisa ser fornecido");
            RuleFor(c => c.Parentesco).NotEmpty().WithMessage("O Parentesco precisa ser fornecido");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
