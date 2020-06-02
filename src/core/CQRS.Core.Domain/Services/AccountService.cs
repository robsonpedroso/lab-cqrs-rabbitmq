using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CQRS.Core.Domain.Contracts.Repository;
using CQRS.Core.Domain.Contracts.Services;
using CQRS.Core.Domain.Entities;
using CQRS.Tools.Utils.Extensions;

namespace CQRS.Core.Domain.Services
{
    public class AccountService: IAccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository) : base()
            => this.accountRepository = accountRepository;
        public async void Save(Account account)
        {
            if (!Enum.IsDefined(typeof(TypeAccount), account.Type) || account.Type == TypeAccount.None)
                account.Type = TypeAccount.Client;

            accountRepository.Save(account);
        }
    }
}
