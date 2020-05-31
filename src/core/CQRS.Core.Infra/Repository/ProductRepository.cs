using CQRS.Core.Domain.Contracts.Repository;
using CQRS.Core.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CQRS.Core.Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository() { }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
