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

namespace library_project_wpf
{
    /// <summary>
    /// Interaction logic for UserMenu.xaml
    /// </summary>
    public partial class UserMenu : Window
    {
        public UserMenu()
        {
            InitializeComponent();
            WelcomeText();
        }

        private void WelcomeText()
        {
            var data = Task.Run(() => GetUserData());
            data.Wait();
            if (data.Result.Length > 0)
            {
                JObject j = JObject.Parse(data.Result);
                tbWelcomeText.Text = "Welcome, " + j["firstname"].ToString() + " " + j["lastname"].ToString() + "!";
            }
        }
        static async Task<string> GetUserData()
        {
            Singleton si = Singleton.Instance;
            string username = si.Username;
            var password = si.Password;
            var authData = Encoding.ASCII.GetBytes($"{username}:{password}");
            Console.WriteLine(username + password);
            var response = string.Empty;
            var url = DbEnvironment.GetBaseUrl() + "/Userdata/User/" + username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(response);
            return response;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnUserProfilie_Click(object sender, RoutedEventArgs e)
        {
            UserMenu objectUserDataMenu = new UserMenu();
            objectUserDataMenu.Show();
        }
    }
}
