using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Contracts.Repository
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
