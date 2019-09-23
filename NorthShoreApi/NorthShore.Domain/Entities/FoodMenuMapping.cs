using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NorthShore.Domain.Entities
{
    public class FoodMenuMapping
    {
        [Key]
        public long Id { get; set; }

        public long FoodId { get; set; }
        public long MenuId { get; set; }
        public Food Food { get; set; }
        public Menu Menu { get; set; }
    }
}
