using System;
using System.Threading.Tasks;
using Products.Domain.Common;
using X.PagedList;

namespace Products.Domain.Aggregates.ProductAggregate
{
    public interface IProductRepository : IRepository<Product>
    {
        /// Was having a dilemma whether to return IQueryable and do paging in service layer
        /// Ultimately went with this because returning IQueryable seemed like a leaky abstraction
        /// and would allow complex queries to be run outside of repository
        Task<IPagedList<Product>> GetPagedAsync(int pageNumber, int pageSize);
        Task<Product?> FindByIdAsync(Guid productId);
        Task<Product> AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
