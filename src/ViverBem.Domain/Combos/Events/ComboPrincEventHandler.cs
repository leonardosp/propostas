using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Combos.Events
{
    public class ComboPrincEventHandler :
        IHandler<ComboPrincRegistradoEvent>,
        IHandler<ComboPrincExcluidoEvent>,
        IHandler<ComboPrincAtualizadoEvent>,
        IHandler<ComboAdicionadoEvent>,
        IHandler<ComboAtualizadoEvent>,
        IHandler<ComboExcluidoEvent>
    {
        public void Handle(ComboPrincRegistradoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ComboPrinc registrado com sucesso");
        }

        public void Handle(ComboPrincExcluidoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ComboPrinc excluido com sucesso");
        }

        public void Handle(ComboPrincAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Combo Princ atualizado com sucesso");
        }

        public void Handle(ComboAdicionadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Combo adicionado com sucesso");
        }

        public void Handle(ComboAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Combo Atualizado com sucesso");
        }

        public void Handle(ComboExcluidoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Combo  excluido com sucesso");
        }
    }
}
