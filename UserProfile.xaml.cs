using Newtonsoft.Json.Linq;
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
using Newtonsoft.Json;

namespace library_project_wpf
{
    /// <summary>
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        public UserProfile()
        {
            InitializeComponent();
            ToggleControls(false);
            FindData();
        }

        private void ToggleControls(bool value)
        {
            tbFirstname.IsEnabled = value;
            tbLastname.IsEnabled = value;
            tbUsername.IsEnabled = value;
            tbPhoneNumber.IsEnabled = value;
            tbStreetAddress.IsEnabled = value;
            tbPostalCode.IsEnabled = value;
            btnSave.IsEnabled = value;
        }

        private void FindData()
        {
            var data = Task.Run(() => GetUserData());
            data.Wait();
            if (data.Result.Length > 0)
            {
                JObject j = JObject.Parse(data.Result);
                tbFirstname.Text = j["firstname"].ToString();
                tbLastname.Text = j["lastname"].ToString();
                tbUsername.Text = j["username"].ToString();
                tbPhoneNumber.Text = j["phone"].ToString();
                tbStreetAddress.Text = j["streetaddress"].ToString();
                tbPostalCode.Text = j["postalcode"].ToString();
                BitmapImage biUserAvatar = new BitmapImage();
                biUserAvatar.BeginInit();
                biUserAvatar.UriSource = new Uri(DbEnvironment.GetBaseUrl() + "/images/" + j["image"].ToString(), UriKind.RelativeOrAbsolute);
                biUserAvatar.EndInit();
                imgUserAvatar.Stretch = Stretch.Fill;
                imgUserAvatar.Source = biUserAvatar;
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
            var url = DbEnvironment.GetBaseUrl() + "/api/Userdata/User/" + username;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }
        static async Task<string> UpdateInformation(string username, string password)
        {
            string base_url = DbEnvironment.GetBaseUrl();
            var response = string.Empty;
            var url = base_url + "/api/login";
            User objectUser = new User(username, password);

            var json = JsonConvert.SerializeObject(objectUser);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            HttpResponseMessage result = await client.PostAsync(url, postData);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnNewInformation_Click(object sender, RoutedEventArgs e)
        {
            ToggleControls(true);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ToggleControls(false);

        }
    }
}
