using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Funcionarios.Commands
{
    public class RegistrarFuncionarioCommand : Command
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public RegistrarFuncionarioCommand(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }
    }
}
