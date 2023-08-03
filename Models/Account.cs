using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    class Account
    {
        private int _id;
        private string _username;
        private SecureString _password;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public SecureString Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
