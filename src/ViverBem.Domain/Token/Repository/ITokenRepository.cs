using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Interfaces;

namespace ViverBem.Domain.Token.Repository
{
    public interface ITokenRepository : IRepository<LoginTokenResult>
    {
        IEnumerable<LoginTokenResult> ObterToken(Guid id);
    }
}
