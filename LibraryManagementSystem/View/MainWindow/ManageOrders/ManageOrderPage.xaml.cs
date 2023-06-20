using DocumentFormat.OpenXml.Wordprocessing;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
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

namespace LibraryManagementSystem.View.MainWindow.ManageOrders
{
    /// <summary>
    /// Interaction logic for ManageOrderPage.xaml
    /// </summary>
    public partial class ManageOrderPage : Page
    {
        public ManageOrderPage()
        {
            InitializeComponent();
        }
        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            a.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#948F89"));
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            a.Background = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            OrderDTO order = orderList.SelectedItem as OrderDTO;
            using(var context = new LMSEntities1())
            {
                foreach(var item in context.ORDER_BOOKS)
                {
                    if(item.orderID == order.Id)
                    {
                        if (item.orderStatus == 4)
                            return;
                        else
                            item.orderStatus += 1;
                        MessageBox.Show(item.orderStatus.ToString());
                        break;
                    }    
                }  
                context.SaveChanges();
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OrderDTO order = orderList.SelectedItem as OrderDTO;
            using (var context = new LMSEntities1())
            {
                foreach (var item in context.ORDER_BOOKS)
                {
                    if (item.orderID == order.Id)
                    {
                        if (item.orderStatus == 1)
                            return;
                        else
                            item.orderStatus -= 1;

                        break;
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
