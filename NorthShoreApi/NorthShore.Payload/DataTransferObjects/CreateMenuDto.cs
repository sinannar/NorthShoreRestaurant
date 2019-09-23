using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShore.Payload.DataTransferObjects
{
    public class CreateMenuDto
    {
        public string Name { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
