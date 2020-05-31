using CQRS.Core.Domain.Entities;
using System.Threading.Tasks;

namespace CQRS.Core.Domain.Contracts.Repository
{
    public interface IAccountRepository : IRepository<Entities.Account>
    {
    }
}
