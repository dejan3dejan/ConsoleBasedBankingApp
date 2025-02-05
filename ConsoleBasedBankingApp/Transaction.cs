using System;
using ConsoleBasedBankingApp;
using System.Text.Json.Serialization;


namespace ConsoleBasedBankingApp
{
    public class Transaction
    {
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } // "Deposit", "Withdraw", "Transfer"
        public decimal Amount { get; set; }
        public string? SenderAccount { get; set; }
        public string ReceiverAccount { get; set; }

        public Transaction() { }

        public Transaction(string type, decimal amount, string? sender, string receiver)
        {
            Timestamp = DateTime.Now;
            Type = type;
            Amount = amount;
            SenderAccount = sender;
            ReceiverAccount = receiver;
        }

        public override string ToString()
        {
            if (SenderAccount == null)
                return $"{Timestamp}: {Type} od {Amount} RSD na {ReceiverAccount}";
            else
                return $"{Timestamp}: {Type} {Amount} RSD od {SenderAccount} ka {ReceiverAccount}";
        }
    }
}