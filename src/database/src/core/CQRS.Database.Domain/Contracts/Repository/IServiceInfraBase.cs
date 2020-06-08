using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Database.Domain.Contracts.Repository
{
    public interface IServiceInfraBase<TEntity> where TEntity : class
    {
        void Delete(TEntity entity);
        void Delete(Guid id);
        Task<TEntity> Get(Guid id);
        void Save(TEntity entity);
        void Update(TEntity entity);
    }
}
