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
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace library_project_wpf
{
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
            if (data.Result.Length > 0)
            {
                dynamic book_data = JsonConvert.DeserializeObject(data.Result);
                List<Book> book_list = new List<Book>();
                foreach (var book in book_data)
                {
                    Console.WriteLine(book);
                    book_list.Add(new Book() { Name = book.name, Author = book.author, Language = book.language, Year = book.year, Isbn = book.isbn, Status = book.status });
                }
                gridBooks.ItemsSource = book_list;
            }
            else
            {
                MessageBox.Show("There are no books");
            }
        }
        static async Task<string> GetAllBooks()
        {
            var response = string.Empty;
            var url = DbEnvironment.GetBaseUrl() + "/api/Bookdata";
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
