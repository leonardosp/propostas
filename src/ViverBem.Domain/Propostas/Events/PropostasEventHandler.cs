using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Propostas.Events
{
    public class PropostasEventHandler :
        IHandler<PropostasRegistradoEvent>,
        IHandler<PropostasExcluidaEvent>,
        IHandler<PropostasAtualizadoEventHandler>
    {
        public void Handle(PropostasRegistradoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine("Endereco do cliente atualizado com sucesso");
        }

        public void Handle(PropostasExcluidaEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereco do cliente atualizado com sucesso");
        }

        public void Handle(PropostasAtualizadoEventHandler message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Endereco do cliente atualizado com sucesso");
        }
    }
}
