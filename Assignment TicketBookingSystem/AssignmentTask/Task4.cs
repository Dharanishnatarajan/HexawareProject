using System;
using System.Collections.Generic;

namespace TicketBookingSystem
{
    public class Task4
    {
        public enum EventType
        {
            Movie,
            Sports,
            Concert
        }

        public class Event
        {
            // Attributes
            public string EventName { get; set; }
            public DateTime EventDate { get; set; }
            public TimeSpan EventTime { get; set; }
            public string VenueName { get; set; }
            public int TotalSeats { get; set; }
            public int AvailableSeats { get; set; }
            public decimal TicketPrice { get; set; }
            public EventType EventType { get; set; }

            // Constructors
            public Event() { }

            public Event(string eventName, DateTime eventDate, TimeSpan eventTime, string venueName,
                        int totalSeats, decimal ticketPrice, EventType eventType)
            {
                EventName = eventName;
                EventDate = eventDate;
                EventTime = eventTime;
                VenueName = venueName;
                TotalSeats = totalSeats;
                AvailableSeats = totalSeats; 
                TicketPrice = ticketPrice;
                EventType = eventType;
            }

            // Methods
            public decimal CalculateTotalRevenue()
            {
                return (TotalSeats - AvailableSeats) * TicketPrice;
            }

            public int GetBookedNoOfTickets()
            {
                return TotalSeats - AvailableSeats;
            }

            public bool BookTickets(int numTickets)
            {
                if (numTickets <= AvailableSeats)
                {
                    AvailableSeats -= numTickets;
                    return true;
                }
                return false;
            }

            public void CancelBooking(int numTickets)
            {
                AvailableSeats = Math.Min(AvailableSeats + numTickets, TotalSeats);
            }

            public void DisplayEventDetails()
            {
                Console.WriteLine($"Event Name: {EventName}");
                Console.WriteLine($"Date: {EventDate.ToShortDateString()}");
                Console.WriteLine($"Time: {EventTime}");
                Console.WriteLine($"Venue: {VenueName}");
                Console.WriteLine($"Available Seats: {AvailableSeats}/{TotalSeats}");
                Console.WriteLine($"Ticket Price: {TicketPrice}");
                Console.WriteLine($"Event Type: {EventType}");
            }
        }

        public class Venue
        {
            public string VenueName { get; set; }
            public string Address { get; set; }

            public Venue() { }

            public Venue(string venueName, string address)
            {
                VenueName = venueName;
                Address = address;
            }

            public void DisplayVenueDetails()
            {
                Console.WriteLine($"Venue Name: {VenueName}");
                Console.WriteLine($"Address: {Address}");
            }
        }

        public class Customer
        {
            public string CustomerName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }


            public Customer(string customerName, string email, string phoneNumber)
            {
                CustomerName = customerName;
                Email = email;
                PhoneNumber = phoneNumber;
            }

            public void DisplayCustomerDetails()
            {
                Console.WriteLine($"Customer Name: {CustomerName}");
                Console.WriteLine($"Email: {Email}");
                Console.WriteLine($"Phone: {PhoneNumber}");
            }
        }

        public class Booking
        {
            private Event _event;

            public Booking(Event eventObj)
            {
                _event = eventObj;
            }

            public decimal CalculateBookingCost(int numTickets)
            {
                return numTickets * _event.TicketPrice;
            }

            public bool BookTickets(int numTickets)
            {
                return _event.BookTickets(numTickets);
            }

            public void CancelBooking(int numTickets)
            {
                _event.CancelBooking(numTickets);
            }

            public int GetAvailableNoOfTickets()
            {
                return _event.AvailableSeats;
            }

            public void GetEventDetails()
            {
                _event.DisplayEventDetails();
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                // Create an event
                Event concert = new Event("Aniruth Concert", new DateTime(2023, 8, 15),
                                         new TimeSpan(19, 0, 0), "Nehru Stadium", 1000, 500, EventType.Concert);

                // Create a booking system for the event
                Booking bookingSystem = new Booking(concert);

                // Display event details
                bookingSystem.GetEventDetails();
                Console.WriteLine();

                // Book tickets
                int ticketsToBook = 3;
                if (bookingSystem.BookTickets(ticketsToBook))
                {
                    decimal cost = bookingSystem.CalculateBookingCost(ticketsToBook);
                    Console.WriteLine($"Successfully booked {ticketsToBook} tickets. Total cost: {cost}");
                }
                else
                {
                    Console.WriteLine("Failed to book tickets. Not enough available seats.");
                }

                Console.WriteLine();
                bookingSystem.GetEventDetails();
                Console.WriteLine();

                // Cancel some tickets
                int ticketsToCancel = 1;
                bookingSystem.CancelBooking(ticketsToCancel);
                Console.WriteLine($"Cancelled {ticketsToCancel} ticket.");

                Console.WriteLine();
                bookingSystem.GetEventDetails();
                Console.WriteLine();

                // Display total revenue
                Console.WriteLine($"Total revenue so far: {concert.CalculateTotalRevenue()}");
                Console.ReadKey();
            }
        }
    
    }
}
