using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Domain.Contracts.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entity);
        void Delete(Guid id);
        Task Get(Guid id);
        void Save(TEntity entity);
        void Update(TEntity entity);
    }
}
