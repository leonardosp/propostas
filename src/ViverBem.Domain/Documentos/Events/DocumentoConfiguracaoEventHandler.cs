using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Documentos.Events
{
    public class DocumentoConfiguracaoEventHandler : 
        IHandler<DocumentoConfiguracaoRegistradoEvent>,
        IHandler<DocumentoConfiguracaoExcluidoEvent>,
        IHandler<DocumentoConfiguracaoAtualizadoEvent>,
        IHandler<DocumentoConfiguracaoItemAtualizadoEvent>,
        IHandler<DocumentoConfiguracaoItemExcluidoEvent>,
        IHandler<DocumentoConfiguracaoItemAdicionadoEvent>

    {
        public void Handle(DocumentoConfiguracaoRegistradoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Documento Configuração registrado com sucesso");
        }

        public void Handle(DocumentoConfiguracaoExcluidoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Documento Configuração Excluido com sucesso");
        }

        public void Handle(DocumentoConfiguracaoAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Documento Configuração Atualizado com sucesso");
        }

        public void Handle(DocumentoConfiguracaoItemAtualizadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Documento Configuração Atualizado com sucesso");
        }

        public void Handle(DocumentoConfiguracaoItemExcluidoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Documento Configuração Excluido com sucesso");
        }

        public void Handle(DocumentoConfiguracaoItemAdicionadoEvent message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Documento Configuração Adicionado com sucesso");
        }
    }
}
