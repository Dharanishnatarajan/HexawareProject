﻿using System.ComponentModel.DataAnnotations;

namespace HotPot.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }
    }
}
