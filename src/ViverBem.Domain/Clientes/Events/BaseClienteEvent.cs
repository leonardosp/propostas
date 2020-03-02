using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Clientes.Events
{
    public abstract class BaseClienteEvent : Event
    {
        public Guid Id { get; set; }
        public string Nome { get; protected set; }

        public string CPF { get; protected set; }

        public string RG { get; protected set; }

        public string PPE { get;protected set; }
        public string Sexo { get; protected set; }
        public int CodigoProf { get; protected set; }
        public string Ocupacao { get; protected set; }
        public string EstadoCivil { get; protected set; }
        public DateTime DataExpedicao { get; protected set; }
        public DateTime DataCadastro { get; protected set; }
        public DateTime DataNasc { get; protected set; }
        public string OrgaoExpedidor { get; protected set; }

        public string Fone { get; protected set; }

        public string Celular { get; protected set; }

        public string Email { get; protected set; }
        public bool Excluido { get; protected set; }
    }
}
