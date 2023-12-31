namespace BazarUi
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class Menu
    {
        private Dictionary<int, string> menuItems;
        private readonly Operations operations;

        public Menu(Operations operations)
        {
            menuItems = new Dictionary<int, string>
            {
                { 1, "Search by topic" },
                { 2, "Info for a certian book" },
                { 3, "Purchase a book" },
                { 4, "Exit" }
            };
            this.operations = operations;
        }

        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                PrintBooks();
                DisplayMenu();
                int choice = GetUserChoice();

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter a topic: ");
                        string topic = Console.ReadLine();
                        operations.Search(topic);
                        break;
                    case 2:
                        Console.Write("Enter an item number: ");
                        int itemNumber = GetIntInput();
                        var watch = new Stopwatch();
                        watch.Start();
                        operations.Info(itemNumber);
                        watch.Stop();
                        var responseTime = watch.ElapsedMilliseconds;
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write("ResponseTime= " + responseTime);
                        Console.ResetColor();
                        Console.WriteLine();
                        break;
                    case 3:
                        Console.Write("Enter an item number for purchase: ");
                        int purchaseItemNumber = GetIntInput();
                        watch = new Stopwatch();
                        watch.Start();
                        operations.Purchase(purchaseItemNumber);
                        watch.Stop();
                        responseTime = watch.ElapsedMilliseconds;
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write("ResponseTime= " + responseTime);
                        Console.ResetColor();
                        Console.WriteLine();
                        break;
                    case 4:
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("Menu:");
            foreach (var menuItem in menuItems)
            {
                Console.WriteLine($"{menuItem.Key}. {menuItem.Value}");
            }
        }

        private void PrintBooks()
        {
            Console.WriteLine(
                "Books:\n"
                    + "1. How to get a good grade in DOS in 40 minutes a day.\r\n2. RPCs for Noobs.\r\n3. Xen and the Art of Surviving Undergraduate School.\r\n4. Cooking for the Impatient Undergrad."
            );
        }

        private int GetUserChoice()
        {
            Console.Write("Select an option: ");
            return GetIntInput();
        }

        private int GetIntInput()
        {
            int number;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Please enter a valid number: ");
            }
            return number;
        }
    }
}
