
namespace TicketBookingSystem.Models
{
    public abstract class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public Venue Venue { get; set; }
        public string EventType { get; set; }
    }
}