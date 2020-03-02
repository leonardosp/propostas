using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViveBem.Domain.Core.Events;
using ViveBem.Domain.Core.Notifications;
using ViverBem.Domain.CommandsHandlers;
using ViverBem.Domain.Documentos.Events;
using ViverBem.Domain.Documentos.Repository;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Documentos.Commands
{
    public  class DocumentoConfiguracaoCommandHandler : CommandHandler, 
        IHandler<RegistrarDocumentoConfiguracaoCommand>,
        IHandler<IncluirDocumentoConfiguracaoItemCommand>,
        IHandler<ExcluirDocumentoConfiguracaoItemCommand>,
        IHandler<ExcluirDocumentoConfiguracaoCommand>,
        IHandler<AtualizarDocumentoConfiguracaoCommand>,
        IHandler<AtualizarDocumentoConfiguracaoItemCommand>
    {
        private readonly IBus _bus;
        private readonly IDocumentoConfiguracaoRepository _documentoRepository;

        public DocumentoConfiguracaoCommandHandler(IUnitOfWork uow, IBus bus, IDomainNotificationHandler<DomainNotification> notification, IDocumentoConfiguracaoRepository documentoConfiguracaoRepository) : base(uow, bus, notification)
        {
            _bus = bus;
            _documentoRepository = documentoConfiguracaoRepository;
        }

        public void Handle(RegistrarDocumentoConfiguracaoCommand message)
        {
            var documento = new DocumentoConfiguracao(message.NrSeqDocumentoConfiguracao,message.CdDocumentoConfiguracao,message.NrOrdem,message.NrColuna,message.NoBackGroundColor,message.NoHorizontalAligment,message.NrTamanhoBorda,message.NrMarginLeft,message.NoPropriedadeLista,message.NrParagraphspacing,message.NrWidth);

            if (!documento.EhValido())
            {
                NotificarValidacoesErro(documento.ValidationResult);
                return;
            }

            var documentoExistente = _documentoRepository.Buscar(f => f.CdDocumentoConfiguracao == documento.CdDocumentoConfiguracao);

            if (documentoExistente.Any())
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "O nome do documento foi utilizado"));
            }

            _documentoRepository.Adicionar(documento);

            if (Commit())
            {
                _bus.RaiseEvent(new DocumentoConfiguracaoRegistradoEvent(documento.NrSeqDocumentoConfiguracao,documento.CdDocumentoConfiguracao,documento.NrOrdem,documento.NrColuna,documento.NoBackGroundColor,documento.NoHorizontalAligment,documento.NrTamanhoBorda,documento.NrMarginLeft,documento.NoPropriedadeLista,documento.NrParagraphspacing,documento.NrWidth));
            }
        }

        public void Handle(IncluirDocumentoConfiguracaoItemCommand message)
        {
            var documentoItem = new DocumentoConfiguracaoItem(message.NoFonte,message.NoFonteColor,message.NoFrase,message.FlgPropriedade,message.NrColuna,message.NrseqDocumentoConfiguracaoItem,message.NrFonteTamanho,message.NoTipoFonte,message.NrSeqDocumentoConfiguracao.Value,message.FlgIdentificador,message.FlgImage,message.NrMaxImageWidth,message.NrMaxImageHeight);
            if (!documentoItem.EhValido())
            {
                NotificarValidacoesErro(documentoItem.ValidationResult);
                return;
            }

            _documentoRepository.AdicionarDocumentoItem(documentoItem);

            if (Commit())
            {
                _bus.RaiseEvent(new DocumentoConfiguracaoItemAdicionadoEvent(documentoItem.NoFonte, documentoItem.NoFonteColor, documentoItem.NoFrase, documentoItem.FlgPropriedade, documentoItem.NrColuna, documentoItem.NrseqDocumentoConfiguracaoItem, documentoItem.NrFonteTamanho, documentoItem.NoTipoFonte, documentoItem.NrSeqDocumentoConfiguracao.Value, documentoItem.FlgIdentificador, documentoItem.FlgImage, documentoItem.NrMaxImageWidth, documentoItem.NrMaxImageHeight));
            }
        }

        public void Handle(ExcluirDocumentoConfiguracaoItemCommand message)
        {
            if (!DocumentoItemExistente(message.NrseqDocumentoConfiguracaoItem, message.MessageType)) return;
            var documentoItem = _documentoRepository.ObterDocumentoItemPorId(message.NrseqDocumentoConfiguracaoItem);

           documentoItem.ExcluirDocumentoItem();

            _documentoRepository.AtualizarDocumentoItem(documentoItem);

            if (Commit())
            {
                _bus.RaiseEvent(new DocumentoConfiguracaoItemExcluidoEvent(message.NrseqDocumentoConfiguracaoItem));
            }
        }

        public void Handle(ExcluirDocumentoConfiguracaoCommand message)
        {
            if (!DocumentoExistente(message.NrSeqDocumentoConfiguracao, message.MessageType)) return;
            var documento = _documentoRepository.ObterPorId(message.NrSeqDocumentoConfiguracao);

            documento.ExcluirDocumento();

            _documentoRepository.Atualizar(documento);

            if (Commit())
            {
                _bus.RaiseEvent(new DocumentoConfiguracaoExcluidoEvent(message.NrSeqDocumentoConfiguracao));
            }
        }

        public void Handle(AtualizarDocumentoConfiguracaoCommand message)
        {
            var documentoAtual = _documentoRepository.ObterPorId(message.NrSeqDocumentoConfiguracao);

            if (!DocumentoExistente(message.NrSeqDocumentoConfiguracao, message.MessageType)) return;


            var doc = DocumentoConfiguracao.DocumentoConfiguracaoFactory.NovoDocumentoCompleto(message.NrSeqDocumentoConfiguracao,message.CdDocumentoConfiguracao,message.NrOrdem,message.NrColuna,message.NoBackGroundColor,message.NoHorizontalAligment,message.NrTamanhoBorda,message.NrMarginLeft,message.NoPropriedadeLista,message.NrParagraphspacing,message.NrWidth);

            if (!DocumentoValido(doc)) return;

            _documentoRepository.Atualizar(doc);

            if (Commit())
            {
                _bus.RaiseEvent(new DocumentoConfiguracaoAtualizadoEvent(doc.NrSeqDocumentoConfiguracao,doc.CdDocumentoConfiguracao,doc.NrOrdem,doc.NrColuna,doc.NoBackGroundColor,doc.NoHorizontalAligment,doc.NrTamanhoBorda,doc.NrMarginLeft,doc.NoPropriedadeLista,doc.NrParagraphspacing,doc.NrWidth));
            }
        }

        public void Handle(AtualizarDocumentoConfiguracaoItemCommand message)
        {
            var documentoItemAtual = _documentoRepository.ObterDocumentoItemPorId(message.NrseqDocumentoConfiguracaoItem);

            if (!DocumentoItemExistente(message.NrseqDocumentoConfiguracaoItem, message.MessageType)) return;


            var docItem = DocumentoConfiguracaoItem.DocumentoConfiguracaoItemFactory.NovoDocumentoItemCompleto(message.NoFonte,message.NoFonteColor,message.NoFrase,message.FlgPropriedade,message.NrColuna,message.NrseqDocumentoConfiguracaoItem,message.NrFonteTamanho,message.NoTipoFonte,message.NrSeqDocumentoConfiguracao.Value,message.FlgIdentificador,message.FlgImage,message.NrMaxImageWidth,message.NrMaxImageHeight);

           if (!DocumentoItemValido(docItem)) return;

            _documentoRepository.AtualizarDocumentoItem(docItem);

            if (Commit())
            {
                _bus.RaiseEvent(new DocumentoConfiguracaoItemAtualizadoEvent(docItem.NoFonte,docItem.NoFonteColor,docItem.NoFrase,docItem.FlgPropriedade,docItem.NrColuna,docItem.NrseqDocumentoConfiguracaoItem,docItem.NrFonteTamanho,docItem.NoTipoFonte,docItem.NrSeqDocumentoConfiguracao.Value,message.FlgIdentificador,message.FlgImage,message.NrMaxImageWidth,message.NrMaxImageHeight));
            }
        }

        public bool DocumentoItemExistente(Guid id, string messageType)
        {
            var docItem = _documentoRepository.ObterDocumentoItemPorId(id);

            if (docItem != null) return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "DocumentoItem não encontrado"));
            return false;
        }

        public bool DocumentoExistente(Guid id, string messageType)
        {
            var doc = _documentoRepository.ObterPorId(id);

            if (doc != null) return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "Documento não encontrado"));
            return false;
        }

     

        public bool DocumentoValido(DocumentoConfiguracao doc)
        {
            if (doc.EhValido()) return true;

            NotificarValidacoesErro(doc.ValidationResult);
            return false;
        }

        public bool DocumentoItemValido(DocumentoConfiguracaoItem docItem)
        {
            if (docItem.EhValido()) return true;

            NotificarValidacoesErro(docItem.ValidationResult);
            return false;
        }
    }
}
