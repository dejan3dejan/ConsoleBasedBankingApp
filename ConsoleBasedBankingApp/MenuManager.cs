using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBasedBankingApp
{
    public class MenuManager
    {
        private readonly Bank _bank;
        private readonly FileManager _fileManager;

        public MenuManager(Bank bank, FileManager fileManager)
        {
            _bank = bank;
            _fileManager = fileManager;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Bankarska Aplikacija ===");
                Console.WriteLine("1. Registracija");
                Console.WriteLine("2. Prijava");
                Console.WriteLine("3. Izlaz");

                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        HandleRegistration();
                        break;
                    case "2":
                        HandleLogin();
                        break;
                    case "3":
                        Console.WriteLine("Hvala na koriscenju. Dovidjenja!");
                        _fileManager.SaveUsers(_bank.GetUsers());
                        return;
                    default:
                        Console.WriteLine("Nevazeci izbor. Pokusajte ponovo.");
                        break;
                }
            }
        }

        private void HandleRegistration()
        {
            try
            {
                string fullName = InputValidator.GetValidString("Unesite ime i prezime: ", 3, 50);
                string pin = InputValidator.GetValidPin();

                string accountNumber = _bank.RegisterUser(fullName, pin);
                _fileManager.SaveUsers(_bank.GetUsers());

                Console.WriteLine("\nPritisnite bilo koji taster za povratak na glavni meni...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska pri registraciji: {ex.Message}");
                Console.WriteLine("\nPritisnite bilo koji taster za povratak na glavni meni...");
                Console.ReadKey();
            }
        }

        private void HandleLogin()
        {
            try
            {
                string accountNumber = InputValidator.GetValidAccountNumber("Unesite broj bankovnog racuna: ");
                string pin = InputValidator.GetValidPin();

                User user = _bank.Login(accountNumber, pin);
                if (user != null)
                {
                    Console.WriteLine($"Dobrodosli, {user.FullName}");
                    ShowUserMenu(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska pri prijavi: {ex.Message}");
            }
        }

        private void ShowUserMenu(User user)
        {
            while (true)
            {
                Console.WriteLine("\n=== Korisnicki Meni ===");
                Console.WriteLine("1. Provera stanja");
                Console.WriteLine("2. Uplata");
                Console.WriteLine("3. Isplata");
                Console.WriteLine("4. Transfer");
                Console.WriteLine("5. Istorija transakcija");
                Console.WriteLine("6. Izlaz");

                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        ShowBalance(user);
                        break;
                    case "2":
                        HandleDeposit(user);
                        break;
                    case "3":
                        HandleWithdrawal(user);
                        break;
                    case "4":
                        HandleTransfer(user);
                        break;
                    case "5":
                        _bank.ShowTransactionHistory(user);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Nevazeci izbor. Pokusajte ponovo.");
                        break;
                }
            }
        }

        private void ShowBalance(User user)
        {
            Console.WriteLine($"Vase stanje na bankovnom racunu iznosi {user.Balance:N2}RSD.");
        }

        private void HandleDeposit(User user)
        {
            try
            {
                decimal amount = InputValidator.GetValidAmount("Unesite iznos za uplatu: ");
                _bank.Deposit(user, amount);
                _fileManager.SaveUsers(_bank.GetUsers());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska pri uplati: {ex.Message}");
            }
        }

        private void HandleWithdrawal(User user)
        {
            try
            {
                decimal amount = InputValidator.GetValidAmount("Unesite iznos za isplatu: ");
                _bank.Withdraw(user, amount);
                _fileManager.SaveUsers(_bank.GetUsers());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska pri isplati: {ex.Message}");
            }
        }

        private void HandleTransfer(User user)
        {
            try
            {
                string receiverAccount = InputValidator.GetValidAccountNumber("Unesite racun primaoca: ");
                decimal amount = InputValidator.GetValidAmount("Unesite iznos za transfer: ");

                _bank.Transfer(user, receiverAccount, amount);
                _fileManager.SaveUsers(_bank.GetUsers());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska pri transferu: {ex.Message}");
            }
        }
    }
}
