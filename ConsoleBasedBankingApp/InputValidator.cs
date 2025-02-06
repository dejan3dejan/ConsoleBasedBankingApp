using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBasedBankingApp
{
    public static class InputValidator
    {
        public static string GetValidString(string prompt, int minLength, int maxLength)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Unos ne moze biti prazan.");
                    continue;
                }

                if (input.Length < minLength || input.Length > maxLength)
                {
                    Console.WriteLine($"Unos mora biti izmedju {minLength} i {maxLength} karaktera.");
                    continue;
                }

                return input;
            }
        }

        public static string GetValidPin()
        {
            while (true)
            {
                string pin = GetValidString("Unesite PIN (4 cifre): ", 4, 4);

                if (!pin.All(char.IsDigit))
                {
                    Console.WriteLine("PIN mora sadrzati samo brojeve.");
                    continue;
                }

                return pin;
            }
        }

        public static string GetValidAccountNumber(string prompt)
        {
            while (true)
            {
                string accountNumber = GetValidString(prompt, 6, 6);

                if (!accountNumber.All(char.IsDigit))
                {
                    Console.WriteLine("Broj racuna mora sadrzati samo brojeve.");
                    continue;
                }

                return accountNumber;
            }
        }

        public static decimal GetValidAmount(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    if (amount <= 0)
                    {
                        Console.WriteLine("Iznos mora biti veci od 0.");
                        continue;
                    }

                    if (amount > 1000000)
                    {
                        Console.WriteLine("Iznos ne moze biti veci od 1,000,000 RSD.");
                        continue;
                    }

                    return amount;
                }

                Console.WriteLine("Nevazeci iznos. Pokusajte ponovo.");
            }
        }
    }
}
