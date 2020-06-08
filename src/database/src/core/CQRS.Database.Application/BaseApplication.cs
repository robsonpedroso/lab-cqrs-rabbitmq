using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Database.Application
{
    public class BaseApplication : IDisposable
    {
        public void Dispose() => GC.SuppressFinalize(this);
    }
}
