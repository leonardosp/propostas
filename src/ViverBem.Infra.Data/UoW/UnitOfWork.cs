using ViveBem.Domain.Core.Commands;
using ViverBem.Domain.Interfaces;
using ViverBem.Infra.Data.Context;

namespace ViverBem.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClientesContext _context;

        public UnitOfWork(ClientesContext context)
        {
            _context = context;
        }
        public CommandResponse Commit()
        {
            var rowsAffected = _context.SaveChanges();
            return new CommandResponse(rowsAffected > 0);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
