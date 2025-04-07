using System;

namespace TicketBookingSystem
{
    public class Task1
    {
        static void Main(string[] args)
        {
            
            Console.Write("Enter number of available tickets: ");
            int availableTickets = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter number of tickets to book: ");
            int noOfBookingTickets = Convert.ToInt32(Console.ReadLine());

            if (availableTickets >= noOfBookingTickets)
            {
                int remainingTickets = availableTickets - noOfBookingTickets;
                Console.WriteLine($"Booking successful!  Balance tickets: {remainingTickets}");
            }
            else
            {
                Console.WriteLine("Booking failed! Tickets unavailable.");
            }
            Console.ReadKey();
        }
    }
}
