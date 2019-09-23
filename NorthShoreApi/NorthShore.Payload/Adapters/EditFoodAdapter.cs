﻿using NorthShore.Domain.Entities;
using NorthShore.Payload.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Payload.Adapters
{
    public class EditFoodAdapter
    {
        public Food Transform(EditFoodDto dto, Food entity)
        {
            entity.IsGlutenFree = dto.IsGlutenFree;
            entity.IsDairyFree = dto.IsDairyFree;
            entity.IsNutFree = dto.IsNutFree;
            entity.Calorie = dto.Calorie;
            entity.Price = dto.Price;
            return entity;
        }
    }
}
