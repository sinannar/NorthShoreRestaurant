using NorthShore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Domain.Repositories
{
    public interface IMenuRepository : IRepository<long, Menu>
    {
    }
}
