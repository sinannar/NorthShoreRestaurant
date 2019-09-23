using NorthShore.Domain.Entities;
using NorthShore.Domain.Repositories;
using NorthShore.EfContext.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Infrastructure.Repositories
{
    public class FoodMenuMappingRepository : Repository<long, FoodMenuMapping>, IFoodMenuMappingRepository
    {
        public FoodMenuMappingRepository(NorthShoreDbContext context) : base(context)
        {
        }
    }
}
