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

        public loginwindow()
        {
            InitializeComponent();
            a = blur_card;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFA5B9D6");
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF5757");
        }

        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
