using Tools.Contracts.Repository;
using Tools.Utils.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Database.Domain.Entities
{
    public abstract class BaseModel : JsonDbObject, IEntity
    {
        [JsonDbIgnore]
        public virtual Guid Id { get; set; }

        [JsonDbIgnore]
        public virtual bool Status { get; set; }

        [JsonDbIgnore]
        public virtual bool Removed { get; set; }

        [JsonDbIgnore]
        public virtual DateTime CreatedAt { get; set; }

        [JsonDbIgnore]
        public virtual DateTime UpdatedAt { get; set; }

        public BaseModel() : base()
        {
            Id = Guid.NewGuid();
            Status = true;
            Removed = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
