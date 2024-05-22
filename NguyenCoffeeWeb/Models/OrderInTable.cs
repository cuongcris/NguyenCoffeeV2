using System;
using System.Collections.Generic;

namespace NguyenCoffeeWeb.Models
{
    public partial class OrderInTable
    {
        public OrderInTable()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UserId { get; set; }
        public short? TableNumber { get; set; }
        public string? ImageAi { get; set; }

        public virtual Account? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
