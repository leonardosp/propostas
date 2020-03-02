using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Funcionarios.Events
{
    public class FuncionarioRegistradoEvent : Event
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public FuncionarioRegistradoEvent(Guid id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }
    }
}
