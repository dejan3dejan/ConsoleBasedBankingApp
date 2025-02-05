using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBasedBankingApp
{
     class User
    {
        public string AccountNumber {  get; set; }
        public string FullName { get; set; }
        public string HashedPIN { get; set; }
        public decimal Balance  { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public User() { }

        public User(string accountNumber, string fullName, string hashedPIN, decimal balance) 
        {
            AccountNumber = accountNumber;
            FullName = fullName;
            HashedPIN = hashedPIN;
            Balance = balance;
            Transactions = new List<Transaction>();
        }
        public bool VerifyPIN(string pin)
        {
            return BCrypt.Net.BCrypt.Verify(pin, HashedPIN);
        }
        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}
