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
using System.Windows.Media.Effects;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf.Transitions;
using DocumentFormat.OpenXml.Presentation;
using System.Windows.Media.Animation;
using DocumentFormat.OpenXml.Spreadsheet;
using LibraryManagementSystem.DTOs;
using CloudinaryDotNet;

namespace LibraryManagementSystem.View.MainClientWindow.BuyBookPage
{
    /// <summary>
    /// Interaction logic for BuyBookPage.xaml
    /// </summary>
    public partial class BuyBookPage : System.Windows.Controls.Page
    {   public BuyBookPage(string AccountID)
        {
            InitializeComponent();
            idTb.Text = AccountID;
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            //a.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#DFDCD7");
            a.Cursor = Cursors.Hand;
            a.RenderTransform = new TranslateTransform(0, -2);
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            Card a = sender as Card;
            //a.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
            a.Cursor = Cursors.None;
            a.RenderTransform = new TranslateTransform(0, 2);

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
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource);
            view.Filter = Filter;
        }

        private void TextBox_TextChanged_Find(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListViewProducts.ItemsSource).Refresh();
            CreateTextBoxFilter();
        }
        #endregion

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainClientWindow w = Application.Current.Windows.OfType<MainClientWindow>().FirstOrDefault();

            if(w.WindowState == WindowState.Maximized)
            {
                imageControl.Width *= 1.2;
                imageControl.Height *= 1.2;

                imageControl.Margin = new Thickness(-140, 10 ,10, 10);
                st1.Margin = new Thickness(0, 0, 180, 18);
                vid.Margin = new Thickness(-120, 10, 50, 10);
                vid.Width += 400;
                vid.Height += 50;
                light.Margin = new Thickness(-150, 0, 0, 0);
                tex.Margin = new Thickness(-150, 0, 0, 0);
                cart.Margin = new Thickness(0, 0, 820, 0);
            }
            else if(w.WindowState == WindowState.Normal) {
                imageControl.Width = 580;
                imageControl.Height = 326;

                st1.Margin = new Thickness(0, 0, 32, 18);
                imageControl.Margin = new Thickness(10, 10, 10, 10);

                vid.Margin = new Thickness(-10, 10, 10, 10);
                vid.Width = 317;
                vid.Height = 150;
                light.Margin = new Thickness(0, 0, 0, 0);
                tex.Margin = new Thickness(0, 0, 0, 0);
                cart.Margin = new Thickness(0, 0, 470, 0);

            }
        }
    }
}
