using System;
using System.Collections.Generic;
using System.Text;
using DO = CQRS.Database.Domain.Entities;


namespace CQRS.Database.Infra.Repository.Mappings
{
    public class AccountMap : BaseModelMap<DO.Account>
    {
        public AccountMap() : base("account")
        {
            Map(x => x.Login).Column("login").Nullable();
            Map(x => x.Name).Column("name").Nullable();
            Map(x => x.Email).Column("email").Nullable();
            Map(x => x.Document).Column("document").Nullable();
            Map(x => x.Phone).Column("phone").Nullable();
            Map(x => x.Type).Column("type").Nullable().CustomType(typeof(Domain.TypeAccount));
            Map(x => x.Password).Column("password").Nullable();
        }
    }
}
