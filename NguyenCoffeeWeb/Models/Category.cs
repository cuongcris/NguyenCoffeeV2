using System;
using System.Collections.Generic;

namespace NguyenCoffeeWeb.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
