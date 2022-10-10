using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_project_wpf
{
    internal class DbEnvironment
    {
        public static string GetBaseUrl()
        {
            return "https://localhost:7161/api";
            //return "https://my-dotnet-university.herokuapp.com/api";
        }
    }
}
