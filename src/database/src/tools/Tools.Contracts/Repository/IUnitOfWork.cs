using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Contracts.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWork Open();
        IUnitOfWorkTransaction BeginTransaction();
        object GetSession();
    }
}
