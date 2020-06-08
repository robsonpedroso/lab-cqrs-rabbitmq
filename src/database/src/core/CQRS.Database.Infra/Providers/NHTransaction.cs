using Tools.Contracts.Repository;
using NHibernate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Database.Infra.Providers
{
    public class NHTransaction : IUnitOfWorkTransaction
    {
        private readonly ISession session;

        private readonly ITransaction transaction;

        public bool IsConnectionOpen { get { return session != null && session.IsOpen; } }

        public NHTransaction(ISession session)
        {
            this.session = session;
            transaction = session.BeginTransaction();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await transaction.RollbackAsync(cancellationToken);
            }
            catch
            {
                throw;
            }
            finally
            {
                session.Clear();
                transaction.Dispose();
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
