using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection.Metadata;
using ConsoleBasedBankingApp;

namespace ConsoleBasedBankingApp
{
    class FileManager
    {
        private const string filePath = "users.json";

        public void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(filePath, json);
        }

        public List<User> LoadUsers()
        {
            if (File.Exists("users.json"))
            {
                string json = File.ReadAllText("users.json");
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            return new List<User>();
        }

        public List<User> GetUsers()
        {
            return LoadUsers();
        }


    }
}
