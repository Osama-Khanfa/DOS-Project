namespace BazarUi
{
    using System;
    using System.Collections.Generic;

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
                        operations.Info(itemNumber);
                        break;
                    case 3:
                        Console.Write("Enter an item number for purchase: ");
                        int purchaseItemNumber = GetIntInput();
                        operations.Purchase(purchaseItemNumber);
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
