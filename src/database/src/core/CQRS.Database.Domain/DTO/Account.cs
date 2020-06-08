using Tools.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using DO = CQRS.Database.Domain.Entities;

namespace CQRS.Database.Domain.DTO
{
    public class Account
    {
        public virtual Guid? Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Document { get; set; }
        public virtual string Email { get; set; }
        public virtual TypeAccount Type { get; set; }
        public virtual string TypeDescription { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Phone { get; set; }


        public Account()
        {

        }
        public Account(DO.Account account)
        {
            this.Id = account.Id;
            this.Name = account.Name;
            this.Email = account.Email;
            this.Login = account.Login;
            this.Document = account.Document;
            this.Phone = account.Phone;
            this.Type = account.Type;
            this.TypeDescription = account.Type.GetDescription();
        }

        public void Valid()
        {
            if (this.Name.IsNullOrWhiteSpace())
                throw new ArgumentException("Nome não foi preenchido");
        }
    }
}
