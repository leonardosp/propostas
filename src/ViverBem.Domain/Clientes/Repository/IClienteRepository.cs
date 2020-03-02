using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Combos;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Clientes.Repository
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        IEnumerable<Cliente> ObterClientePorFuncionario(Guid organizadorId);
        Cliente ObterPorCpf(string cpf);
        IEnumerable<Dependente> ObterDependentePorCliente(Guid clienteId);
        IEnumerable<ComboPrinc> ObterCombos();
        Combo ObterComboSecundario(Guid id);
        IEnumerable<Profissao> ObterProfissoes();
        Profissao ObterProfissaoPorId(Int32 id);
        Endereco ObterEnderecoPorId(Guid id);
        void AdicionarEndereco(Endereco endereco);
        void AtualizarEndereco(Endereco endereco);
        void AdicionarDependente(Dependente dependente);
        void AtualizarDependente(Dependente dependente);
    }
}
