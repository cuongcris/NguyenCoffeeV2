﻿namespace NguyenCoffeeWeb.Models
{
    public class CardItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
    }
}
