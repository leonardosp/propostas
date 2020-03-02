using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.CommandsHandlers;
using ViverBem.Domain.Funcionarios.Events;
using ViverBem.Domain.Funcionarios.Repository;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Funcionarios.Commands
{
    public class FuncionarioCommandHandler : CommandHandler, IHandler<RegistrarFuncionarioCommand>
    {
        private readonly IBus _bus;
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FuncionarioCommandHandler(IUnitOfWork uow, IBus bus, IDomainNotificationHandler<DomainNotification> notification, IFuncionarioRepository funcionarioRepository) : base(uow, bus, notification)
        {
            _bus = bus;
            _funcionarioRepository = funcionarioRepository;
        }
        public void Handle(RegistrarFuncionarioCommand message)
        {
            var funcionario = new Funcionario(message.Id, message.Nome, message.Email);

            if (!funcionario.EhValido())
            {
                NotificarValidacoesErro(funcionario.ValidationResult);
                return;
            }

            var funcionarioExistente = _funcionarioRepository.Buscar(f => f.Email == funcionario.Email);

            if (funcionarioExistente.Any())
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Já existe um Documento Configuração criado com este nome"));
            }

            _funcionarioRepository.Adicionar(funcionario);

            if (Commit())
            {
                _bus.RaiseEvent(new FuncionarioRegistradoEvent(funcionario.Id, funcionario.Nome, funcionario.Email));
            }
        }
    }
}
