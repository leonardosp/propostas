using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Clientes;
using ViverBem.Domain.Combos;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Propostas.Repository
{
    public interface IPropostasRepository : IRepository<Propostas>
    {
        IEnumerable<Propostas> ObterPropostaPorFuncionario(Guid organizadorId);
        IEnumerable<Cliente> ObterClientes();
        
        IEnumerable<Cliente> ObterClientesPorFuncionario(Guid funcionarioId);
        Cliente ObterClientePorCpf(string Cpf);
        Cliente ObterClientePorid(Guid id);
        Cliente ObterClientePorNome(string nome);
        IEnumerable<Dependente> ObterPorCliente(Guid clienteId);
        IEnumerable<ComboPrinc> ObterCombos();
        IEnumerable<Combo> ObterCombosPorId(Guid id);
        DebitoEmConta ObterDebitoEmContaPorId(Guid id);
        IEnumerable<DebitoEmConta> ObterDebitoEmConta();
        IEnumerable<Propostas> ObterPropostasRecusadas();
        IEnumerable<Propostas> ObterPropostasAceitas();
        IEnumerable<Propostas> ObterPropostasParaAprovar();
        IEnumerable<Propostas> ObterPropostasPorData(DateTime DtInic, DateTime DtFin);
        IEnumerable<Propostas> ObterPropostasAprovadasPorData(DateTime DtInic, DateTime DtFin);
        IEnumerable<Propostas> ObterPropostasRecusadasPorData(DateTime DtInic, DateTime DtFin);
        IEnumerable<Propostas> ObterPropostasComPronum();
        Propostas ObterUltimaProposta();
        Propostas ObterPropostaPorCpf(string cpf);
    }
}
