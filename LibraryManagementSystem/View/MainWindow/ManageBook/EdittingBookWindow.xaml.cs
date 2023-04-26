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

namespace LibraryManagementSystem.View.MainWindow.ManageBook
{
    /// <summary>
    /// Interaction logic for EdittingBookWindow.xaml
    /// </summary>
    public partial class EdittingBookWindow : Window
    {
        public static string masach;
        public static Image Image;
        public EdittingBookWindow()
        {
            InitializeComponent();
        }
        public EdittingBookWindow(string a)
        {
            InitializeComponent();
            masach = a;
            Image = image_img;
        }
    }
}
