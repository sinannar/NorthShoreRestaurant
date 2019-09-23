using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NorthShore.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> Commit();
    }
}
