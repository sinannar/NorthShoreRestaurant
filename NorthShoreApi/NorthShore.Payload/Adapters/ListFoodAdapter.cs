using NorthShore.Domain.Entities;
using NorthShore.Payload.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NorthShore.Payload.Adapters
{
    public class ListFoodAdapter
    {
        public List<ShowFoodDto> Transform(IQueryable<Food> list)
        {
            if (list == null || list.Count() == 0)
                return new List<ShowFoodDto>();
            else
                return list.Select(food => new ShowFoodDto
                {
                    Id = food.Id,
                    Calorie = food.Calorie,
                    Name = food.Name,
                    IsGlutenFree = food.IsGlutenFree,
                    IsDairyFree = food.IsDairyFree,
                    IsNutFree = food.IsNutFree,
                    Price = food.Price,
                }).ToList();
        }
    }
}
