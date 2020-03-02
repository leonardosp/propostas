using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ViverBem.Infra.CrossCutting.Identity.Models.ManageViewModels;

namespace ViverBem.Infra.CrossCutting.Identity.Models
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _acessor;

        public AspNetUser(IHttpContextAccessor acessor)
        {
            _acessor = acessor;
        }

        public string Name => _acessor.HttpContext.User.Identity.Name;

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _acessor.HttpContext.User.Claims;
        }

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_acessor.HttpContext.User.GetUserId()) : Guid.NewGuid();
        }

        public bool IsAuthenticated()
        {
            return _acessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
