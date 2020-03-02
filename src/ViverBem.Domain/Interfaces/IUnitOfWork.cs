using System;
using System.Collections.Generic;
using System.Text;
using ViveBem.Domain.Core.Commands;

namespace ViverBem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        CommandResponse Commit();
    }
}
