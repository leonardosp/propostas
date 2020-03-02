using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Clientes.Events
{
    public class DependenteClienteAtualizadoEvent : Event
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Parentesco { get; private set; }
        public decimal Participacao { get; private set; }

        public DependenteClienteAtualizadoEvent(Guid DependenteId, string nome, DateTime datanascimento, string parentesco, decimal participacao, Guid clienteId)
        {
            Id = DependenteId;
            Nome = nome;
            DataNascimento = datanascimento;
            Parentesco = parentesco;
            Participacao = participacao;
            AggregateId = clienteId;
        }
    }
}
