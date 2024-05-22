using System;
using System.Collections.Generic;

namespace NguyenCoffeeWeb.Models
{
    public partial class ImageAi
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UserId { get; set; }
        public string? Image { get; set; }

        public virtual Account? User { get; set; }
    }
}
