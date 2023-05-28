using LibraryManagementSystem.DTOs;
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

namespace LibraryManagementSystem.View.MainWindow.ManageUser
{
    /// <summary>
    /// Interaction logic for ManageUserPage.xaml
    /// </summary>
    public partial class ManageUserPage : Page
    {
        public ManageUserPage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged_Find(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dtg_manage.ItemsSource).Refresh();
            CreateTextBoxFilter();
        }

        public void CreateTextBoxFilter()
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dtg_manage.ItemsSource);
            view.Filter = Filter;
        }

        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(txbFilter.Text))
                return true;
            else
                return ((item as UserDTO).FullName.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    ((item as UserDTO).EmailAddress.IndexOf(txbFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
