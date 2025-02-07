using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleBasedBankingApp
{
    public partial class Bank
    {
        private List<User> users;
        private FileManager fileManager;

        public Bank()
        {
            fileManager = new FileManager();
            users = fileManager.LoadUsers();
        }

        public string RegisterUser(string fullName, string pin)
        {
            string accountNumber = GenerateAccountNumber();
            User newUser = new User(accountNumber, fullName, BCrypt.Net.BCrypt.HashPassword(pin), 0);
            users.Add(newUser);
            Console.WriteLine($"\nUspesno kreiran nalog!");
            Console.WriteLine($"Vas broj racuna je: {accountNumber}");
            Console.WriteLine("\nZAPAMTITE VAŠ BROJ RAČUNA, TREBACE VAM ZA PRIJAVU!");
            return accountNumber;
        }

        private Dictionary<string, int> failedLoginAttempts = new Dictionary<string, int>();

        public User Login(string accountNumber, string pin)
        {
            User user = users.FirstOrDefault(u => u.AccountNumber == accountNumber);

            if (user == null)
            {
                Console.WriteLine("Račun ne postoji.");
                return null;
            }

            if (failedLoginAttempts.ContainsKey(accountNumber) && failedLoginAttempts[accountNumber] >= 3)
            {
                Console.WriteLine("Vaš nalog je privremeno zaključan zbog previše neuspešnih pokušaja.");
                return null;
            }

            if (user.VerifyPIN(pin))
            {
                failedLoginAttempts[accountNumber] = 0; // Reset pokušaja
                Console.WriteLine("Uspešno ste se prijavili!");
                return user;
            }
            else
            {
                if (!failedLoginAttempts.ContainsKey(accountNumber))
                {
                    failedLoginAttempts[accountNumber] = 0;
                }
                failedLoginAttempts[accountNumber]++;

                Console.WriteLine($"Neispravan PIN. Preostalo pokušaja: {3 - failedLoginAttempts[accountNumber]}");

                return null;
            }
        }

        public List<User> GetUsers()
        {
            return users;
        }

        private string GenerateAccountNumber()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public void Deposit(User user, decimal amount)
        {
            string description = InputValidator.GetValidString("Unesite opis transakcije (opcionalno): ", 0, 100);
            
            user.Balance += amount;
            user.AddTransaction(new Transaction("Deposit", amount, null, user.AccountNumber, description));

            fileManager.SaveUsers(users);
            Console.WriteLine($"\nUspesno ste uplatili {amount:N2}RSD");
            Console.WriteLine($"Novo stanje na racunu je: {user.Balance:N2}RSD");
        }
        public void Withdraw(User user, decimal amount)
        {
            if(user.Balance >= amount)
            {
                string description = InputValidator.GetValidString("Unesite opis transakcije (opcionalno): ", 0, 100);

                user.Balance -= amount;
                user.AddTransaction(new Transaction("Withdraw", amount, user.AccountNumber, null, description));

                fileManager.SaveUsers(users);
                Console.WriteLine($"\nUspesno ste podigli {amount:N2}RSD");
                Console.WriteLine($"Novo stanje na racunu je: {user.Balance:N2}RSD");
            }
            else
            {
                Console.WriteLine("Nemate dovoljno sredstava!");
            }
        }

        public void Transfer(User sender, string receiverAccountNumber, decimal amount)
        {
            User receiver = users.FirstOrDefault(u => u.AccountNumber == receiverAccountNumber);

            if(receiver != null && sender.Balance >= amount)
            {
                Console.WriteLine($"Transferujete sredstva korisniku {receiver.FullName}");
                string description = InputValidator.GetValidString("Unesite opis transakcije (opcionalno): ", 0, 100);

                sender.Balance -= amount;
                receiver.Balance += amount;

                var transferOut = new Transaction("Transfer Out", amount, sender.AccountNumber, receiver.AccountNumber, description);
                var transferIn = new Transaction("Transfer Out", amount, sender.AccountNumber, receiver.AccountNumber, description)
                {
                    Reference = transferOut.Reference
                };

                sender.AddTransaction(transferOut);
                receiver.AddTransaction(transferIn);

                fileManager.SaveUsers(users);
                Console.WriteLine($"\nUspesno ste poslali {amount:N2}RSD korisniku {receiver.FullName}");
                Console.WriteLine($"Referenca transakcije: {transferOut.Reference}");
            }
            else
            {
                if (receiver == null)
                    Console.WriteLine("Racun primaoca nije pronadjen!");
                else
                    Console.WriteLine("Nemate dovoljno sredstava!");
            }
        }
        public void ShowTransactionHistory(User user)
        {
            Console.Clear();
            Console.WriteLine("\n=== ISTORIJA TRANSAKCIJA ===\n");

            if (user.Transactions.Count == 0)
            {
                Console.WriteLine("Nema dostupnih transakcija.");
                return;
            }

            int dateWidth = 19;
            int typeWidth = 10;
            int amountWidth = 15;
            int descWidth = 30;

            // Header
            string header = $"|{"Datum i vreme".PadRight(dateWidth)}|" +
                            $"{"Tip".PadRight(typeWidth)}|" +
                            $"{"Iznos".PadRight(amountWidth)}|" +
                            $"{"Opis".PadRight(descWidth)}|";

            string separator = new string('-', header.Length);

            Console.WriteLine(separator);
            Console.WriteLine(header);
            Console.WriteLine(separator);

            // Printanje Transakcija
            foreach (var transaction in user.Transactions.OrderByDescending(t => t.Timestamp))
            {
                string amount = transaction.Type.Contains("Out") ?
                    $"-{transaction.Amount:N2}" :
                    $"+{transaction.Amount:N2}";

                Console.WriteLine($"|{transaction.Timestamp.ToString("dd.MM.yyyy HH:mm:ss").PadRight(dateWidth)}|" +
                                  $"{transaction.Type.PadRight(typeWidth)}|" +
                                  $"{amount.PadRight(amountWidth)}|" +
                                  $"{(transaction.Description ?? "").PadRight(descWidth)}|");
            }

            Console.WriteLine(separator);
            Console.WriteLine($"\nUkupan broj transakcija: {user.Transactions.Count}");
            Console.WriteLine($"Trenutno stanje: {user.Balance:N2} RSD\n");
        }
    }
}
