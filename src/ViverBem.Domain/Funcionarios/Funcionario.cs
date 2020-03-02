using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Models;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Propostas;

namespace ViverBem.Domain.Funcionarios
{
    public class Funcionario : Entity<Funcionario>
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public ICollection<Cliente> Clientes { get; private set; }
        public ICollection<Propostas.Propostas> Propostas { get; private set; }
        public Funcionario(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        protected Funcionario() { }
        public Funcionario(Guid id)
        {
            Id = id;
        }
        public override bool EhValido()
        {
            return true;
        }
    }
}
