using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Combos.Commands;
using ViverBem.Domain.Combos.Repository;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Application.Services
{
    public class ComboAppService : IComboAppService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IComboRepository _comboRepository;
        private readonly IUser _user;

        public ComboAppService(IBus bus, IMapper mapper, IComboRepository comboRepository, IUser user)
        {
            _bus = bus;
            _mapper = mapper;
            _comboRepository = comboRepository;
            _user = user;
        }

        public void AdicionarComboItem(ComboViewModel comboViewModel)
        {
            var docItemCommand = _mapper.Map<IncuirComboCommand>(comboViewModel);
            _bus.SendCommand(docItemCommand);
        }

        public void Atualizar(ComboPrincViewModel comboPrinc)
        {
            var atualizarDocumentoCommand = _mapper.Map<AtualizarComboPrinciCommand>(comboPrinc);
            _bus.SendCommand(atualizarDocumentoCommand);
        }

        public void AtualizarDocumentoItem(ComboViewModel comboViewModel)
        {
            var atualizarDocumentoItemCommand = _mapper.Map<AtualizarComboCommand>(comboViewModel);
            _bus.SendCommand(atualizarDocumentoItemCommand);
        }

        public void Dispose()
        {
            _comboRepository.Dispose();
        }

        public void Excluir(Guid id)
        {
            _bus.SendCommand(new ExcluirComboPrincCommand(id));
        }

        public IEnumerable<ComboViewModel> ObterPorComboPrinc(Guid comboId)
        {
            return _mapper.Map<IEnumerable<ComboViewModel>>(_comboRepository.ObterComboItemPorComboPrinc(comboId));
        }

        public ComboPrincViewModel ObterPorId(Guid id)
        {
            return _mapper.Map<ComboPrincViewModel>(_comboRepository.ObterPorId(id));
        }

        public IEnumerable<ComboPrincViewModel> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ComboPrincViewModel>>(_comboRepository.ObterTodos());
        }

        public void Registrar(ComboPrincViewModel documentoConfiguracao)
        {
            var registroCommand = _mapper.Map<RegistrarComboPrincCommand>(documentoConfiguracao);
            _bus.SendCommand(registroCommand);
        }
    }
}
