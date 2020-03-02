using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.Clientes.Events;
using ViverBem.Domain.Clientes.Repository;
using ViverBem.Domain.CommandsHandlers;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Clientes.Commands
{
    public class ClienteCommandHandler : CommandHandler,
        IHandler<RegistrarClienteCommand>,
        IHandler<AtualizarClienteCommand>,
        IHandler<ExcluirClienteCommand>,
        IHandler<IncluirEnderecoClienteCommand>,
        IHandler<AtualizarEnderecoClienteCommand>,
        IHandler<AtualizarDependenteClienteCommand>,
        IHandler<IncluirDependenteClienteCommand>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IBus _bus;
        private readonly IUser _user;

        public ClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork uow, IBus bus, IDomainNotificationHandler<DomainNotification> notifications, IUser user) : base(uow, bus, notifications)
        {
            _clienteRepository = clienteRepository;
            _bus = bus;
            _user = user;
        }

        public void Handle(RegistrarClienteCommand message)
        {
            var endereco = new Endereco(message.Endereco.Id, message.Endereco.Logradouro, message.Endereco.Numero, message.Endereco.Complemento, message.Endereco.Bairro, message.Endereco.CEP, message.Endereco.Cidade, message.Endereco.Estado, message.Id);
            var cliente = Cliente.ClienteFactory.NovoClienteCompleto(message.Id,message.Nome,message.CPF,message.RG,message.DataExpedicao,message.DataNasc,DateTime.Now,
                message.OrgaoExpedidor,message.Fone,message.Celular,message.Email,message.EstadoCivil,message.Sexo,message.Ocupacao,message.CodigoProf,message.PPE,message.FuncionarioId,endereco);

            if (!ClienteValido(cliente)) return;

            //Validações 

            //Persistencia
            _clienteRepository.Adicionar(cliente);

            if (Commit())
            {
                Console.WriteLine("Evento Registrado com sucesso!");
                _bus.RaiseEvent(new ClienteRegistradoEvent(cliente.Id,cliente.Nome,cliente.CPF,cliente.RG,cliente.DataExpedicao,cliente.DataNasc,DateTime.Now,cliente.OrgaoExpedidor,cliente.Fone,cliente.Celular,cliente.Sexo,cliente.EstadoCivil,cliente.Email,cliente.Ocupacao,cliente.PPE,cliente.CodigoProf));
            }
        }

        public void Handle(AtualizarClienteCommand message)
        {
            var clienteAtual = _clienteRepository.ObterPorId(message.Id);

            if (!ClienteExistente(message.Id, message.MessageType)) return;


            var cliente = Cliente.ClienteFactory.NovoClienteCompleto(message.Id, message.Nome, message.CPF,message.RG,message.DataExpedicao,message.DataNasc,DateTime.Now,message.OrgaoExpedidor,message.Fone,message.Celular,message.Email ,message.EstadoCivil,message.Sexo,message.Ocupacao,message.CodigoProf,message.PPE, message.FuncionarioId, clienteAtual.Endereco);

            if (!ClienteValido(cliente)) return;

            _clienteRepository.Atualizar(cliente);

            if (Commit())
            {
                _bus.RaiseEvent(new ClienteAtualizadoEvent(cliente.Id,cliente.Nome,cliente.CPF,cliente.RG,cliente.DataExpedicao,cliente.DataNasc,cliente.DataCadastro,cliente.OrgaoExpedidor,cliente.Fone,cliente.Celular,cliente.Email,cliente.Sexo,cliente.EstadoCivil,cliente.CodigoProf,cliente.Ocupacao,cliente.PPE));
            }
        }

        public void Handle(ExcluirClienteCommand message)
        {
            if (!ClienteExistente(message.Id, message.MessageType)) return;
            var clienteAtual = _clienteRepository.ObterPorId(message.Id);

            clienteAtual.ExcluirCliente();

            _clienteRepository.Atualizar(clienteAtual);

            if (Commit())
            {
                _bus.RaiseEvent(new ClienteExcluidoEvent(message.Id));
            }
        }

        public bool ClienteValido(Cliente cliente)
        {
            if (cliente.EhValido()) return true;

            NotificarValidacoesErro(cliente.ValidationResult);
            return false;
        }

        public bool ClienteExistente(Guid id, string messageType)
        {
            var cliente = _clienteRepository.ObterPorId(id);

            if (cliente != null) return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "Cliente não encontrado"));
            return false;
        }

        public void Handle(IncluirEnderecoClienteCommand message)
        {
            var endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.CEP, message.Cidade, message.Estado, message.ClienteId.Value);
            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return;
            }

            _clienteRepository.AdicionarEndereco(endereco);

            if (Commit())
            {
                _bus.RaiseEvent(new EnderecoClienteAdicionadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.CEP, endereco.Cidade, endereco.Estado, endereco.ClienteId.Value));
            }
        }

        public void Handle(AtualizarEnderecoClienteCommand message)
        {
            var endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.CEP, message.Cidade, message.Estado, message.ClienteId.Value);
            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return;
            }

            _clienteRepository.AtualizarEndereco(endereco);

            if (Commit())
            {
                _bus.RaiseEvent(new EnderecoClienteAtualizadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.CEP, endereco.Cidade, endereco.Estado, endereco.ClienteId.Value));
            }
        }

        public void Handle(AtualizarDependenteClienteCommand message)
        {
            var dependente = new Dependente(message.Id, message.Nome, message.Parentesco, message.Participacao, message.ClienteId.Value);
            if (!dependente.EhValido())
            {
                NotificarValidacoesErro(dependente.ValidationResult);
                return;
            }

            _clienteRepository.AtualizarDependente(dependente);

            if (Commit())
            {
                _bus.RaiseEvent(new DependenteClienteAtualizadoEvent(dependente.Id, dependente.Nome, dependente.DataNascimento, dependente.Parentesco, dependente.Participacao, dependente.ClienteId.Value));
            }
        }

        public void Handle(IncluirDependenteClienteCommand message)
        {
            var dependente = new Dependente(message.Id, message.Nome, message.Parentesco, message.Participacao, message.ClienteId.Value);
            if (!dependente.EhValido())
            {
                NotificarValidacoesErro(dependente.ValidationResult);
                return;
            }

            _clienteRepository.AdicionarDependente(dependente);

            if (Commit())
            {
                _bus.RaiseEvent(new DependenteClienteAdicionadoEvent(dependente.Id,dependente.Nome,dependente.Parentesco,dependente.Participacao, dependente.ClienteId.Value));
            }
        }
    }
}
