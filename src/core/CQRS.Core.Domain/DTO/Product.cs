using System;
using System.Collections.Generic;
using System.Linq;
using DO = CQRS.Core.Domain.Entities;

namespace CQRS.Core.Domain.DTO
{
    public class Product
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public IList<ProductItem> Items { get; set; }

        public Product() { }

        public Product(DO.Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Items = product.Items?.Select(x => new ProductItem(x))?.ToList();
        }
    }
}
