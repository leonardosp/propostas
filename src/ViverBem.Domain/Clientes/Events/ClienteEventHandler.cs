using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Clientes.Events
{
    public class ClienteEventHandler :
        IHandler<ClienteRegistradoEvent>,
        IHandler<ClienteAtualizadoEvent>,
        IHandler<ClienteExcluidoEvent>,
        IHandler<EnderecoClienteAdicionadoEvent>,
        IHandler<EnderecoClienteAtualizadoEvent>,
        IHandler<DependenteClienteAdicionadoEvent>,
        IHandler<DependenteClienteAtualizadoEvent>
    {
        public void Handle(EnderecoClienteAdicionadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereco do cliente atualizado com sucesso");
        }

        public void Handle(EnderecoClienteAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereco do cliente adicionado com sucesso");
        }

        public void Handle(ClienteAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cliente Atualizado com sucesso");
        }

        public void Handle(ClienteRegistradoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cliente Registrado com sucesso");
        }

        public void Handle(ClienteExcluidoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cliente Excluido com sucesso");
        }

        public void Handle(DependenteClienteAdicionadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Dependente do cliente atualizado com sucesso");
        }

        public void Handle(DependenteClienteAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Dependente do cliente atualizado com sucesso");
        }
    }
}
