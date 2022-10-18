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
using System.Windows.Shapes;

namespace library_project_wpf
{
    /// <summary>
    /// Interaction logic for LibraryData.xaml
    /// </summary>
    public partial class LibraryData : Window
    {
        public LibraryData()
        {
            InitializeComponent();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search SearchWindow = new Search();
            SearchWindow.Top = this.Top + 150;
            SearchWindow.Left = this.Left + 150;
            SearchWindow.ShowDialog();
        }
        private void btnLoans_Click(object sender, RoutedEventArgs e)
        {
            Loans LoansWindow = new Loans();
            LoansWindow.Top = this.Top + 150;
            LoansWindow.Left = this.Left + 150;
            LoansWindow.ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
