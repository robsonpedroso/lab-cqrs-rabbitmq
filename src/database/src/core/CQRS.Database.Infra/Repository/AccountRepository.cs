using CQRS.Database.Domain.Contracts.Repository;
using CQRS.Database.Domain.Entities;
using CQRS.Database.Infra.Providers;
using Tools.Contracts.Repository;
using Tools.Utils.Extensions;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DO = CQRS.Database.Domain.Entities;
using DTO = CQRS.Database.Domain.DTO;

namespace CQRS.Database.Infra.Repository
{
    public class AccountRepository : NHBaseRepository<DO.Account>, IAccountRepository
    {
        public AccountRepository(IUnitOfWork uow) : base(uow) { }
    }
}