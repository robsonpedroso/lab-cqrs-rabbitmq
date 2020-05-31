using CQRS.Tools.Utils.Components;
using System;

namespace CQRS.Core.Domain.Entities
{
    public abstract class BaseModel
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
