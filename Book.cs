using library_project_wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    internal class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public int Year { get; set; }
        public string Isbn { get; set; }
        public string Status { get; set; }
        public Book(string name, string author, string language, int year, string isbn, string status)
        {
            Name = name;
            Author = author;
            Language = language;
            Year = year;
            Isbn = isbn;
            Status = status;
        }
        public Book()
        {
        }
    }
}