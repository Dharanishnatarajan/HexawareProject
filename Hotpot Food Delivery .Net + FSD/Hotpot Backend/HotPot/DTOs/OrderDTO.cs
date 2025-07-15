namespace HotPot.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public string Address { get; set; }          
        public string PhoneNumber { get; set; }

        public string Status { get; set; } 

        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int MenuItemId { get; set; }


        public int Quantity { get; set; }

        public MenuItemSummaryDTO MenuItem { get; set; }

    }

    public class MenuItemSummaryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RestaurantSummaryDTO Restaurant { get; set; }
    }

    public class RestaurantSummaryDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }



    public class CreateOrderDTO
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }


    public class UpdateOrderStatusDTO
    {
        public string Status { get; set; }
    }


}