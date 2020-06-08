using FluentNHibernate.Mapping;
using DO = CQRS.Database.Domain.Entities;

namespace CQRS.Database.Infra.Repository.Mappings
{
    public abstract class BaseModelMap<TEntity> : ClassMap<TEntity> where TEntity : DO.BaseModel
    {
        public BaseModelMap(string table)
        {
            Table(table);
            Id(x => x.Id).Column("id").GeneratedBy.Assigned();
            Map(x => x.Object).Column("object_json").CustomType("StringClob").CustomSqlType("text");
            Map(x => x.Removed).Column("removed").Not.Nullable();
            Map(x => x.CreatedAt).Column("created_at").Not.Update();
            Map(x => x.UpdatedAt).Column("updated_at");
        }
    }
}
