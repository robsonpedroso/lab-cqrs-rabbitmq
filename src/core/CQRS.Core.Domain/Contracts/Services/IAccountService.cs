using CQRS.Core.Domain.Entities;
using System.Threading.Tasks;

namespace CQRS.Core.Domain.Contracts.Services
{
    public interface IAccountService
    {
        void Save(Account account);
    }
}
