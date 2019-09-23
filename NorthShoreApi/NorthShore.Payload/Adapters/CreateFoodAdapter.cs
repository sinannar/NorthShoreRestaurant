using NorthShore.Domain.Entities;
using NorthShore.Payload.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Payload.Adapters
{
    public class CreateFoodAdapter
    {
        public Food Transform(CreateFoodDto dto)
        {
            return new Food
            {
                Name = dto.Name,
                Calorie = dto.Calorie
            };
        }
    }
}
