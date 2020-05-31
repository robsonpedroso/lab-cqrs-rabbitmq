using CQRS.Core.Domain.Contracts.Repository;
using CQRS.Core.Domain.Contracts.Services;
using CQRS.Tools.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DO = CQRS.Core.Domain.Entities;
using DTO = CQRS.Core.Domain.DTO;

namespace CQRS.Core.Application
{
    public class AccountApplication : BaseApplication, IDisposable
    {
        private readonly IAccountRepository accountRepository;
        private readonly IAccountService accountService;
        public AccountApplication(IAccountRepository accountRepository, IAccountService accountService) : base()
        {
            this.accountRepository = accountRepository;
            this.accountService = accountService;
        }

        public DTO.Account Save(DTO.Account account)
        {
            account.Valid();
            var entity = new DO.Account(account);
            accountService.Save(entity);

            return new DTO.Account(entity);
        }

        public Task Get(Guid id)
        {
            var result = accountRepository.Get(id);

            if (result.IsNull())
                throw new ArgumentException($"Usuário {id} não encontrada.");

            return result;
        }

        public async void Delete(Guid id)
        {
            await Get(id);
            accountRepository.Delete(id);
        }
    }
}
