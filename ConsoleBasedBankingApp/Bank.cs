using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
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

        public User Login(string accountNumber, string pin)
        {
            User user = users.FirstOrDefault(u => u.AccountNumber == accountNumber);
            if (user != null && user.VerifyPIN(pin))
            {
                return user;
            }
            return null;
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
            Console.WriteLine($"Uspesno ste uplatili {amount}RSD. Novo stanje na racunu je: {user.Balance}");
        }
        public void Withdraw(User user, decimal amount)
        {
            if(user.Balance >= amount)
            {
                user.Balance -= amount;
                Console.WriteLine($"Uspesno ste podigli {amount}RSD. Novo stranje na racunu je {user.Balance}");
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
                sender.Balance -= amount;
                receiver.Balance += amount;
                Console.WriteLine($"Uspesno ste poslali {amount}RSD korisniku {receiver.FullName}");
            }
            else
            {
                Console.WriteLine("Nemate dovoljno sredstava!");
            }
        }
    }
}
