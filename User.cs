using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    internal class User
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Streetaddress { get; set; }
        public string Postalcode { get; set; }
        public User(string username, string firstname, string lastname, string phone, string streetaddress, string postalcode)
        {
            Username = username;
            Firstname = firstname;
            Lastname = lastname;
            Phone = phone;
            Streetaddress = streetaddress;
            Postalcode = postalcode;
        }
    }
}
