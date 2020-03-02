using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ViveBem.Domain.Core.Models;

namespace ViverBem.Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        void Adicionar(TEntity obj);
        TEntity ObterPorId(Guid id);
        IEnumerable<TEntity> ObterTodos();
        void Atualizar(TEntity obj);
        void Remover(Guid id);
        IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate);
        int SaveChanges();
    }
}
