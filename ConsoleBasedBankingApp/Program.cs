using ConsoleBasedBankingApp;
using System;
using BCrypt.Net;

class Program
{
        static void Main()
        {
            try
            {
                Bank bank = new Bank();
                FileManager fileManager = new FileManager();
                MenuManager menuManager = new MenuManager(bank, fileManager);

                menuManager.ShowMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Doslo je do greske: {ex.Message}");
                Console.WriteLine("Aplikacija ce biti zatvorena.");
            }
        }
}