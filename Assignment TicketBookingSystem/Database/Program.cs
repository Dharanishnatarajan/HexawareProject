using System;
using System.Collections.Generic;
using TicketBookingSystem.Models;
using TicketBookingSystem.Repositories;
using App;

namespace TicketBookingSystem
{
    class Program
    {
        private static IBookingSystemRepository repository = new BookingSystemRepository();

        static void Main(string[] args)
        {
            Console.WriteLine("Ticket Booking System");
            Console.WriteLine("Available Options:");
            Console.WriteLine("1.Create_event\n2.Book_tickets\n3.Cancel_booking\n4.Available_seats\n5.Event_details\n6.Booking_Details\n7.Exit\n");

            while (true)
            {
                Console.Write("\nEnter Option: ");
                int command = Convert.ToInt32(Console.ReadLine());

                try
                {
                    switch (command)
                    {
                        case 1:
                            CreateEvent();
                            break;
                        case 2:
                            BookTickets();
                            break;
                        case 3:
                            CancelBooking();
                            break;
                        case 4:
                            GetAvailableSeats();
                            break;
                        case 5:
                            GetEventDetails();
                            break;
                        case 6:
                            ViewAllBookings();
                            break;
                        case 7:
                            return;

                        default:
                            Console.WriteLine("Invalid command");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void CreateEvent()
        {

            // Get venue details first
            Console.WriteLine("Enter venue details:");
            Console.Write("Venue Name: ");
            string venueName = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();
            Console.Write("Capacity: ");
            int capacity = int.Parse(Console.ReadLine());

            Venue venue = new Venue
            {
                Name = venueName,
                Address = address,
                Capacity = capacity
            };

            Console.WriteLine("--------------------------");
            Console.WriteLine("Enter event details:");

            // Get event details
            Console.Write("Event Name: ");
            string name = Console.ReadLine();
            Console.Write("Date (yyyy-MM-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine());
            Console.Write("Time (hh:mm): ");
            TimeSpan time = TimeSpan.Parse(Console.ReadLine());
            Console.Write("Total Seats: ");
            int totalSeats = int.Parse(Console.ReadLine());
            Console.Write("Ticket Price: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Event Type (movie/concert/sports): ");
            string type = Console.ReadLine();

            try
            {
                Event @event = repository.CreateEvent(name, date, time, totalSeats, price, type, venue);
                Console.WriteLine($"Event created with ID: {@event.EventId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating event: {ex.Message}");
            }
        }
        static void ViewAllBookings()
        {
            List<Booking> bookings = repository.GetAllBookings();
            Console.WriteLine("\nBooking Details:");
            Console.WriteLine("Customer ID\tEvent ID\tTickets\t\tCost");
            foreach (var booking in bookings)
            {
                Console.WriteLine($"{booking.BookingId}\t\t{booking.EventId}\t\t{booking.NumberOfTickets}\t\t{booking.TotalCost}");
            }
        }


        static void BookTickets()
        {
            Console.Write("Enter Event Name: ");
            string eventName = Console.ReadLine();

            Console.Write("Number of Tickets: ");
            int numTickets = int.Parse(Console.ReadLine());

            // Repository now handles customer input internally
            Booking booking = repository.BookTickets(eventName, numTickets);
            Console.WriteLine($"Booking successful. Booking ID: {booking.BookingId}, Total Cost: {booking.TotalCost}");
        }

        static void CancelBooking()
        {
            Console.Write("Enter Booking ID to cancel: ");
            int bookingId = int.Parse(Console.ReadLine());

            repository.CancelBooking(bookingId);
            Console.WriteLine("Booking cancelled successfully");
        }

        static void GetAvailableSeats()
        {
            Console.Write("Enter Event ID: ");
            int eventId = int.Parse(Console.ReadLine());

            int availableSeats = repository.GetAvailableNoOfTickets(eventId);
            Console.WriteLine($"Available seats: {availableSeats}");

        }

        static void GetEventDetails()
        {
            List<Event> events = repository.GetEventDetails();
            Console.WriteLine("\nEvent Details:");
            Console.WriteLine("ID\tName\t\t\tDate\t\tType\t\tAvailable\tPrice");

            foreach (var @event in events)
            {
                Console.WriteLine($"{@event.EventId}\t{@event.EventName}\t\t{@event.Date:dd-MM-yyyy}\t{@event.EventType}\t\t{@event.AvailableSeats}\t\t{@event.TicketPrice}");
            }
        }

    }
}