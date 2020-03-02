using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ViverBem.Domain.Token;
using ViverBem.Domain.Token.Repository;
using ViverBem.Infra.Data.Context;

namespace ViverBem.Infra.Data.Repository
{
    public class TokenRepository : Repository<LoginTokenResult>, ITokenRepository
    {
        public TokenRepository(ClientesContext context) : base(context)
        {

        }

        public IEnumerable<LoginTokenResult> ObterToken(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<LoginTokenResult> ObterTodos()
        {
            var sql = "SELECT * FROM TOKENS C ";

            return Db.Database.GetDbConnection().Query<LoginTokenResult>(sql);
        }
    }
}
