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

namespace library_project_wpf
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            initializeTextBoxes();
        }

        private void initializeTextBoxes()
        {
            string textUsername = "Username";
            tbUsername.Foreground = Brushes.Gray;
            tbPassword.Foreground = Brushes.Gray;
            tbUsername.Text = textUsername;
            tbPassword.Text = "Password";
            //tbUsername.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_GotKeyboardFocus);
            //tbUsername.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_LostKeyboardFocus);
            //tbPassword.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_GotKeyboardFocus);
            //tbPassword.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(tb_LostKeyboardFocus);
            
        }

        private void tb_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
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


        private void tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Make sure sender is the correct Control.
            if (sender is TextBox)
            {
                //If nothing was entered, reset default text.
                if (((TextBox)sender).Text.Trim().Equals(""))
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    ((TextBox)sender).Text = "Text";
                }
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
                    Console.WriteLine("Login OK");
                    MessageBox.Show("Login OK");
                    Singleton si = Singleton.Instance;
                    si.Username = username;
                    si.Password = password;
                    UserMenu objectStudentMenu = new UserMenu();
                    objectStudentMenu.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Wrong username/password");
                    tbUsername.Clear();
                    tbPassword.Clear();
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
                tbUsername.Clear();
                tbPassword.Clear();
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
    }
}

