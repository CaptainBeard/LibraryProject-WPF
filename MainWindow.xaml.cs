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
            InitializeTextBoxes();
            BtnLoginChecker();
        }

        private void InitializeTextBoxes()
        {
            tbUsername.Foreground = Brushes.Gray;
            tbPassword.Foreground = Brushes.Gray;
            tbUsername.Text = "Username";
            tbPassword.Text = "Password";
            tbUsername.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(TbFocus);
            tbPassword.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(TbFocus);
            tbUsername.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(TbLostFocus);
            tbPassword.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(TbLostFocus);
        }

        private void TbFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                //If nothing has been entered yet.
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }

        private void TbLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                //If nothing was entered, reset default text
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    if (tbUsername.IsFocused)
                        ((TextBox)sender).Text = "Username";
                    if (tbPassword.IsFocused)
                        ((TextBox)sender).Text = "Password";
                }
            }
        }

        private void BtnLoginChecker()
        {
            if (tbUsername.Text == "Username" || tbPassword.Text == "Password" || tbUsername.Text.Length < 4 || tbPassword.Text.Length < 4)
            {
                btnLogin.IsEnabled = false;
            }
            else
            {
                btnLogin.IsEnabled = true;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = tbPassword.Text;
            var data = Task.Run(() => LoginToLibrary(username, password));
            data.Wait();

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
                    UserMenu objectMainMenu = new UserMenu();
                    objectMainMenu.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Username and password don't match.");
                    InitializeTextBoxes();
                }
            }
            else
            {
                MessageBox.Show("Something went wrong.");
                InitializeTextBoxes();
            }
        }
        static async Task<string> LoginToLibrary(string username, string password)
        {
            string base_url = DbEnvironment.GetBaseUrl();
            var response = string.Empty;
            var url = base_url + "/login";
            User objectUser = new User(username, password);

            var json = JsonConvert.SerializeObject(objectUser);
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

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnLoginChecker();
        }

    }
}

