using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.Classes
{
    class Writer
    {
        public static void WriteDBString(string text)
        {
            try
            {
                string complString = text;
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ConnectionString.txt");
                ProcessWrite(path, complString).Wait();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }

        private static object ProcessWrite(string path, object complString)
        {
            throw new NotImplementedException();
        }

        static Task ProcessWrite(string path, string text)
        {
            return WriteTextAsync(path, text);
        }

        static async Task WriteTextAsync(string path, string text)
        {
            try
            {
                byte[] encodedText = System.Text.Encoding.UTF8.GetBytes(text);
                using (FileStream sourceStream = new FileStream(path,
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true))
                {
                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }
    }
}
