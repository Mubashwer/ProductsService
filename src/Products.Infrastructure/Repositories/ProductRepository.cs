using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Aggregates.ProductAggregate;
using Products.Domain.Common;
using X.PagedList;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsContext _context;

        public ProductRepository(ProductsContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IPagedList<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .AsNoTracking()
                .AsQueryable()
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Product> AddAsync(Product product)
        {
            return (await _context.Products.AddAsync(product)).Entity;
        }

        public async Task<Product?> FindByIdAsync(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product).State = EntityState.Modified;
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }
    }
}