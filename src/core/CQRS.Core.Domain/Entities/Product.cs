using CQRS.Tools.Utils.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Core.Domain.Entities
{
    public class Product : BaseModel
    {
        [JsonDbIgnore]
        public virtual string Name { get; set; }

        [JsonDbIgnore]
        public virtual string Description { get; set; }

        public virtual IList<VO.ProductItem> Items { get; set; }

        public Product() { }

        public Product(DTO.Product product)
        {
            Id = product.Id.HasValue ? product.Id.Value : Guid.NewGuid();
            Name = product.Name;
            Description = product.Description;
            Items = product.Items?.Select(x => new VO.ProductItem(x))?.ToList();
        }
    }
}
