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

namespace LibraryManagementSystem.View.Login
{
    /// <summary>
    /// Interaction logic for VertificationPage.xaml
    /// </summary>
    public partial class VertificationPage : Page
    {
        public VertificationPage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Error != null)
            {
                Error.Visibility = Visibility.Hidden;
            }
        }
    }
}
