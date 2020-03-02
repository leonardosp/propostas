﻿using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Clientes.Commands
{
    public class IncluirDependenteClienteCommand : Command
    {
        public IncluirDependenteClienteCommand(Guid DependenteId, string nome, string parentesco, decimal participacao, Guid clienteId)
        {
            Id = DependenteId;
            Nome = nome;
            Parentesco = parentesco;
            Participacao = participacao;
            ClienteId = clienteId;
        }
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Parentesco { get; private set; }
        public decimal Participacao { get; private set; }
        public Guid? ClienteId { get; private set; }
    }
}
