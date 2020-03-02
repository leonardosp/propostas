using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Clientes.Events
{
    public class EnderecoClienteAdicionadoEvent : Event
    {
        public Guid Id { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string CEP { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }

        public EnderecoClienteAdicionadoEvent(Guid EnderecoId, string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string estado, Guid clienteId)
        {
            Id = EnderecoId;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            CEP = cep;
            Cidade = cidade;
            Estado = estado;
            AggregateId = clienteId;
        }
    }
}
