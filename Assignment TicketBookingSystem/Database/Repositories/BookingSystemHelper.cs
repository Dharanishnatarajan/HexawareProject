using System;
using App;
using Microsoft.Data.SqlClient;
using TicketBookingSystem.Models;

namespace TicketBookingSystem.Repositories
{
    public partial class BookingSystemRepository
    {
        
        private int EnsureVenueExists(SqlConnection conn, SqlTransaction transaction, Venue venue)
        {
            const string sql = @"IF EXISTS (SELECT 1 FROM Venues WHERE Name = @Name AND Address = @Address)
                               SELECT VenueId FROM Venues WHERE Name = @Name AND Address = @Address
                               ELSE
                               BEGIN
                                   INSERT INTO Venues (Name, Address, Capacity)
                                   VALUES (@Name, @Address, @Capacity);
                                   SELECT SCOPE_IDENTITY();
                               END";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Name", venue.Name);
                cmd.Parameters.AddWithValue("@Address", venue.Address);
                cmd.Parameters.AddWithValue("@Capacity", venue.Capacity);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int CreateEventRecord(SqlConnection conn, SqlTransaction transaction,
                                    string eventName, DateTime date, TimeSpan time,
                                    int totalSeats, decimal ticketPrice, string eventType, int venueId)
        {
            const string sql = @"INSERT INTO Events (EventName, Date, Time, TotalSeats, AvailableSeats, 
                               TicketPrice, EventType, VenueId) 
                               VALUES (@EventName, @Date, @Time, @TotalSeats, @TotalSeats, 
                               @TicketPrice, @EventType, @VenueId);
                               SELECT SCOPE_IDENTITY();";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@EventName", eventName);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@Time", time);
                cmd.Parameters.AddWithValue("@TotalSeats", totalSeats);
                cmd.Parameters.AddWithValue("@TicketPrice", ticketPrice);
                cmd.Parameters.AddWithValue("@EventType", eventType);
                cmd.Parameters.AddWithValue("@VenueId", venueId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private Event CreateEventInstance(string eventType, int eventId, string eventName)
        {
            return eventType.ToLower() switch
            {
                "movie" => new Movie { EventId = eventId, EventName = eventName },
                "concert" => new Concert { EventId = eventId, EventName = eventName },
                "sports" => new Sports { EventId = eventId, EventName = eventName },
                _ => throw new ArgumentException("Invalid event type")
            };
        }

        private Event MapEventFromReader(SqlDataReader reader)
        {
            string eventType = reader["EventType"].ToString();
            Event @event = eventType.ToLower() switch
            {
                "movie" => new Movie(),
                "concert" => new Concert(),
                "sports" => new Sports(),
                _ => throw new Exception("Unknown event type")
            };

            @event.EventId = Convert.ToInt32(reader["EventId"]);
            @event.EventName = reader["EventName"].ToString();
            @event.Date = Convert.ToDateTime(reader["Date"]);
            @event.Time = TimeSpan.Parse(reader["Time"].ToString());
            @event.TotalSeats = Convert.ToInt32(reader["TotalSeats"]);
            @event.AvailableSeats = Convert.ToInt32(reader["AvailableSeats"]);
            @event.TicketPrice = Convert.ToDecimal(reader["TicketPrice"]);
            @event.EventType = eventType;

            return @event;
        }

        private Customer GetCustomerDetailsFromConsole()
        {
            Console.WriteLine("Enter customer details:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            return new Customer { Name = name, Email = email, Phone = phone };
        }

        private (int EventId, decimal TicketPrice, int AvailableSeats) GetEventInfo(
            SqlConnection conn, SqlTransaction transaction, string eventName)
        {
            const string sql = "SELECT EventId, TicketPrice, AvailableSeats FROM Events WHERE EventName = @EventName";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@EventName", eventName);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new Exception("Event not found");
                    return (
                        Convert.ToInt32(reader["EventId"]),
                        Convert.ToDecimal(reader["TicketPrice"]),
                        Convert.ToInt32(reader["AvailableSeats"])
                    );
                }
            }
        }

