using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    public static class UserData
    {
        public static UserMenu myUserMenu;

        public static async Task<string> GetUserData()
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
    }
}
