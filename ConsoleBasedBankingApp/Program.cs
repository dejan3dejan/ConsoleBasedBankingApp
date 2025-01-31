using ConsoleBasedBankingApp;
using System;

class Program
{
    static void Main()
    {
        Bank bank = new Bank();
        FileManager fileManager = new FileManager();
        List<User> users = fileManager.LoadUsers();

        while (true)
        {
            Console.WriteLine("1. Registracija");
            Console.WriteLine("2. Prijava");
            Console.WriteLine("3. Izlaz");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Unesite ime i prezime: ");
                string fullName = Console.ReadLine();

                Console.Write("Unesite PIN: ");
                string pin = Console.ReadLine();

                bank.RegisterUser(fullName, pin);
                fileManager.SaveUsers(bank.GetUsers());
            }
            else if(choice == "2")
            {
                Console.Write("Unesite broj bankovnog racuna: ");
                string accountNumber = Console.ReadLine();

                Console.Write("Unesite PIN: ");
                string pin = Console.ReadLine();

                User user = bank.Login(accountNumber, pin);

                if (user != null)
                {
                    Console.WriteLine($"Dobrodosli, {user.FullName}");
                    BankMenu(bank, user, fileManager);
                }
                else
                {
                    Console.WriteLine("Neispravan broj racuna ili PIN.");
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine("Hvala na koriscenju. Dovidjenja!!!");
            }
        }
    }
    static void BankMenu(Bank bank, User user, FileManager fileManager)
    {
        while (true)
        {
            Console.WriteLine("1. Provera stanja");
            Console.WriteLine("2. Uplata");
            Console.WriteLine("3. Isplata");
            Console.WriteLine("4. Transfer");
            Console.WriteLine("5. Izlaz");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine($"Vase stanje na bankovnom racunu iznosi {user.Balance}RSD.");
            }
            else if( choice == "2")
            {
                Console.WriteLine("Unesite iznos za uplatu: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                bank.Deposit(user, amount);
                fileManager.SaveUsers(fileManager.GetUsers());
            }
            else if( choice == "3")
            {
                Console.WriteLine("Unesite iznos za isplatu: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                bank.Withdraw(user, amount);
                fileManager.SaveUsers(fileManager.GetUsers());
            }
            else if( choice == "4")
            {
                Console.WriteLine();
                string receiverAccount = Console.ReadLine();

                Console.WriteLine("Unesite iznos za transfer: ");
                decimal amount = decimal.Parse(Console.ReadLine());

                bank.Transfer(user, receiverAccount, amount);
                fileManager.SaveUsers(fileManager.GetUsers());
            }
            else if (choice == "5")
            {
                break;
            }
        }
    }
}