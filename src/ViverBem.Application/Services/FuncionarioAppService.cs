using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Bus;
using ViverBem.Application.Interfaces;
using ViverBem.Application.ViewModels;
using ViverBem.Domain.Funcionarios.Commands;
using ViverBem.Domain.Funcionarios.Repository;

namespace ViverBem.Application.Services
{
    public class FuncionarioAppService : IFuncionarioAppService
    {
        private readonly IMapper _mapper;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IBus _bus;

        public FuncionarioAppService(IMapper mapper, IFuncionarioRepository funcionarioRepository, IBus bus)
        {
            _mapper = mapper;
            _funcionarioRepository = funcionarioRepository;
            _bus = bus;
        }
        public void Dispose()
        {
            _funcionarioRepository.Dispose();
        }

        public void Registrar(FuncionarioViewModel funcionarioViewModel)
        {
            var registroCommand = _mapper.Map<RegistrarFuncionarioCommand>(funcionarioViewModel);
            _bus.SendCommand(registroCommand);
        }
    }
}
