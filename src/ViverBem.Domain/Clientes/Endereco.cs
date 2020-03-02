using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Clientes
{
    public class Endereco : Entity<Endereco>
    {
        public Endereco(Guid id, string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string estado, Guid? clienteId)
        {
            Id = id;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            CEP = cep;
            Cidade = cidade;
            Estado = estado;
            ClienteId = clienteId;
        }
        public override bool EhValido()
        {
            //RuleFor(c => c.Logradouro).NotEmpty().WithMessage("O Logradouro precisa ser fornecido");
            //RuleFor(c => c.Bairro).NotEmpty().WithMessage("O Bairro precisa ser fornecido");
            //RuleFor(c => c.CEP).NotEmpty().WithMessage("O CEP precisa ser fornecido");
            //RuleFor(c => c.Cidade).NotEmpty().WithMessage("A Cidade precisa ser fornecido");
            //RuleFor(c => c.Numero).NotEmpty().WithMessage("O Numero precisa ser fornecido");
            //RuleFor(c => c.Estado).NotEmpty().WithMessage("O Estado precisa ser fornecido");
            //RuleFor(c => c.Complemento).NotEmpty().WithMessage("O Complemento precisa ser fornecido.");
            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string CEP { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public Guid? ClienteId { get; private set; }

        //Construtor para o EF
        protected Endereco() { }
        //Propriedade de navegação do EF
        public virtual Cliente Cliente { get; private set; }
    }
}
