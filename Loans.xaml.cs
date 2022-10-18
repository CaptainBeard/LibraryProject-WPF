using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace library_project_wpf
{
    /// <summary>
    /// Interaction logic for Loans.xaml
    /// </summary>
    public partial class Loans : Window
    {
        public Loans()
        {
            InitializeComponent();
            GetLoans();
        }

        private void GetLoans()
        {
            var data = Task.Run(() => GetAllLoans());
            data.Wait();
            if (data.Result.Length > 0)
            {
                dynamic loan_data = JsonConvert.DeserializeObject(data.Result);
                List<Loan> loan_list = new List<Loan>();
                foreach (var loan in loan_data)
                {
                    Console.WriteLine(loan);
                    loan_list.Add(new Loan() { Name = loan.name, Loan_date = loan.loan_date, Loan_end = loan.loan_end });
                }
                gridLoans.ItemsSource = loan_list;
            }
            else
            {
                MessageBox.Show("There are no loans");
            }
        }

        static async Task<string> GetAllLoans()
        {
            Singleton si = Singleton.Instance;
            string username = si.Username;
            var response = string.Empty;
            var url = DbEnvironment.GetBaseUrl() + "/api/Loan/" + username;
            var client = new HttpClient();
            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
