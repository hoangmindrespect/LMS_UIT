using MaterialDesignThemes.Wpf;
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

namespace LibraryManagementSystem.View.MainClientWindow.ManageCusOrders
{
    /// <summary>
    /// Interaction logic for ManageCusOrders.xaml
    /// </summary>
    public partial class ManageCusOrders : Page
    {
        public ManageCusOrders()
        {
            InitializeComponent();
        }

        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            a.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#948F89"));
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            a.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }
    }
}
