using ConsoleBasedBankingApp;
using System;
using BCrypt.Net;

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
                string fullName = GetValidStringInput("Unesite ime i prezime: ");
                string pin = GetValidStringInput("Unesite PIN: ");

                bank.RegisterUser(fullName, pin);
                fileManager.SaveUsers(bank.GetUsers());
            }
            else if(choice == "2")
            {
                string accountNumber = GetValidStringInput("Unesite broj bankovnog racuna: ");
                string pin = GetValidStringInput("Unesite PIN: ");

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
                decimal amount = GetValidDecimalInput("Unesite iznos za uplatu: ");

                bank.Deposit(user, amount);
                fileManager.SaveUsers(fileManager.GetUsers());
            }
            else if( choice == "3")
            {
                decimal amount = GetValidDecimalInput("Unesite iznos za isplatu: ");

                bank.Withdraw(user, amount);
                fileManager.SaveUsers(fileManager.GetUsers());
            }
            else if( choice == "4")
            {
                Console.WriteLine();
                string receiverAccount = Console.ReadLine();

                decimal amount = GetValidDecimalInput("Unesite iznos za transfer: ");

                bank.Transfer(user, receiverAccount, amount);
                fileManager.SaveUsers(fileManager.GetUsers());
            }
            else if (choice == "5")
            {
                break;
            }
        }
    }

    static string GetValidStringInput(string message)
    {
        string input;
        do
        {
            Console.Write(message);
            input = Console.ReadLine().Trim();
        } while(string.IsNullOrEmpty(input));

        return input;
    }

    static decimal GetValidDecimalInput(string message)
    {
        decimal result;
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine();

            if(decimal.TryParse(input, out result) && result > 0)
            {
                return result;
            }
            Console.WriteLine("Neispravan unos! Unesite broj veci od 0.");
        }

    }
}