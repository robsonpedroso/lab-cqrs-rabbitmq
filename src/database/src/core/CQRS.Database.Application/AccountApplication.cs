using CQRS.Database.Domain.Contracts.Repository;
using CQRS.Database.Domain.Contracts.Services;
using Tools.Utils.Extensions;
using System;
using System.Threading.Tasks;
using DO = CQRS.Database.Domain.Entities;
using DTO = CQRS.Database.Domain.DTO;

namespace CQRS.Database.Application
{
    public class AccountApplication : BaseApplication, IDisposable
    {
        private readonly IAccountInfraService accountInfraService;
        public AccountApplication(IAccountInfraService accountInfraService) : base()
        {
            this.accountInfraService = accountInfraService;
        }

        public async Task<DTO.Account> Save(DTO.Account account)
        {
            account.Valid();
            var entity = new DO.Account(account);

            accountInfraService.Save(entity);

            return new DTO.Account(entity);
        }

        public async Task<DTO.Account> Get(Guid id)
        {
            var result = await accountInfraService.Get(id);

            if (result.IsNull())
                throw new ArgumentException($"Usuário {id} não encontrada.");

            return new DTO.Account(result);
        }

        public async void Delete(Guid id)
        {
            await Get(id);
            accountInfraService.Delete(id);
        }
    }
}
