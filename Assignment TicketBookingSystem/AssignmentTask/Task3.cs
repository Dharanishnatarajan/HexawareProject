using System;


namespace TicketBookingSystem
{
    public class Task3
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Available Ticket Types:");
                Console.WriteLine("1. Silver");
                Console.WriteLine("2. Gold");
                Console.WriteLine("3. Diamond");
                Console.WriteLine("4. Exit");

                Console.Write("Enter ticket type (Silver/Gold/Diamond/Exit): ");
                string ticketType = Console.ReadLine().ToLower();

                if (ticketType == "exit")
                {
                    Console.WriteLine("Thank you!");
                    break;
                }

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
                    Console.WriteLine("Invalid ticket type..");
                    continue;
                }

                double totalCost = pricePerTicket * noOfTickets;
                Console.WriteLine($"\n Booking Confirmed!");
                Console.WriteLine($"Ticket Type: {ticketType.ToUpper()}");
                Console.WriteLine($"No. of Tickets: {noOfTickets}");
                Console.WriteLine($"Total Cost: {totalCost}");
            }

            Console.ReadLine();
        }

    }

}


