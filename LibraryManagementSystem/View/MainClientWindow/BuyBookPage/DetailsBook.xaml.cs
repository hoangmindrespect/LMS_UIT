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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryManagementSystem.View.MainClientWindow.BuyBookPage
{
    /// <summary>
    /// Interaction logic for DetailsBook.xaml
    /// </summary>
    public partial class DetailsBook : Window
    {
        public DetailsBook(BookDTO book)
        {
            InitializeComponent();
            if (book.TacGia != null)
                tg.Text = book.TacGia;
            if (book.TheLoai != null)
                tl.Text = book.TheLoai;
            if (book.NXB != null)
                nhaxb.Text = book.NXB;
            if (book.NamXB != null)
                namxb.Text = book.NamXB.ToString();
            if (book.SoLuong != null)
                sl.Text = book.SoLuong.ToString();
            gia.Text =decimal.Round(book.Gia, 0).ToString().Replace('$', '₫');
            if (book.MoTa != null)
                mt.Text = book.MoTa.ToString();
            //if(!string.IsNullOrEmpty(book.MoTa))
            //    mt.Text = book.MoTa.ToString();
            //tl.Text = book.TheLoai.ToString();
            if (book.ImageSource != null)
                img.Source = new BitmapImage(new Uri(book.ImageSource, UriKind.RelativeOrAbsolute));

        }
    }
}
