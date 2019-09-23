using NorthShore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Domain.Repositories
{
    public interface IFoodRepository : IRepository<long, Food>
    {
    }
}
