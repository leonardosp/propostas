using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Interfaces;
using ViverBem.Domain.Propostas.Commands;
using ViverBem.Domain.Propostas.Repository;

namespace ViverBem.Application.Services
{
    public class PropostaAppService : IPropostaAppService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IPropostasRepository _propostasRepository;
        private readonly IUser _user;

        public PropostaAppService(IBus bus, IMapper mapper, IPropostasRepository propostasRepository, IUser user)
        {
            _bus = bus;
            _mapper = mapper;
            _propostasRepository = propostasRepository;
            _user = user;
        }

        public void Atualizar(PropostasViewModel propostaViewModel)
        {
            var atualizarClienteCommand = _mapper.Map<AtualizarPropostasCommand>(propostaViewModel);
            _bus.SendCommand(atualizarClienteCommand);
        }

        public void Dispose()
        {
            _propostasRepository.Dispose();
        }

        public void Excluir(Guid id)
        {
            _bus.SendCommand(new ExcluirPropostasCommand(id));
        }

        public ClienteViewModel ObterClientePorCpf(string CPF)
        {
            return _mapper.Map<ClienteViewModel>(_propostasRepository.ObterClientePorCpf(CPF));
        }

        public IEnumerable<ClienteViewModel> ObterClientePorFuncionario(Guid funcionarioId)
        {
            return _mapper.Map<IEnumerable<ClienteViewModel>>(_propostasRepository.ObterClientesPorFuncionario(funcionarioId));
        }

        public ClienteViewModel ObterClientePorid(Guid id)
        {
            return _mapper.Map<ClienteViewModel>(_propostasRepository.ObterClientePorid(id));
        }

        public ClienteViewModel ObterClientePorNome(string nome)
        {
            return _mapper.Map<ClienteViewModel>(_propostasRepository.ObterClientePorNome(nome));
        }

        public IEnumerable<ClienteViewModel> ObterClientes()
        {
            return _mapper.Map<IEnumerable<ClienteViewModel>>(_propostasRepository.ObterClientes());
        }

        public IEnumerable<ComboPrincViewModel> ObterCombos()
        {
            return _mapper.Map<IEnumerable<ComboPrincViewModel>>(_propostasRepository.ObterCombos());
        }

        public IEnumerable<ComboViewModel> ObterCombosPorId(Guid id)
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(_propostasRepository.ObterCombosPorId(id));
        }

        public DebitoEmContaDisponivel ObterDebitoEmContaPorId(Guid id)
        {
            return _mapper.Map<DebitoEmContaDisponivel>(_propostasRepository.ObterDebitoEmContaPorId(id));
        }

        public IEnumerable<DebitoEmContaDisponivel> ObterDebitosEmConta()
        {
            return _mapper.Map<IEnumerable<DebitoEmContaDisponivel>>(_propostasRepository.ObterDebitoEmConta());
        }

        public IEnumerable<DependenteViewModel> ObterPorCliente(Guid clienteId)
        {
            return _mapper.Map<IEnumerable<DependenteViewModel>>(_propostasRepository.ObterPorCliente(clienteId));
        }

        public PropostasViewModel ObterPorCpf(string cpf)
        {
            return _mapper.Map<PropostasViewModel>(_propostasRepository.ObterPropostaPorCpf(cpf));
        }

        public IEnumerable<PropostasViewModel> ObterPorFuncionario(Guid funcionarioId)
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostaPorFuncionario(funcionarioId));
        }

        public PropostasViewModel ObterPorId(Guid id)
        {
            return _mapper.Map<PropostasViewModel>(_propostasRepository.ObterPorId(id));
        }

        public IEnumerable<PropostasViewModel> ObterPropostasAprovadas()
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostasAceitas());
        }

        public IEnumerable<PropostasViewModel> ObterPropostasAprovadasPorData(DateTime DtInic, DateTime DtFim)
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostasAprovadasPorData(DtInic, DtFim));
        }

        public IEnumerable<PropostasViewModel> ObterPropostasComPronum()
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostasComPronum());
        }

        public IEnumerable<PropostasViewModel> ObterPropostasParaAprovar()
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostasParaAprovar());
        }

        public IEnumerable<PropostasViewModel> ObterPropostasPorData(DateTime DtInic, DateTime DtFim)
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostasPorData(DtInic,DtFim));
        }

        public IEnumerable<PropostasViewModel> ObterPropostasRecusadas()
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostasRecusadas());
        }

        public IEnumerable<PropostasViewModel> ObterPropostasRecusadasPorData(DateTime DtInic, DateTime DtFim)
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterPropostasRecusadasPorData(DtInic, DtFim));
        }

        public IEnumerable<PropostasViewModel> ObterTodos()
        {
            return _mapper.Map<IEnumerable<PropostasViewModel>>(_propostasRepository.ObterTodos());
        }

        public PropostasViewModel ObterUltimaProposta()
        {
            return _mapper.Map<PropostasViewModel>(_propostasRepository.ObterUltimaProposta());
        }

        public void Registrar(PropostasViewModel propostaViewModel)
        {
            var registroCommand = _mapper.Map<RegistrarPropostasCommand>(propostaViewModel);
            _bus.SendCommand(registroCommand);
        }
    }
}
