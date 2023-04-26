using LibraryManagementSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace LibraryManagementSystem.View.MainWindow.ManageBook
{
    /// <summary>
    /// Interaction logic for ManageBookPage.xaml
    /// </summary>
    public partial class ManageBookPage : Page
    {
        public ManageBookPage()
        {
            InitializeComponent();
        }

        private void mainwindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainWindowSystem w = Application.Current.Windows.OfType<MainWindowSystem>().FirstOrDefault();

            if(w.WindowState == WindowState.Maximized)
            {
                dtg_manage.FontSize = 16;
                c1.Width = 80;
                c1.FontSize = c2.FontSize = c3.FontSize = c4.FontSize = c5.FontSize = c6.FontSize = c7.FontSize = c8.FontSize = 14;
            }    
            else if (w.WindowState == WindowState.Normal)
            {
                dtg_manage.FontSize = 14;
                c1.Width = 60;
                c1.FontSize = c2.FontSize = c3.FontSize = c4.FontSize = c5.FontSize = c6.FontSize = c7.FontSize = c8.FontSize = 12;

            }
        }

        #region Search
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(txbFilter.Text))
                return true;
            else
                return ((item as BookDTO).TenSach.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as BookDTO).TacGia.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void CreateTextBoxFilter()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dtg_manage.ItemsSource);
            view.Filter = Filter;
        }

        private void TextBox_TextChanged_Find(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dtg_manage.ItemsSource).Refresh();
            CreateTextBoxFilter();
        }
        #endregion
    }
}
