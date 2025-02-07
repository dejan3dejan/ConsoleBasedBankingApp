using System;
using System.CodeDom.Compiler;
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
        public string? ReceiverAccount { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }

        public Transaction() { }

        public Transaction(string type, decimal amount, string? sender, string? receiver, string description = "")
        {
            Timestamp = DateTime.Now;
            Type = type;
            Amount = amount;
            SenderAccount = sender;
            ReceiverAccount = receiver;
            Description = description;
            Reference = GenerateReference();
        }

        private string GenerateReference()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string random = new Random().Next(1000, 9999).ToString();
            return $"{timestamp}-{random}"; //yyyyMMdd-HHmmss-XXXX
        }

        public override string ToString()
        {
            if (SenderAccount == null)
                return $"{Timestamp}: {Type} od {Amount} RSD na {ReceiverAccount}";
            else
                return $"{Timestamp}: {Type} {Amount} RSD od {SenderAccount} ka {ReceiverAccount}";

            string formattedAmount = $"{Amount:N2} RSD";
            string formattedTimestamp = Timestamp.ToString("dd.MM.yyyy HH:mm:ss");

            switch (Type)
            {
                case "Deposit":
                    return $"{formattedTimestamp} | Uplata | +{formattedAmount} | {Description}";
                case "Withdraw":
                    return $"{formattedTimestamp} | Isplata | -{formattedAmount} | {Description}";
                case "Transfer Out":
                    return $"{formattedTimestamp} | Poslato | -{formattedAmount} | Ka: {ReceiverAccount} | {Description}";
                case "Transfer In":
                    return $"{formattedTimestamp} | Primljeno | +{formattedAmount} | Od: {SenderAccount} | {Description}";
                default:
                        return $"{formattedTimestamp} | {Type} | {formattedAmount} | {Description}";
            }
        }
    }
}