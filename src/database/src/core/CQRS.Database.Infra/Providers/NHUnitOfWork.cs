using Tools.Contracts.Repository;
using NHibernate;
using System;

namespace CQRS.Database.Infra.Providers
{
    public class NHUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly NHContext context;

        private IUnitOfWorkTransaction transaction;

        private ISession session;

        public NHUnitOfWork(NHContext context)
        {
            this.context = context;
        }

        public IUnitOfWork Open()
        {
            session = context.SessionFactory.OpenSession();

            return this;
        }

        public IUnitOfWorkTransaction BeginTransaction()
        {
            if (session == null || !session.IsOpen)
            {
                Open();
            }
            else
            {
                if (transaction != null)
                    transaction.Dispose();
            }

            transaction = new NHTransaction(session);

            return transaction;
        }

        public object GetSession()
        {
            return session;
        }

        public void Dispose()
        {
            transaction?.Dispose();

            session?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
