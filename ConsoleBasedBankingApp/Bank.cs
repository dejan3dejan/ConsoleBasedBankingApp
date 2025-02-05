using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBasedBankingApp
{
    class Bank
    {
        private List<User> users;
        private FileManager fileManager;

        public Bank()
        {
            fileManager = new FileManager();
            users = fileManager.LoadUsers();
        }

        public void RegisterUser(string fullName, string pin)
        {
            string accountNumber = GenerateAccountNumber();
            User newUser = new User(accountNumber, fullName, BCrypt.Net.BCrypt.HashPassword(pin), 0);
            users.Add(newUser);
            Console.WriteLine($"Uspesno kreiran nalog! Vas broj racuna je: {accountNumber}");
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

            // Provera da li je korisnik blokiran
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
            user.Balance += amount;

            user.AddTransaction(new Transaction("Deposit", amount, null, user.AccountNumber));

            fileManager.SaveUsers(users);
            Console.WriteLine($"Uspesno ste uplatili {amount}RSD. Novo stanje na racunu je: {user.Balance}RSD");
        }
        public void Withdraw(User user, decimal amount)
        {
            if(user.Balance >= amount)
            {
                user.Balance -= amount;

                user.AddTransaction(new Transaction("Withdraw", amount, user.AccountNumber, null));

                fileManager.SaveUsers(users);
                Console.WriteLine($"Uspesno ste podigli {amount}RSD. Novo stranje na racunu je {user.Balance}RSD");
            }
            else
            {
                Console.WriteLine("Nemate dovoljno sredstava!");
            }
        }

        public void Transfer(User sender, string receiverAccountNumber, decimal amount)
        {
            User receiver = users.FirstOrDefault(u => u.AccountNumber == receiverAccountNumber);
            Console.WriteLine($"Transferujete sredstva korisniku {receiver.FullName}. Unesite kolicinu koju zelite da transferujete: ");

            if(receiver != null && sender.Balance >= amount)
            {
                sender.Balance -= amount;
                receiver.Balance += amount;

                sender.Transactions.Add(new Transaction("Transfer Out", amount, sender.AccountNumber, receiver.AccountNumber));
                receiver.Transactions.Add(new Transaction("Transfer In", amount, sender.AccountNumber, receiver.AccountNumber));

                fileManager.SaveUsers(users);
                Console.WriteLine($"Uspesno ste poslali {amount}RSD korisniku {receiver.FullName}.");
            }
            else
            {
                Console.WriteLine("Nemate dovoljno sredstava!");
            }
        }
        public void ShowTransactionHistory(User user)
        {
            Console.WriteLine("\nISTORIJA TRANSAKCIJA");

            if (user.Transactions.Count == 0)
            {
                Console.WriteLine("Nema dostupnih transakcija.");
                return;
            }

            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("| Tip         | Iznos      | Od        | Ka        |");
            Console.WriteLine("----------------------------------------------------");

            foreach (var transaction in user.Transactions)
            {
                Console.WriteLine($"| {transaction.Type,-10} | {transaction.Amount,10} RSD | {transaction.SenderAccount ?? "N/A",-8} | {transaction.ReceiverAccount ?? "N/A",-8} |");
            }

            Console.WriteLine("----------------------------------------------------");
        }
    }
}
