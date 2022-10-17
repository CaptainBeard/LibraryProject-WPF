using Newtonsoft.Json;
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
using static System.Net.Mime.MediaTypeNames;

namespace library_project_wpf
{
    /// <summary>
    /// Interaction logic for NewPassword.xaml
    /// </summary>
    public partial class NewPassword : Window
    {
        public NewPassword()
        {
            InitializeComponent();
        }


        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            Singleton si = Singleton.Instance;
            string username = si.Username;
            string password = si.Password;
            string oldPassword = tbOldPassword.Password.ToString();
            string NewPassword = tbNewPassword.Password.ToString();
            string ConfirmedPassword = tbConfirmedPassword.Password.ToString();
            if (oldPassword == "" || NewPassword == "" || ConfirmedPassword == "")
            {
                MessageBox.Show("You must fill all fields");
            }
            else if (oldPassword != password)
            {
                MessageBox.Show("Old password is not correct");
            }
            else if (oldPassword == NewPassword)
            {
                MessageBox.Show("Old and new password is the same");
            }
            else if (NewPassword != ConfirmedPassword)
            {
                MessageBox.Show("New password doesn't match");
            }
            else
            {
                UpdateData(username, NewPassword);
            }
        }
        private void UpdateData(string username, string password)
        {
            var data = Task.Run(() => SendData(username, password));
            data.Wait();
            if (data.Result.Length > 0)
            {
                MessageBox.Show("Your password has been changed.");
                Singleton si = Singleton.Instance;
                si.Password = password;
                Close();
            }
        }
        static async Task<string> SendData(string username, string password)
        {
            string base_url = DbEnvironment.GetBaseUrl();
            var response = string.Empty;
            var url = base_url + "/api/Userdata/Password/" + username;
            Pwd objectUser = new Pwd(username, password);
            var json = JsonConvert.SerializeObject(objectUser);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            HttpResponseMessage result = await client.PutAsync(url, postData);
            response = await result.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            Console.WriteLine(response);
            return response;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
