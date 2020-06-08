using FluentMigrator;

namespace CQRS.Database.Infra.Repository.Migrations.Gestor
{
    [Migration(201909042140), Tags("CQRS")]
    public class CreateTableAccount : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("account")
                .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("name").AsString(50).Nullable()
                .WithColumn("login").AsString(50).Nullable()
                .WithColumn("password").AsString(50).Nullable()
                .WithColumn("email").AsString(100).Nullable()
                .WithColumn("phone").AsString(20).Nullable()
                .WithColumn("document").AsString(20).Nullable()
                .WithColumn("type").AsString(2).Nullable()
                .WithColumn("object_json").AsCustom("varchar(max)").Nullable()
                .WithColumn("removed").AsInt32().Nullable()
                .WithColumn("created_at").AsDateTime().Nullable()
                .WithColumn("updated_at").AsDateTime().Nullable();
        }
    }
}
