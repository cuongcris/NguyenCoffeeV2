using System;
using System.Collections.Generic;

namespace NguyenCoffeeWeb.Models
{
    public partial class OrderDetail
    {
        public Guid ProductId { get; set; }
        public float? UnitPrice { get; set; }
        public int? Quanlity { get; set; }
        public string OrderId { get; set; } = null!;

        public virtual OrdersOnline Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
