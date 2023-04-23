using LibraryManagementSystem.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace LibraryManagementSystem.View.Login
{
    /// <summary>
    /// Interaction logic for loginwindow.xaml
    /// </summary>
    public partial class loginwindow : Window
    {
        public static Card a;
        private static loginwindow _instance;

        public loginwindow()
        {
            InitializeComponent();
            DataContext = new LoginRegisViewModel();
            a = blur_card;
        }

        private void btnMinimizeMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA5B9D6");
        }

        private void btnMinimizeMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            btn.Background = new SolidColorBrush(Colors.White);
        }

        private void btnCloseMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA5B9D6");
            btn.Background = new SolidColorBrush(Colors.OrangeRed);
        }

        private void btnCloseMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush(Colors.White);
        }

        private void btnMinimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
