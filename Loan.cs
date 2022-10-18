using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    internal class Loan
    {
        public string Name { get; set; }
        public DateTime Loan_date { get; set; }
        public DateTime Loan_end { get; set; }

        public Loan(string name, DateTime loan_date, DateTime loan_end)
        {
            Name = name;
            Loan_date = loan_date;
            Loan_end = loan_end;
        }
        public Loan()
        {
        }
        
    }
}
