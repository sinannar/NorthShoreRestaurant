using NorthShore.Domain.Entities;
using NorthShore.Payload.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Payload.Adapters
{
    public class CreateMenuAdapter
    {
        public Menu Transform(CreateMenuDto dto)
        {
            return new Menu
            {
                Name = dto.Name,
                DiscountRate = dto.DiscountRate
            };
        }
    }
}
