using System;


namespace TicketBookingSystem
{
    public class Task2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available Ticket Types:");
            Console.WriteLine("1. Silver");
            Console.WriteLine("2. Gold");
            Console.WriteLine("3. Diamond");

            Console.Write("\nEnter ticket type (Silver/Gold/Diamond): ");
            string ticketType = Console.ReadLine().ToLower();

            Console.Write("Enter number of tickets to book: ");
            int noOfTickets = Convert.ToInt32(Console.ReadLine());

            double pricePerTicket = 0;

            if (ticketType == "silver")
            {
                pricePerTicket = 100;
            }
            else if (ticketType == "gold")
            {
                pricePerTicket = 200;
            }
            else if (ticketType == "diamond")
            {
                pricePerTicket = 300;
            }
            else
            {
                Console.WriteLine("Invalid ticket type selected.");
                return;
            }

            double totalCost = pricePerTicket * noOfTickets;

            Console.WriteLine($"\n Booking successful!");
            Console.WriteLine($"Ticket Type: {ticketType.ToUpper()}");
            Console.WriteLine($"Number of Tickets: {noOfTickets}");
            Console.WriteLine($"Price per Ticket: {pricePerTicket}");
            Console.WriteLine($"Total Cost: {totalCost}");

            Console.ReadLine();
        }
    }
}
