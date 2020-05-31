using CQRS.Core.Domain.Contracts.Repository;
using CQRS.Tools.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DO = CQRS.Core.Domain.Entities;
using DTO = CQRS.Core.Domain.DTO;

namespace CQRS.Core.Application
{
    public class ProductApplication : BaseApplication, IDisposable
    {
        private readonly IProductRepository productRepository;

        public ProductApplication(IProductRepository productRepository) : base()
            => this.productRepository = productRepository;

        public DTO.Product Save(DTO.Product product)
        {
            var entity = new DO.Product(product);
            productRepository.Save(entity);

            return new DTO.Product(entity);
        }

        public Task Get(Guid id)
        {
            var result = productRepository.Get(id);

            if (result.IsNull())
                throw new ArgumentException($"Produto {id} não encontrado.");

            return result;
        }

        public async void Delete(Guid id)
        {
            await Get(id);
            productRepository.Delete(id);
        }
    }
}
