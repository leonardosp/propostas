using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Interfaces;
using ViverBem.Domain.Token.Commands;
using ViverBem.Domain.Token.Repository;

namespace ViverBem.Application.Services
{
    public class TokenAppService : ITokenAppService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUser _user;

        public TokenAppService(IBus bus, IMapper mapper, ITokenRepository tokenRepository, IUser user)
        {
            _bus = bus;
            _mapper = mapper;
            _tokenRepository = tokenRepository;
            _user = user;
        }

        public void Atualizar(TokenResultViewModel clienteViewModel)
        {
            var atualizarToken = _mapper.Map<AtualizarTokenCommand>(clienteViewModel);
            _bus.SendCommand(atualizarToken);
        }

        public void Dispose()
        {
            _tokenRepository.Dispose();
        }

        public void Excluir(Guid id)
        {
            _bus.SendCommand(new ExcluirTokenCommand(id));
        }

        public TokenResultViewModel ObterPorId(Guid id)
        {
            return _mapper.Map<TokenResultViewModel>(_tokenRepository.ObterPorId(id));
        }

        public IEnumerable<TokenResultViewModel> ObterTodos()
        {
            return _mapper.Map<IEnumerable<TokenResultViewModel>>(_tokenRepository.ObterTodos());
        }

        public void Registrar(TokenResultViewModel tokenViewModel)
        {
            var registroCommand = _mapper.Map<RegistrarTokenCommand>(tokenViewModel);
            _bus.SendCommand(registroCommand);
        }
    }
}
