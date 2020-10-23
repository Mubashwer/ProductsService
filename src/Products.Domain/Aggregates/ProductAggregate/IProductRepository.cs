using System;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Domain.Aggregates.ProductAggregate
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();
        Task<Product?> FindAsync(Guid productId);
        Task<Product> AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