        private void ValidateAvailableSeats(int availableSeats, int requestedSeats)
        {
            if (requestedSeats > availableSeats)
                throw new Exception($"Not enough seats available. Available: {availableSeats}, Requested: {requestedSeats}");
        }

        private void UpdateAvailableSeats(SqlConnection conn, SqlTransaction transaction,
                                        int eventId, int seatChange)
        {
            const string sql = "UPDATE Events SET AvailableSeats = AvailableSeats + @SeatChange WHERE EventId = @EventId";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@SeatChange", seatChange);
                cmd.Parameters.AddWithValue("@EventId", eventId);
                cmd.ExecuteNonQuery();
            }
        }

        private Booking CreateBooking(SqlConnection conn, SqlTransaction transaction,
                                    (int EventId, decimal TicketPrice, int AvailableSeats) eventInfo,
                                    int numTickets, Customer customer)
        {
            int bookingId = CreateBookingRecord(conn, transaction, eventInfo.EventId, numTickets, eventInfo.TicketPrice);
            int customerId = EnsureCustomerExists(conn, transaction, customer);
            LinkBookingToCustomer(conn, transaction, bookingId, customerId);

            return new Booking
            {
                BookingId = bookingId,
                EventId = eventInfo.EventId,
                NumberOfTickets = numTickets,
                TotalCost = numTickets * eventInfo.TicketPrice,
                BookingDate = DateTime.Now
            };
        }

        private int CreateBookingRecord(SqlConnection conn, SqlTransaction transaction,
                                      int eventId, int numTickets, decimal ticketPrice)
        {
            const string sql = @"INSERT INTO Bookings (EventId, NumberOfTickets, TotalCost) 
                               VALUES (@EventId, @NumTickets, @TotalCost);
                               SELECT SCOPE_IDENTITY();";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@EventId", eventId);
                cmd.Parameters.AddWithValue("@NumTickets", numTickets);
                cmd.Parameters.AddWithValue("@TotalCost", numTickets * ticketPrice);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int EnsureCustomerExists(SqlConnection conn, SqlTransaction transaction, Customer customer)
        {
            const string sql = @"IF NOT EXISTS (SELECT 1 FROM Customers WHERE Email = @Email)
                               INSERT INTO Customers (Name, Email, Phone) 
                               VALUES (@Name, @Email, @Phone);
                               SELECT CustomerId FROM Customers WHERE Email = @Email;";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void LinkBookingToCustomer(SqlConnection conn, SqlTransaction transaction,
                                         int bookingId, int customerId)
        {
            const string sql = "INSERT INTO BookingCustomers (BookingId, CustomerId) VALUES (@BookingId, @CustomerId)";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.ExecuteNonQuery();
            }
        }

        private (int EventId, int NumTickets) GetBookingInfo(SqlConnection conn, SqlTransaction transaction, int bookingId)
        {
            const string sql = "SELECT EventId, NumberOfTickets FROM Bookings WHERE BookingId = @BookingId";
            using (var cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        throw new Exception("Booking not found");
                    return (
                        Convert.ToInt32(reader["EventId"]),
                        Convert.ToInt32(reader["NumberOfTickets"])
                    );
                }
            }
        }

        private void RemoveBookingRecords(SqlConnection conn, SqlTransaction transaction, int bookingId)
        {
            const string deleteLinksSql = "DELETE FROM BookingCustomers WHERE BookingId = @BookingId";
            using (var cmd = new SqlCommand(deleteLinksSql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                cmd.ExecuteNonQuery();
            }

            const string deleteBookingSql = "DELETE FROM Bookings WHERE BookingId = @BookingId";
            using (var cmd = new SqlCommand(deleteBookingSql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@BookingId", bookingId);
                cmd.ExecuteNonQuery();
            }
        }

        private Booking MapBookingFromReader(SqlDataReader reader)
        {
            return new Booking
            {
                BookingId = Convert.ToInt32(reader["BookingId"]),
                EventId = Convert.ToInt32(reader["EventId"]),
                NumberOfTickets = Convert.ToInt32(reader["NumberOfTickets"]),
                TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                BookingDate = Convert.ToDateTime(reader["BookingDate"])
            };
        }
    }
}