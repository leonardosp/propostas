using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Events;

namespace ViverBem.Domain.Funcionarios.Events
{
    public class FuncionarioEventHandler : IHandler<FuncionarioRegistradoEvent>
    {
        public void Handle(FuncionarioRegistradoEvent message)
        {
            //
        }
    }
}
