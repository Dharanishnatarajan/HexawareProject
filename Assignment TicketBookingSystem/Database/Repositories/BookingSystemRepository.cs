using System;
using System.Collections.Generic;
using App;
using Microsoft.Data.SqlClient;
using TicketBookingSystem.Models;

namespace TicketBookingSystem.Repositories
{
    public partial class BookingSystemRepository : IBookingSystemRepository
    {
        public Event CreateEvent(string eventName, DateTime date, TimeSpan time, int totalSeats,
                               decimal ticketPrice, string eventType, Venue venue)
        {
            using (var conn = DBUtil.GetDBConn())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    int venueId = EnsureVenueExists(conn, transaction, venue);
                    int eventId = CreateEventRecord(conn, transaction, eventName, date, time,
                                                  totalSeats, ticketPrice, eventType, venueId);
                    transaction.Commit();
                    return CreateEventInstance(eventType, eventId, eventName);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to create event: " + ex.Message);
                }
            }
        }

        public List<Event> GetEventDetails()
        {
            var events = new List<Event>();
            using (var conn = DBUtil.GetDBConn())
            using (var cmd = new SqlCommand("SELECT * FROM Events", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    events.Add(MapEventFromReader(reader));
                }
            }
            return events;
        }

        public int GetAvailableNoOfTickets(int eventId)
        {
            using (var conn = DBUtil.GetDBConn())
            using (var cmd = new SqlCommand("SELECT AvailableSeats FROM Events WHERE EventId = @EventId", conn))
            {
                cmd.Parameters.AddWithValue("@EventId", eventId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public decimal CalculateBookingCost(int numTickets, decimal ticketPrice)
        {
            return numTickets * ticketPrice;
        }

        public Booking BookTickets(string eventName, int numTickets)
        {
            var customer = GetCustomerDetailsFromConsole();
            using (var conn = DBUtil.GetDBConn())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var eventInfo = GetEventInfo(conn, transaction, eventName);
                    ValidateAvailableSeats(eventInfo.AvailableSeats, numTickets);
                    UpdateAvailableSeats(conn, transaction, eventInfo.EventId, numTickets);
                    var booking = CreateBooking(conn, transaction, eventInfo, numTickets, customer);
                    transaction.Commit();
                    return booking;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void CancelBooking(int bookingId)
        {
            using (var conn = DBUtil.GetDBConn())
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var bookingInfo = GetBookingInfo(conn, transaction, bookingId);
                    UpdateAvailableSeats(conn, transaction, bookingInfo.EventId, -bookingInfo.NumTickets);
                    RemoveBookingRecords(conn, transaction, bookingId);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Booking GetBookingDetails(int bookingId)
        {
            using (var conn = DBUtil.GetDBConn())
            using (var cmd = new SqlCommand(
                @"SELECT b.*, e.EventName, e.TicketPrice 
                FROM Bookings b
                JOIN Events e ON b.EventId = e.EventId
                WHERE b.BookingId = @BookingId", conn))
            {
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? MapBookingFromReader(reader) : null;
                }
            }
        }

        public List<Booking> GetAllBookings()
        {
            var bookings = new List<Booking>();
            using (var conn = DBUtil.GetDBConn())
            using (var cmd = new SqlCommand("SELECT * FROM Bookings", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    bookings.Add(MapBookingFromReader(reader));
                }
            }
            return bookings;
        }

    }
}