using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryManagementSystem.View.MainWindow.ImportBook
{
    /// <summary>
    /// Interaction logic for ImportBookPage.xaml
    /// </summary>
    public partial class ImportBookPage : Page
    {
        public ImportBookPage()
        {
            InitializeComponent();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainWindowSystem w = Application.Current.Windows.OfType<MainWindowSystem>().FirstOrDefault();

            if(w.WindowState == WindowState.Maximized)
            {
                dtg_Import.FontSize = 17;
                stack1.Margin = new Thickness(0, -180, 0, 0);
            }    
            else if(w.WindowState == WindowState.Normal)
            {
                dtg_Import.FontSize = 14;
                stack1.Margin = new Thickness(0, -0, 0, 0);
            }    
        }
    }
}
