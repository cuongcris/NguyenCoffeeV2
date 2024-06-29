using System;
using System.Collections.Generic;

namespace NguyenCoffeeWeb.Models
{
    public partial class OrderInTable
    {
        public DateTime CreatedAt { get; set; }
        public Guid? UserId { get; set; }
        public short? TableNumber { get; set; }
        public string? ImageAi { get; set; }
        public string? UserName { get; set; }
        public Guid Id { get; set; }
        public bool IsPay { get; set; }
    }
}
