using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Diagnostics;
using library_project_wpf;
using System.Net.Http;
using System.Threading;

namespace library_project_wpf
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            BtnLoginChecker();
        }

        private void BtnLoginChecker()
        {
            if (tbUsername.Text.Length < 4 || ckbShowPassword.IsChecked == false && tbPasswordBox.Password.Length < 4 || ckbShowPassword.IsChecked == true && tbPasswordTxtBox.Text.Length < 4)
            {
                btnLogin.IsEnabled = false;
            }
            else
            {
                btnLogin.IsEnabled = true;
            }
        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            tbPasswordTxtBox.Text = tbPasswordBox.Password;
            tbPasswordBox.Visibility = Visibility.Collapsed;
            tbPasswordTxtBox.Visibility = Visibility.Visible;
        }
        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            tbPasswordBox.Password = tbPasswordTxtBox.Text;
            tbPasswordBox.Visibility = Visibility.Visible;
            tbPasswordTxtBox.Visibility = Visibility.Collapsed;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = "";
            if (ckbShowPassword.IsChecked == false)
            {
                password = tbPasswordBox.Password.ToString();
            }
            else
            {
                password = tbPasswordTxtBox.Text;
            }
            var data = Task.Run(() => LoginToLibrary(username, password));
            try
            {
                data.Wait();
            }
            catch (Exception)
            {

                MessageBox.Show("Connection cannot be established");
                return;
            }

            if (data.Result.Length > 0)
            {
                string response = data.Result;
                Debug.WriteLine(response);
                if (string.Compare(response, "true") == 0)
                {
                    Console.WriteLine("You are logged in");
                    MessageBox.Show("You are logged in");
                    Singleton si = Singleton.Instance;
                    si.Username = username;
                    si.Password = password;
                    UserMenu MainMenu = new UserMenu();
                    MainMenu.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    MainMenu.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Username and password don't match.");
                    tbPasswordBox.Clear();
                }
            }
            else
            {
                MessageBox.Show("Something went wrong.");
            }
        }
        static async Task<string> LoginToLibrary(string username, string password)
        {
            string base_url = DbEnvironment.GetBaseUrl();
            var response = string.Empty;
            var url = base_url + "/api/login";
            Login objectLogin = new Login(username, password);
            var json = JsonConvert.SerializeObject(objectLogin);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            HttpResponseMessage result = await client.PostAsync(url, postData);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void tbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnLoginChecker();
        }

        private void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            BtnLoginChecker();
        }

        private void tbPasswordTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnLoginChecker();
        }
    }
}

