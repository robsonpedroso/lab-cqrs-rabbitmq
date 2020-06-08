using Tools.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using CQRS.Database.Domain.Entities;

namespace CQRS.Database.Domain.Contracts.Repository
{
    public interface IAccountInfraService : IServiceInfraBase<Account>
    {

    }
}
