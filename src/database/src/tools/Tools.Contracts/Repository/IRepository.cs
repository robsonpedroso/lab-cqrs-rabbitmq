using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Contracts.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Add(TEntity entity);
        Task Delete(TEntity entity);
        Task Delete(Guid id);
        Task<TEntity> Get(Guid id);
        Task<TEntity> Save(TEntity entity);
        Task Update(TEntity entity);
    }
}
