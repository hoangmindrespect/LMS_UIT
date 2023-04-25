using LiveCharts.Wpf;
using LiveCharts;
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

namespace LibraryManagementSystem.View.MainWindow.Statistical
{
    /// <summary>
    /// Interaction logic for StatisticalPage.xaml
    /// </summary>
    public partial class StatisticalPage : Page
    {
        public StatisticalPage()
        {
            InitializeComponent(); 
        }

        private void statistical_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainWindowSystem w = Application.Current.Windows.OfType<MainWindowSystem>().FirstOrDefault();

            if (w.WindowState == WindowState.Maximized)
            {
                income.Width += 80;
                income.Height += 80;
                income.Margin = new Thickness(240, 0, 0, 0);

                spending.Width += 80;
                spending.Height += 80;
                spending.Margin = new Thickness(240, 0, 0, 0);

                profit.Width += 80;
                profit.Height += 80;
                profit.Margin = new Thickness(240, 0, 0, 0);

                label.FontSize = label1.FontSize = label2.FontSize = 20;
                label3.FontSize = label4.FontSize = label5.FontSize = 25;
                label3.Margin = new Thickness(10, -185, 0, 0);
                label4.Margin = new Thickness(10, -150, 0, 0);
                label5.Margin = new Thickness(10, -150, 0, 0);
                filter1.Margin = new Thickness(10, -305, 0, 0);
                filter2.Margin = new Thickness(10, -305, 0, 0);
                border1.Margin = new Thickness(10, -35, 0, 0);


            }
            else if (w.WindowState == WindowState.Normal)
            {
                income.Width = 100;
                income.Height = 100;
                income.Margin = new Thickness(140, -10, 0, 0);

                spending.Width = 93;
                spending.Height = 64.328;
                spending.Margin = new Thickness(150, 0, 0, 0);

                profit.Width = 93;
                profit.Height = 64.328;
                profit.Margin = new Thickness(150, 0, 0, 0);

                label.FontSize = label1.FontSize = label2.FontSize = 14;
                label3.FontSize = label4.FontSize = label5.FontSize = 17;
                label3.Margin = new Thickness(10, -95, 0, 0);
                label4.Margin = new Thickness(10, -70, 0, 0);
                label5.Margin = new Thickness(10, -70, 0, 0);
                filter1.Margin = new Thickness(10, -212, 0, 0);
                filter2.Margin = new Thickness(10, -212, 0, 0);
                border1.Margin = new Thickness(10, -27, 0, 0);
            }

        }
    }
}
