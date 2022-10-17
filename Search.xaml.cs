using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

namespace library_project_wpf
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public Search()
        {
            InitializeComponent();
            FindBooks();
        }
        private void FindBooks()
        {
            var data = Task.Run(() => GetAllBooks());
            data.Wait();
            Console.WriteLine(data.Result);
            if (data.Result.Length > 3) //Result is not []
            {
                dynamic book_data = JsonConvert.DeserializeObject(data.Result);

                gridBooks.ItemsSource = book_data;//writes the data to DataGrid

                string books = "";
                foreach (var book in book_data)
                {
                    books += book.name + " | " + book.author + " | " + "\n";
                }
                //txtBooks.Text = book_data;*/
                Console.WriteLine(books);
            }
            else
            {
                MessageBox.Show("There are no books");
            }
        }
        static async Task<string> GetAllBooks()
        {
            var response = string.Empty;
            var url = DbEnvironment.GetBaseUrl() + "/api/Book";
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
