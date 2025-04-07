using System;
using System.Collections.Generic;
using TicketBookingSystem.Models;

namespace TicketBookingSystem.Repositories
{
    public interface IBookingSystemRepository
    {
        Event CreateEvent(string eventName, DateTime date, TimeSpan time, int totalSeats,
                         decimal ticketPrice, string eventType, Venue venue);
        List<Event> GetEventDetails();
        List<Booking> GetAllBookings();
        int GetAvailableNoOfTickets(int eventId);
        decimal CalculateBookingCost(int numTickets, decimal ticketPrice);
        Booking BookTickets(string eventName, int numTickets); 
        void CancelBooking(int bookingId);
        Booking GetBookingDetails(int bookingId);
    }
}