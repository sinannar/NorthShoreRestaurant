using NorthShore.Domain.Repositories;
using NorthShore.EfContext.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NorthShore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private NorthShoreDbContext _context { get; set; }
        private bool disposed = false;

        public UnitOfWork(NorthShoreDbContext context)
        {
            _context = context;
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
