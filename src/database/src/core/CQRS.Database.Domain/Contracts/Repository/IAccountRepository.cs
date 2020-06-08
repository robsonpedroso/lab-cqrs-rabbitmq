using CQRS.Database.Domain.Entities;
using Tools.Contracts.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRS.Database.Domain.Contracts.Repository
{
    public interface IAccountRepository : IRepository<Entities.Account>
    {
    }
}
