using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Clientes.Commands;
using ViverBem.Domain.Clientes.Repository;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Application.Services
{
    public class ClienteAppService : IClienteAppService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clienteRepository;
        private readonly IUser _user;

        public ClienteAppService(IBus bus, IMapper mapper, IClienteRepository clienteRepository, IUser user)
        {
            _bus = bus;
            _mapper = mapper;
            _clienteRepository = clienteRepository;
            _user = user;
        }
        public void AdicionarEndereco(EnderecoViewModel enderecoViewModel)
        {
            var enderecoCommand = _mapper.Map<IncluirEnderecoClienteCommand>(enderecoViewModel);
            _bus.SendCommand(enderecoCommand);
        }

        public void Atualizar(ClienteViewModel clienteViewModel)
        {
            var atualizarClienteCommand = _mapper.Map<AtualizarClienteCommand>(clienteViewModel);
            _bus.SendCommand(atualizarClienteCommand);
        }

        public void AtualizarEndereco(EnderecoViewModel enderecoViewModel)
        {
            var enderecoCommand = _mapper.Map<AtualizarEnderecoClienteCommand>(enderecoViewModel);
            _bus.SendCommand(enderecoCommand);
        }

        public void Dispose()
        {
            _clienteRepository.Dispose();
        }

        public void Excluir(Guid id)
        {
            _bus.SendCommand(new ExcluirClienteCommand(id));
        }

        public EnderecoViewModel ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoViewModel>(_clienteRepository.ObterEnderecoPorId(id));
        }

        public ClienteViewModel ObterPorId(Guid id)
        {
            return _mapper.Map<ClienteViewModel>(_clienteRepository.ObterPorId(id));
        }

        public IEnumerable<ClienteViewModel> ObterPorFuncionario(Guid funcionarioId)
        {
            return _mapper.Map<IEnumerable<ClienteViewModel>>(_clienteRepository.ObterClientePorFuncionario(funcionarioId));
        }

        public IEnumerable<ClienteViewModel> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ClienteViewModel>>(_clienteRepository.ObterTodos());
        }

        public void Registrar(ClienteViewModel clienteViewModel)
        {
            var registroCommand = _mapper.Map<RegistrarClienteCommand>(clienteViewModel);
            _bus.SendCommand(registroCommand);
        }

        public void AdicionarDependente(DependenteViewModel dependenteViewModel)
        {
            var dependenteCommand = _mapper.Map<IncluirDependenteClienteCommand>(dependenteViewModel);
            _bus.SendCommand(dependenteCommand);
        }

        public void AtualizarDependente(DependenteViewModel dependenteViewModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DependenteViewModel> ObterPorCliente(Guid clienteId)
        {
            return _mapper.Map<IEnumerable<DependenteViewModel>>(_clienteRepository.ObterDependentePorCliente(clienteId));
        }

        public IEnumerable<ComboPrincViewModel> ObterCombos()
        {
            return _mapper.Map<IEnumerable<ComboPrincViewModel>>(_clienteRepository.ObterCombos());
        }

        public ComboViewModel ObterComboSecundario(Guid id)
        {
            return _mapper.Map<ComboViewModel>(_clienteRepository.ObterComboSecundario(id));
        }

        public IEnumerable<ProfissaoViewModel> ObterProfissoes()
        {
            return _mapper.Map<IEnumerable<ProfissaoViewModel>>(_clienteRepository.ObterProfissoes());
        }

        public ProfissaoViewModel ObterProfissaoPorId(Int32 id)
        {
            return _mapper.Map<ProfissaoViewModel>(_clienteRepository.ObterProfissaoPorId(id));
        }

        public ClienteViewModel ObterPorCpf(string Cpf)
        {
            return _mapper.Map<ClienteViewModel>(_clienteRepository.ObterPorCpf(Cpf));
        }
    }
}
