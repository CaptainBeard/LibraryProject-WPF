using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    internal class ImgName
    {
        public string Username { get; set; }
        public string Image { get; set; }

        public ImgName(string username, string image)
        {
            Username = username;
            Image = image;
        }
    }
}
