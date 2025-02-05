using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection.Metadata;
using ConsoleBasedBankingApp;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace ConsoleBasedBankingApp
{
    class FileManager
    {
        private const string filePath = "users.json";

        public List<User> LoadUsers()
        {
            if (!File.Exists(filePath)) return new List<User>();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        public void SaveUsers(List<User> users)
        {
            string json = JsonConvert.SerializeObject(
                users, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public List<User> GetUsers()
        {
            return LoadUsers();
        }
    }
}
