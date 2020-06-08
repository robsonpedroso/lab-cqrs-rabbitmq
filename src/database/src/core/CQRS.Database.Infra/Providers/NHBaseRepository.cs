using Tools.Contracts.Repository;
using System;
using NHibernate;
using NHibernate.Criterion;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CQRS.Database.Infra.Providers
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

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var added = await Session.SaveAsync(entity);

            return await Session.GetAsync(entity.GetType(), added) as TEntity;
        }

        public virtual async Task Delete(TEntity entity)
        {
            await Session.DeleteAsync(entity);
        }

        public virtual async Task DeleteById(Guid id)
        {
            var entity = await GetById(id);

            await Delete(entity);
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var result = await Session.CreateCriteria<TEntity>()
                .Add(Restrictions.Eq("Id", id))
                .UniqueResultAsync<TEntity>();

            return result;
        }

        public virtual async Task<TEntity> Save(TEntity entity)
        {
            return await Session.MergeAsync(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await Session.UpdateAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> List()
        {
            var results = await Session.CreateCriteria<TEntity>()
                .ListAsync<TEntity>();

            return results;
        }

        public async Task Delete(Guid id)
        {
            var entity = await Get(id);
            await Session.DeleteAsync(entity);
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await GetById(id);
        }
    }
}
