namespace TicketBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int EventId { get; set; }
        public int CustomerId { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
