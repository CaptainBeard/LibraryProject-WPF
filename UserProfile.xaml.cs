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
using System.Reflection.Emit;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using System.Net;

namespace library_project_wpf
{
    public partial class UserProfile : Window
    {

        // Updatase data on previous window
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        //

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
            btnNewInformation.IsEnabled = !value;
        }

        private void FindData()
        {
            var data = Task.Run(() => GetUserData());
            data.Wait();
            if (data.Result.Length > 0)
            {
                JObject j = JObject.Parse(data.Result);
                tbUsername.Text = j["username"].ToString();
                tbFirstname.Text = j["firstname"].ToString();
                tbLastname.Text = j["lastname"].ToString();
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

        private void UpdateData()
        {
            string username = tbUsername.Text;
            string firstname = tbFirstname.Text;
            string lastname = tbLastname.Text;
            string phone = tbPhoneNumber.Text;
            string streetaddress = tbStreetAddress.Text;
            string postalcode = tbPostalCode.Text;
            var data = Task.Run(() => SendData(username, firstname, lastname, phone, streetaddress, postalcode));
            data.Wait();
            if (data.Result.Length > 0)
            {
                MessageBox.Show("Your personal information has been changed");
            }
        }

        static async Task<string> SendData(string username, string firstname, string lastname, string phone, string streetaddress, string postalcode)
        {
            string base_url = DbEnvironment.GetBaseUrl();
            var response = string.Empty;
            var url = base_url + "/api/Userdata/user/" + username;
            User objectUser = new User(username, firstname, lastname, phone, streetaddress, postalcode);
            var json = JsonConvert.SerializeObject(objectUser);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            HttpResponseMessage result = await client.PutAsync(url, postData);
            response = await result.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            Console.WriteLine(response);
            return response;
        }
        private void ChangeImageName(string image)
        {
            Singleton si = Singleton.Instance;
            string username = si.Username;
            var data = Task.Run(() => SendImageName(username, image));
            data.Wait();
            if (data.Result.Length > 0)
            {
                MessageBox.Show("Your image has been changed");
            }
        }

        static async Task<string> SendImageName(string username, string image)
        {

            string base_url = DbEnvironment.GetBaseUrl();
            var response = string.Empty;
            var url = base_url + "/api/Userdata/Image/" + username;
            ImgName objectUser = new ImgName(username, image);
            var json = JsonConvert.SerializeObject(objectUser);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            HttpResponseMessage result = await client.PutAsync(url, postData);
            response = await result.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            Console.WriteLine(response);
            return response;
        }
        private void ChangeImageFile(OpenFileDialog image)
        {
            // Sends image to the web api
            string base_url = DbEnvironment.GetBaseUrl();
            var url = base_url + "/api/Upload";
            var client = new WebClient();
            client.UploadFile(url, "POST", image.FileName);
        }

        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (btnSave.IsEnabled == true)
            {
                if (MessageBox.Show("Are you sure you want to close without saving?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    Close();
                }

            }
            Close();
        }

        private void btnNewInformation_Click(object sender, RoutedEventArgs e)
        {
            ToggleControls(true);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ToggleControls(false);
            UpdateData();
            // Updates data on the UserMenu
            DataChangedEventHandler handler = DataChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void btnNewPassword_Click(object sender, RoutedEventArgs e)
        {
            NewPassword ChangePassword = new NewPassword();
            ChangePassword.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ChangePassword.ShowDialog();
        }

        private void btnNewImage_Click(object sender, RoutedEventArgs e)
        {
            string fullPath;
            string imageName;
            string[] partsFileName;
            string[] FileNameWithoutFileExtension;
            string fileNameToSend;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a profile picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            try
            {
                if (op.ShowDialog() == true)
                {
                    fullPath = op.FileName;
                    partsFileName = fullPath.Split('\\');
                    imageName = partsFileName[partsFileName.Length - 1];
                    FileNameWithoutFileExtension = imageName.Split('.');
                    fileNameToSend = FileNameWithoutFileExtension[0] + ".png";
                    ChangeImageName(fileNameToSend);
                    ChangeImageFile(op);
                    // Updates user data on UserMenu
                    DataChangedEventHandler handler = DataChanged;
                    if (handler != null)
                    {
                        handler(this, new EventArgs());
                    }
                    // Updates user data on this window
                    FindData();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong");
            }

        }
    }
}
