using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Application.ViewModels;

namespace ViverBem.Application.Interfaces
{
    public interface IFuncionarioAppService : IDisposable
    {
        void Registrar(FuncionarioViewModel funcionarioViewModel);

    }
}
