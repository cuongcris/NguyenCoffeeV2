using System;
using System.Collections.Generic;

namespace NguyenCoffeeWeb.Models
{
    public partial class OrdersOnline
    {
        public OrdersOnline()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? PackagingDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? Status { get; set; }

        public virtual Account? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
