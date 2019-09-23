using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Payload.DataTransferObjects
{
    public class AddFoodToMenuDto
    {
        public long MenuId { get; set; }
        public List<long> FoodIds { get; set; }
    }
}
