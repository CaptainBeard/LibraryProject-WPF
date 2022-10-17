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
    public partial class UserMenu : Window
    {
        public UserMenu()
        {
            InitializeComponent();
            WelcomeText();
            UserData.myUserMenu = this;
        }

        private void WelcomeText()
        {
            var data = Task.Run(() => UserData.GetUserData());
            data.Wait();
            if (data.Result.Length > 0)
            {
                JObject j = JObject.Parse(data.Result);
                tbWelcomeText.Text = "Welcome, " + j["firstname"].ToString() + " " + j["lastname"].ToString() + "!";
                BitmapImage biUserAvatar = new BitmapImage();
                biUserAvatar.BeginInit();
                biUserAvatar.UriSource = new Uri(DbEnvironment.GetBaseUrl() + "/images/" + j["image"].ToString(), UriKind.RelativeOrAbsolute);
                biUserAvatar.EndInit();
                imgUserAvatar.Stretch = Stretch.Fill;
                imgUserAvatar.Source = biUserAvatar;
            }
        }

        private void btnUserProfilie_Click(object sender, RoutedEventArgs e)
        {
            UserProfile UserMenu = new UserProfile();
            UserMenu.DataChanged += InformationUpdate;
            UserMenu.Top = this.Top + 150;
            UserMenu.Left = this.Left + 150;
            UserMenu.ShowDialog();
        }

        private void InformationUpdate(object sender, EventArgs e)
        {
            WelcomeText();
        }
        private void btnBookSearch_Click(object sender, RoutedEventArgs e)
        {
            LibraryData LibraryMenu = new LibraryData();
            LibraryMenu.Top = this.Top + 150;
            LibraryMenu.Left = this.Left + 150;
            LibraryMenu.ShowDialog();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
