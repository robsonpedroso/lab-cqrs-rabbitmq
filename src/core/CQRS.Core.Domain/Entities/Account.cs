using CQRS.Tools.Utils.Components;
using CQRS.Tools.Utils.Extensions;
using System;

namespace CQRS.Core.Domain.Entities
{
    public class Account : BaseModel
    {
        [JsonDbIgnore]
        public virtual string Name { get; set; }

        [JsonDbIgnore]
        public virtual string Email { get; set; }

        [JsonDbIgnore]
        public virtual string Login { get; set; }

        [JsonDbIgnore]
        public virtual TypeAccount Type { get; set; }

        [JsonDbIgnore]
        public virtual string Password { get; set; }

        [JsonDbIgnore]
        public virtual string Document { get; set; }

        [JsonDbIgnore]
        public virtual string Phone { get; set; }

        public Account()
        {

        }
        public Account(DTO.Account account)
        {
            this.Id = account.Id ?? Guid.NewGuid();
            this.Name = account.Name;
            this.Password = account.Password;
            this.Email = account.Email;
            this.Login = account.Login;
            this.Document = account.Document;
            this.Phone = account.Phone;
            this.Type = account.Type;
        }
    }
}
