using Gestor.Tools.Contracts.Repository;
using System;
using NHibernate;
using NHibernate.Criterion;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Gestor.Tools.Providers
{
    public abstract class NHBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IUnitOfWork _unitOfWork;
        private ISession _session;

        protected ISession Session
        {
            get
            {
                if (_session == null)
                    _session = _unitOfWork.GetSession() as ISession;

                return _session;
            }
        }

        protected NHBaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            var added = await Session.SaveAsync(entity);

            return await Session.GetAsync(entity.GetType(), added) as TEntity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Session.DeleteAsync(entity);
        }

        public virtual async Task DeleteByIdAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);

            await DeleteAsync(entity);
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            var result = await Session.CreateCriteria<TEntity>()
                .Add(Restrictions.Eq("Id", id))
                .UniqueResultAsync<TEntity>();

            return result;
        }

        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            return await Session.MergeAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Session.UpdateAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            var results = await Session.CreateCriteria<TEntity>()
                .ListAsync<TEntity>();

            return results;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            await Session.DeleteAsync(entity);
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await GetByIdAsync(id);
        }
    }
}
