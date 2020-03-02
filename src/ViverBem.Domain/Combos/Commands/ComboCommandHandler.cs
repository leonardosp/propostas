using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.Combos.Events;
using ViverBem.Domain.Combos.Repository;
using ViverBem.Domain.CommandsHandlers;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Combos.Commands
{
    public class ComboCommandHandler : CommandHandler,
        IHandler<IncuirComboCommand>,
        IHandler<AtualizarComboCommand>,
        IHandler<ExcluirComboCommand>,
        IHandler<RegistrarComboPrincCommand>,
        IHandler<ExcluirComboPrincCommand>,
        IHandler<AtualizarComboPrinciCommand>
    {
        private readonly IBus _bus;
        private readonly IComboRepository _comboRepository;

        public ComboCommandHandler(IUnitOfWork uow, IBus bus,IComboRepository comboRepository, IDomainNotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _bus = bus;
            _comboRepository = comboRepository;
      }

        public void Handle(IncuirComboCommand message)
        {
            var combo = new Combo(message.CodCombo,message.PlanoSrv,message.PlaPrinServ,message.VlrPremio,message.VlrCapital,message.CodComissUsr,message.comboPrincipalId.Value);
            if (!combo.EhValido())
            {
                NotificarValidacoesErro(combo.ValidationResult);
                return;
            }

            _comboRepository.AdicionarComboItem(combo);

            if (Commit())
            {
                _bus.RaiseEvent(new ComboAdicionadoEvent(combo.CodCombo,combo.PlanoSrv,combo.PlaPrinServ,combo.VlrPremio,combo.VlrCapital,combo.ComboPrincId.Value,combo.CodComissUsr,combo.Id));
            }
        }

        public void Handle(AtualizarComboCommand message)
        {
            var comboItemAtual = _comboRepository.ObterComboItemPorComboPrinc(message.ComboId);

            if (!ComboExistente(message.ComboId, message.MessageType)) return;


            var docItem = Combo.ComboFactory.NovoComboCompleto(message.ComboId,message.CodCombo,message.PlanoSrv,message.PlaPrinServ,message.VlrPremio,message.VlrCapital,message.CodComissUsr,message.comboPrincipalId.Value);


            _comboRepository.AtualizarComboItem(docItem);

            if (Commit())
            {
                _bus.RaiseEvent(new ComboAtualizadoEvent(docItem.CodCombo,docItem.PlanoSrv,docItem.PlaPrinServ,docItem.VlrPremio,docItem.VlrCapital,docItem.ComboPrincId.Value,docItem.CodComissUsr,docItem.Id));
            }
        }

        public void Handle(ExcluirComboCommand message)
        {
            if (!ComboExistente(message.Id, message.MessageType)) return;
            var documentoItem = _comboRepository.ObtercomboItemItemPorId(message.Id);

            documentoItem.ExcluirComboItem();

            _comboRepository.AtualizarComboItem(documentoItem);

            if (Commit())
            {
                _bus.RaiseEvent(new ComboExcluidoEvent(message.Id));
            }
        }

        public void Handle(RegistrarComboPrincCommand message)
        {
            var comboPri = new ComboPrinc(message.CodCombo,message.Corretor,message.ComboPrincId);

            if (!comboPri.EhValido())
            {
                NotificarValidacoesErro(comboPri.ValidationResult);
                return;
            }

            var documentoExistente = _comboRepository.Buscar(f => f.comboPrincId == comboPri.comboPrincId);

            if (documentoExistente.Any())
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "O nome do combo foi utilizado"));
            }

            _comboRepository.Adicionar(comboPri);

            if (Commit())
            {
                _bus.RaiseEvent(new ComboPrincRegistradoEvent(comboPri.CodCombo,comboPri.Corretor,comboPri.comboPrincId));
            }
        }

        public void Handle(ExcluirComboPrincCommand message)
        {
            if (!ComboPrincExistente(message.Id, message.MessageType)) return;
            var documento = _comboRepository.ObterPorId(message.Id);

            _comboRepository.Atualizar(documento);

            if (Commit())
            {
                _bus.RaiseEvent(new ComboPrincExcluidoEvent(message.Id));
            }
        }

        public void Handle(AtualizarComboPrinciCommand message)
        {
            var documentoAtual = _comboRepository.ObterPorId(message.Id);

            if (!ComboPrincExistente(message.Id, message.MessageType)) return;


            var doc = ComboPrinc.ComboPrincFactory.NovoComboCompleto(message.Id,message.Corretor,message.CodCombo);


            _comboRepository.Atualizar(doc);

            if (Commit())
            {
                _bus.RaiseEvent(new ComboPrincRegistradoEvent(doc.CodCombo,doc.Corretor,doc.comboPrincId));
            }
        }
        public bool ComboExistente(Guid id, string messageType)
        {
            var comboItem = _comboRepository.ObterComboItemPorComboPrinc(id);

            if (comboItem != null) return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "Combo não encontrado"));
            return false;
        }
        public bool ComboPrincExistente(Guid id, string messageType)
        {
            var doc = _comboRepository.ObterPorId(id);

            if (doc != null) return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "Combo não encontrado"));
            return false;
        }

    }
}
