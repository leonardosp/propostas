using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Token.Events
{
    public class TokenEventHandler :
        IHandler<TokenRegistradoEvent>,
        IHandler<TokenAtualizadoEvent>,
        IHandler<TokenExcluidoEvent>
    {
        public void Handle(TokenAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereco do cliente atualizado com sucesso");
        }

        public void Handle(TokenRegistradoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereco do cliente atualizado com sucesso");
        }

        public void Handle(TokenExcluidoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereco do cliente atualizado com sucesso");
        }
    }
}
