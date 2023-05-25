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
            tg.Text = book.TacGia;
            tl.Text = book.TheLoai;
            nhaxb.Text = book.NXB;
            namxb.Text = book.NamXB.ToString();
            sl.Text = book.SoLuong.ToString();
            gia.Text = book.Gia.ToString();
            img.Source = new BitmapImage(new Uri(book.ImageSource, UriKind.RelativeOrAbsolute));

        }
    }
}
