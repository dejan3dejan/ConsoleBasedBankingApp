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
        public string PIN { get; set; } //Hashovati
        public decimal Balance  { get; set; }

        public User(string accountNumber, string fullName, string pin, decimal balance) 
        {
            AccountNumber = accountNumber;
            FullName = fullName;
            PIN = pin;
            Balance = balance;
        }
    }
}
