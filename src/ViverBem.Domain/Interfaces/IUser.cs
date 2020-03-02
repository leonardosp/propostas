using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ViverBem.Domain.Interfaces
{
    public interface IUser
    {
        string Name { get; }
        Guid GetUserId();
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
