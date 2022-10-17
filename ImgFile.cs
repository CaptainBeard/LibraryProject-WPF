using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    internal class ImgFile
    {
        public OpenFileDialog Image { get; set; }

        public ImgFile(OpenFileDialog image)
        {
            Image = image;
        }
    }
}
