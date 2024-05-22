using System;
using System.Collections.Generic;

namespace NguyenCoffeeWeb.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProductName { get; set; }
        public double? UnitPrice { get; set; }
        public string? Image { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? IsOnline { get; set; }
        public string? Description { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
