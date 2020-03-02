using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Clientes.Events
{
    public class DependenteClienteAdicionadoEvent : Event
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Parentesco { get; private set; }
        public decimal Participacao { get; private set; }

        public DependenteClienteAdicionadoEvent(Guid DependenteId, string nome,string parentesco, decimal participacao, Guid clienteId)
        {
            Id = DependenteId;
            Nome = nome;
            Parentesco = parentesco;
            Participacao = participacao;
            AggregateId = clienteId;
        }
    }
}
