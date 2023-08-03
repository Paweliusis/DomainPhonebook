using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Classes
{
    class Reader
    {
        public static string ReadConnectionString()
        {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\ConnectionString.txt";
            var connectionString = File.ReadAllText(path);
            return connectionString.ToString();
        }
    }
}
