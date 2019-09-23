using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NorthShore.Domain.Entities
{
    public class Food : BaseEntity<long>
    {
        public int Calorie { get; set; }
        public string Name { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsDairyFree { get; set; }
        public bool IsNutFree { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<FoodMenuMapping> MenuMappings { get; set; }
    }
}
