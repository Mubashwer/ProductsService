using System;
using System.Threading;
using System.Threading.Tasks;

namespace Products.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
