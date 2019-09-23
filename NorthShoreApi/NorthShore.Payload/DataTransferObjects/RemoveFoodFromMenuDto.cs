using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Payload.DataTransferObjects
{
    public class RemoveFoodFromMenuDto
    {
        public long MenuId { get; set; }
        public long FoodId { get; set; }
    }
}
