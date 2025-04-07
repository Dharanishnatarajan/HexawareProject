using System;
using System.Collections.Generic;

namespace TicketBookingSystem
{
    public class Task5
    {
        public enum EventType { Movie, Sports, Concert }
        public enum MovieGenre { Action, Comedy, Horror, Drama, SciFi }
        public enum ConcertType { Theatrical, Classical, Rock, Recital }

        public class Event
        {
            public string EventName { get; set; }
            public DateTime EventDate { get; set; }
            public TimeSpan EventTime { get; set; }
            public string VenueName { get; set; }
            public int TotalSeats { get; set; }
            public int AvailableSeats { get; set; }
            public decimal TicketPrice { get; set; }
            public EventType Type { get; set; }

            public Event(string eventName, DateTime eventDate, TimeSpan eventTime, string venueName,
                        int totalSeats, decimal ticketPrice, EventType type)
            {
                EventName = eventName;
                EventDate = eventDate;
                EventTime = eventTime;
                VenueName = venueName;
                TotalSeats = totalSeats;
                AvailableSeats = totalSeats;
                TicketPrice = ticketPrice;
                Type = type;
            }

            public virtual void DisplayEventDetails()
            {
                Console.WriteLine($"Event Name: {EventName}");
                Console.WriteLine($"Date: {EventDate.ToShortDateString()}");
                Console.WriteLine($"Time: {EventTime}");
                Console.WriteLine($"Venue: {VenueName}");
                Console.WriteLine($"Available Seats: {AvailableSeats}/{TotalSeats}");
                Console.WriteLine($"Ticket Price: {TicketPrice}");
                Console.WriteLine($"Event Type: {Type}");
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
        }

        public class Movie : Event
        {
            public MovieGenre Genre { get; set; }
            public string ActorName { get; set; }
            public string ActressName { get; set; }

            public Movie(string eventName, DateTime eventDate, TimeSpan eventTime, string venueName,
                         int totalSeats, decimal ticketPrice, MovieGenre genre, string actorName, string actressName)
                : base(eventName, eventDate, eventTime, venueName, totalSeats, ticketPrice, EventType.Movie)
            {
                Genre = genre;
                ActorName = actorName;
                ActressName = actressName;
            }

            public override void DisplayEventDetails()
            {
                base.DisplayEventDetails();
                Console.WriteLine($"Genre: {Genre}");
                Console.WriteLine($"Actor: {ActorName}");
                Console.WriteLine($"Actress: {ActressName}");
            }
        }

        public class Concert : Event
        {
            public string Artist { get; set; }
            public ConcertType ConcertType { get; set; }

            public Concert(string eventName, DateTime eventDate, TimeSpan eventTime, string venueName,
                         int totalSeats, decimal ticketPrice, string artist, ConcertType type)
                : base(eventName, eventDate, eventTime, venueName, totalSeats, ticketPrice, EventType.Concert)
            {
                Artist = artist;
                ConcertType = type;
            }

            public override void DisplayEventDetails()
            {
                base.DisplayEventDetails();
                Console.WriteLine($"Artist: {Artist}");
                Console.WriteLine($"Concert Type: {ConcertType}");
            }
        }

        public class Sports : Event
        {
            public string SportName { get; set; }
            public string TeamsName { get; set; }

            public Sports(string eventName, DateTime eventDate, TimeSpan eventTime, string venueName,
                         int totalSeats, decimal ticketPrice, string sportName, string teamsName)
                : base(eventName, eventDate, eventTime, venueName, totalSeats, ticketPrice, EventType.Sports)
            {
                SportName = sportName;
                TeamsName = teamsName;
            }

            public override void DisplayEventDetails()
            {
                base.DisplayEventDetails();
                Console.WriteLine($"Sport: {SportName}");
                Console.WriteLine($"Teams: {TeamsName}");
            }
        }

        public class TicketBookingSystem
        {
            private List<Event> events = new List<Event>();

            public void Main()
            {
                Console.WriteLine("Welcome to Ticket Booking System");

                events.Add(new Movie("Avengers: Endgame", new DateTime(2023, 12, 15), new TimeSpan(18, 30, 0),
                    "IMAX", 200, 1500, MovieGenre.Action, "Robert Downey Jr.", "Scarlett Johansson"));

                events.Add(new Concert("Flim Festival", new DateTime(2023, 12, 20), new TimeSpan(20, 0, 0),
                    "Nehru Stadium", 5000, 2500, "Aniruth", ConcertType.Rock));

                events.Add(new Sports("World Cup", new DateTime(2023, 12, 25), new TimeSpan(14, 0, 0),
                    "National Stadium", 25000, 700, "Cricket", "India vs Pakistan"));

                while (true)
                {
                    Console.WriteLine("\nMenu:");
                    Console.WriteLine("1. View All Events");
                    Console.WriteLine("2. Book Tickets");
                    Console.WriteLine("3. Cancel Tickets");
                    Console.WriteLine("4. Exit");
                    Console.Write("Enter your choice: ");

                    if (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("\nAvailable Events:");
                            for (int i = 0; i < events.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {events[i].EventName} ({events[i].Type})");
                            }
                            Console.Write("\nEnter event number to view details: ");
                            if (int.TryParse(Console.ReadLine(), out int eventNum) && eventNum > 0 && eventNum <= events.Count)
                            {
                                events[eventNum - 1].DisplayEventDetails();
                            }
                            else
                            {
                                Console.WriteLine("Invalid event selection.");
                            }
                            break;

                        case 2:
                            Console.WriteLine("\nAvailable Events:");
                            for (int i = 0; i < events.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {events[i].EventName} ({events[i].Type})");
                            }
                            Console.Write("\nEnter event number to book tickets: ");
                            if (int.TryParse(Console.ReadLine(), out int bookEventNum) && bookEventNum > 0 && bookEventNum <= events.Count)
                            {
                                Console.Write("Enter number of tickets: ");
                                if (int.TryParse(Console.ReadLine(), out int tickets) && tickets > 0)
                                {
                                    if (events[bookEventNum - 1].BookTickets(tickets))
                                    {
                                        Console.WriteLine($"Booking successful! Total cost: {tickets * events[bookEventNum - 1].TicketPrice}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Not enough seats available.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid number of tickets.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid event selection.");
                            }
                            break;

                        case 3:
                            Console.WriteLine("\nAvailable Events:");
                            for (int i = 0; i < events.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {events[i].EventName} ({events[i].Type})");
                            }
                            Console.Write("\nEnter event number to cancel tickets: ");
                            if (int.TryParse(Console.ReadLine(), out int cancelEventNum) && cancelEventNum > 0 && cancelEventNum <= events.Count)
                            {
                                Console.Write("Enter number of tickets to cancel: ");
                                if (int.TryParse(Console.ReadLine(), out int cancelTickets) && cancelTickets > 0)
                                {
                                    events[cancelEventNum - 1].CancelBooking(cancelTickets);
                                    Console.WriteLine($"{cancelTickets} tickets cancelled successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid number of tickets.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid event selection.");
                            }
                            break;

                        case 4:
                            Console.WriteLine("Thank you for using Ticket Booking System!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            new TicketBookingSystem().Main();
        }
    }
}