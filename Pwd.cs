using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    internal class Pwd
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Pwd(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
