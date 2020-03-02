using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Application.ViewModels;

namespace ViverBem.Application.Interfaces
{
    public interface IClienteAppService : IDisposable
    {
        void Registrar(ClienteViewModel clienteViewModel);
        IEnumerable<ClienteViewModel> ObterTodos();
        IEnumerable<ClienteViewModel> ObterPorFuncionario(Guid funcionarioId);
        ClienteViewModel ObterPorId(Guid id);
        ClienteViewModel ObterPorCpf(String Cpf);
        IEnumerable<ComboPrincViewModel> ObterCombos();
        ComboViewModel ObterComboSecundario(Guid id);
        IEnumerable<ProfissaoViewModel> ObterProfissoes();
        ProfissaoViewModel ObterProfissaoPorId(Int32 id);
        void Atualizar(ClienteViewModel clienteViewModel);
        void Excluir(Guid id);
        void AdicionarEndereco(EnderecoViewModel enderecoViewModel);
        void AtualizarEndereco(EnderecoViewModel enderecoViewModel);
        void AdicionarDependente(DependenteViewModel dependenteViewModel);
        void AtualizarDependente(DependenteViewModel dependenteViewModel);
        EnderecoViewModel ObterEnderecoPorId(Guid id);
        IEnumerable<DependenteViewModel> ObterPorCliente(Guid clienteId);
    }
}
