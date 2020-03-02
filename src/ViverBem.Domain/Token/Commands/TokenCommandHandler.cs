using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.Clientes.Repository;
using ViverBem.Domain.CommandsHandlers;
using ViverBem.Domain.Interfaces;
using ViverBem.Domain.Token.Events;
using ViverBem.Domain.Token.Repository;

namespace ViverBem.Domain.Token.Commands
{
    public class TokenCommandHandler : CommandHandler,
        IHandler<RegistrarTokenCommand>,
        IHandler<AtualizarTokenCommand>,
        IHandler<ExcluirTokenCommand>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IBus _bus;
        private readonly IUser _user;

        public TokenCommandHandler(ITokenRepository tokenRepository, IUnitOfWork uow, IBus bus, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(uow, bus, notifications)
        {
            _tokenRepository = tokenRepository;
            _bus = bus;
            _user = user;
        }
        public void Handle(RegistrarTokenCommand message)
        {
            var token = LoginTokenResult.TokenFactory.NovoToken(message.Id, message.AccessToken,message.Error,message.ErrorDescription,message.RETORNOTOTAL,message.LimiteDoToken);

            //Persistencia
            _tokenRepository.Adicionar(token);

            if (Commit())
            {
                Console.WriteLine("Token Registrado com sucesso!");
                _bus.RaiseEvent(new TokenRegistradoEvent(token.Id,token.AccessToken,token.Error,token.ErrorDescription,token.LimiteDoToken));
            }
        }

        public void Handle(AtualizarTokenCommand message)
        {
            var tokenAtual = _tokenRepository.ObterPorId(message.Id);


            var token = LoginTokenResult.TokenFactory.NovoToken(message.Id,message.AccessToken,message.Error,message.ErrorDescription,message.RETORNOTOTAL,message.LimiteDoToken);


            _tokenRepository.Atualizar(token);

            if (Commit())
            {
                _bus.RaiseEvent(new TokenAtualizadoEvent(token.Id,token.AccessToken,token.Error,token.ErrorDescription,token.LimiteDoToken));
            }
        }

        public void Handle(ExcluirTokenCommand message)
        {
            var tokenAtual = _tokenRepository.ObterPorId(message.Id);

            _tokenRepository.Atualizar(tokenAtual);

            if (Commit())
            {
                _bus.RaiseEvent(new TokenExcluidoEvent(message.Id));
            }
        }
    }
}
