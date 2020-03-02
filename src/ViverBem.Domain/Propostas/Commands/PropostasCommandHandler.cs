using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.Clientes.Events;
using ViverBem.Domain.CommandsHandlers;
using ViverBem.Domain.Interfaces;
using ViverBem.Domain.Propostas.Events;
using ViverBem.Domain.Propostas.Repository;

namespace ViverBem.Domain.Propostas.Commands
{
    public class PropostasCommandHandler : CommandHandler,
        IHandler<RegistrarPropostasCommand>,
        IHandler<ExcluirPropostasCommand>,
        IHandler<AtualizarPropostasCommand>
    {
        private readonly IPropostasRepository _propostasRepository;
        private readonly IBus _bus;
        private readonly IUser _user;

        public PropostasCommandHandler(IPropostasRepository propostasRepository, IUnitOfWork uow, IBus bus, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(uow, bus, notifications)
        {
            _propostasRepository = propostasRepository;
            _bus = bus;
            _user = user;
        }

        public void Handle(ExcluirPropostasCommand message)
        {
            if (!PropostaExistente(message.Id, message.MessageType)) return;
            var clienteAtual = _propostasRepository.ObterPorId(message.Id);

            clienteAtual.ExcluirCliente();

            _propostasRepository.Atualizar(clienteAtual);

            if (Commit())
            {
                _bus.RaiseEvent(new ClienteExcluidoEvent(message.Id));
            }
        }

        public void Handle(RegistrarPropostasCommand message)
        {
            var cliente = Propostas.PropostasFactory.NovaPropostaCompleto(message.Id, message.Corretor, message.CPF, message.Nome, message.Dt_nasc, message.Sexo, message.Est_civil, message.Ender, message.Endnum, message.EndComplemento, message.Bairro, message.Cidade, message.UF, message.CEP, message.DDDTel, message.Tel, message.DDDCel, message.Cel, message.Identidade, message.Org_emissor, message.Email, message.Cod_prof, message.Ocupacao, message.Dt_venda,DateTime.Now, message.Id_ext_props, message.CodCombo, message.VlrPremio, message.VlrCapital, message.Tp_pgto, message.Banco, message.Agencia, message.Agencia_dv, message.CC, message.Cc_dv, message.Dia_debito, message.Tipo_envio, message.Endercobr, message.Endnumcobr, message.Endcomplcobr, message.Bairrocobr, message.Cidadecobr, message.Ufcobr, message.Cepcobr,message.Pronum,message.AssMatrc,message.Prodtpag,message.StatusConsulta,message.StatusFinanceiro, message.PPE,message.Aprovado,message.Excluido, message.FuncionarioId,message.IdDoClienteX);

            //Validações 

            //Persistencia
            _propostasRepository.Adicionar(cliente);

            if (Commit())
            {
                //Console.WriteLine("Evento Registrado com sucesso!");
                _bus.RaiseEvent(new PropostasRegistradoEvent(cliente.DataVenda, cliente.Id_ext_props, cliente.Tp_pgto, cliente.CPF, cliente.Nome, cliente.Dt_nasc, cliente.Sexo, cliente.Est_civil, cliente.Ender, cliente.Endnum, cliente.EndComplemento, cliente.Bairro, cliente.Cidade, cliente.UF, cliente.CEP, cliente.DDDTel, cliente.Tel, cliente.DDDCel, cliente.Cel, cliente.Identidade, cliente.Org_emissor, cliente.Email, cliente.Cod_prof, cliente.Ocupacao, cliente.Dt_venda,DateTime.Now, cliente.Id_ext_props, cliente.CodCombo, cliente.VlrPremio, cliente.VlrCapital, cliente.Tp_pgto, cliente.Banco, cliente.Agencia, cliente.Agencia_dv, cliente.CC, cliente.Cc_dv, cliente.Dia_debito, cliente.Tipo_envio, cliente.Endercobr, cliente.Endnumcobr, cliente.Endcomplcobr, cliente.PPE,cliente.Bairrocobr, cliente.Cidadecobr, cliente.Ufcobr, cliente.Cepcobr,cliente.IdDoClienteX));
            }
        }

        public void Handle(AtualizarPropostasCommand message)
        {
            var clienteAtual = _propostasRepository.ObterPorId(message.Id);

            if (!PropostaExistente(message.Id, message.MessageType)) return;


            var cliente = Propostas.PropostasFactory.NovaPropostaCompleto(message.Id, message.Corretor, message.CPF, message.Nome, message.Dt_nasc, message.Sexo, message.Est_civil, message.Ender, message.Endnum, message.EndComplemento, message.Bairro, message.Cidade, message.UF, message.CEP, message.DDDTel, message.Tel, message.DDDCel, message.Cel, message.Identidade, message.Org_emissor, message.Email, message.Cod_prof, message.Ocupacao, message.Dt_venda,DateTime.Now, message.Id_ext_props, message.CodCombo, message.VlrPremio, message.VlrCapital, message.Tp_pgto, message.Banco, message.Agencia, message.Agencia_dv, message.CC, message.Cc_dv, message.Dia_debito, message.Tipo_envio, message.Endercobr, message.Endnumcobr, message.Endcomplcobr, message.Bairrocobr,message.Cidadecobr,message.Ufcobr,message.Cepcobr,message.Pronum,message.AssMatrc,message.Prodtpag,message.StatusConsulta,message.StatusFinanceiro,message.PPE,message.Aprovado,message.Excluido,message.FuncionarioId,message.IdDoClienteX);

            _propostasRepository.Atualizar(cliente);

            if (Commit())
            {
                _bus.RaiseEvent(new PropostasAtualizadoEventHandler(cliente.DataVenda,cliente.Id_ext_props,cliente.Tp_pgto,cliente.CPF,cliente.Nome,cliente.Dt_nasc,cliente.Sexo,cliente.Est_civil,cliente.Ender,cliente.Endnum,cliente.EndComplemento,cliente.Bairro,cliente.Cidade,cliente.UF,cliente.CEP,cliente.DDDTel,cliente.Tel,cliente.DDDCel,cliente.Cel,cliente.Identidade,cliente.Org_emissor,cliente.Email,cliente.Cod_prof,cliente.Ocupacao,cliente.Dt_venda,DateTime.Now,cliente.Id_ext_props,cliente.CodCombo,cliente.VlrPremio,cliente.VlrCapital,cliente.Tp_pgto,cliente.Banco,cliente.Agencia,cliente.Agencia_dv,cliente.CC,cliente.Cc_dv,cliente.Dia_debito,cliente.Tipo_envio,cliente.Endercobr,cliente.Endnumcobr,cliente.Endcomplcobr,cliente.Bairrocobr,cliente.Cidadecobr,cliente.Ufcobr,cliente.Cepcobr,cliente.Pronum,cliente.Assmatrc,cliente.Prodtpag,cliente.StatusConsulta,cliente.StatusFinanceiro,cliente.PPE,cliente.Aprovado,cliente.Excluido,message.IdDoClienteX));
            }
        }
        public bool PropostaExistente(Guid id, string messageType)
        {
            var cliente = _propostasRepository.ObterPorId(id);

            if (cliente != null) return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "Proposta não encontrada"));
            return false;
        }
    }
}
