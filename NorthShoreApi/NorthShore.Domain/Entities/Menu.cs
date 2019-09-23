using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NorthShore.Domain.Entities
{
    public class Menu : BaseEntity<long>
    {
        public string Name { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TotalCalorie { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public IEnumerable<FoodMenuMapping> FoodMappings { get; set; }
    }
}
