using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Documentos.Commands;
using ViverBem.Domain.Documentos.Repository;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Application.Services
{
    public class DocumentoConfiguracaoAppService : IDocumentoAppService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IDocumentoConfiguracaoRepository _documentoRepository;
        private readonly IUser _user;

        public DocumentoConfiguracaoAppService(IBus bus, IMapper mapper, IDocumentoConfiguracaoRepository documentoRepository, IUser user)
        {
            _bus = bus;
            _mapper = mapper;
            _documentoRepository = documentoRepository;
            _user = user;
        }

        public void AdicionarDocumentoItem(DocumentoConfiguracaoItemViewModel documentoItemViewModel)
        {
            var docItemCommand = _mapper.Map<IncluirDocumentoConfiguracaoItemCommand>(documentoItemViewModel);
            _bus.SendCommand(docItemCommand);
        }

        public void Atualizar(DocumentoConfiguracaoViewModel documentoConfiguracao)
        {
            var atualizarDocumentoCommand = _mapper.Map<AtualizarDocumentoConfiguracaoCommand>(documentoConfiguracao);
            _bus.SendCommand(atualizarDocumentoCommand);
        }

        public void AtualizarDocumentoItem(DocumentoConfiguracaoItemViewModel documentoItemViewModel)
        {
            var atualizarDocumentoItemCommand = _mapper.Map<AtualizarDocumentoConfiguracaoItemCommand>(documentoItemViewModel);
            _bus.SendCommand(atualizarDocumentoItemCommand);
        }

        public void Dispose()
        {
            _documentoRepository.Dispose();
        }

        public void Excluir(Guid id)
        {
            _bus.SendCommand(new ExcluirDocumentoConfiguracaoCommand(id));
        }

        public IEnumerable<DocumentoConfiguracaoItemViewModel> ObterPorDocumento(Guid documentoID)
        {
            return _mapper.Map<IEnumerable<DocumentoConfiguracaoItemViewModel>>(_documentoRepository.ObterDocumentoItemPorDocumento(documentoID));
        }

        public DocumentoConfiguracaoViewModel ObterPorId(Guid id)
        {
            return _mapper.Map<DocumentoConfiguracaoViewModel>(_documentoRepository.ObterPorId(id));
        }

        public IEnumerable<DocumentoConfiguracaoViewModel> ObterTodos()
        {
            return _mapper.Map<IEnumerable<DocumentoConfiguracaoViewModel>>(_documentoRepository.ObterTodos());
        }

        public void Registrar(DocumentoConfiguracaoViewModel documentoConfiguracao)
        {
            var registroCommand = _mapper.Map<RegistrarDocumentoConfiguracaoCommand>(documentoConfiguracao);
            _bus.SendCommand(registroCommand);
        }
    }
}
