using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Application.ViewModels;

namespace ViverBem.Application.Interfaces
{
    public interface IPropostaAppService : IDisposable
    {
        void Registrar(PropostasViewModel propostaViewModel);
        IEnumerable<PropostasViewModel> ObterTodos();
        IEnumerable<PropostasViewModel> ObterPorFuncionario(Guid funcionarioId);
        IEnumerable<DebitoEmContaDisponivel> ObterDebitosEmConta();
        DebitoEmContaDisponivel ObterDebitoEmContaPorId(Guid id);
        IEnumerable<ComboPrincViewModel> ObterCombos();
        IEnumerable<ComboViewModel> ObterCombosPorId(Guid id);
        IEnumerable<ClienteViewModel> ObterClientes();
        IEnumerable<ClienteViewModel> ObterClientePorFuncionario(Guid funcionarioId);

        ClienteViewModel ObterClientePorCpf(string CPF);
        ClienteViewModel ObterClientePorid(Guid id);
        ClienteViewModel ObterClientePorNome(string nome);
        IEnumerable<DependenteViewModel> ObterPorCliente(Guid clienteId);
        IEnumerable<PropostasViewModel> ObterPropostasAprovadas();
        IEnumerable<PropostasViewModel> ObterPropostasRecusadas();
        IEnumerable<PropostasViewModel> ObterPropostasParaAprovar();
        IEnumerable<PropostasViewModel> ObterPropostasPorData(DateTime DtInic, DateTime DtFim);
        IEnumerable<PropostasViewModel> ObterPropostasAprovadasPorData(DateTime DtInic, DateTime DtFim);
        IEnumerable<PropostasViewModel> ObterPropostasRecusadasPorData(DateTime DtInic, DateTime DtFim);
        PropostasViewModel ObterUltimaProposta();
        PropostasViewModel ObterPorId(Guid id);
        PropostasViewModel ObterPorCpf(string cpf);
        IEnumerable<PropostasViewModel> ObterPropostasComPronum();
        void Atualizar(PropostasViewModel propostaViewModel);
        void Excluir(Guid id);
    }
}
