using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel.ClientVM.BuyBookVM
{
    public class BuyBookViewModel:BaseViewModel
    {
        public ObservableCollection<BookDTO> Books = new ObservableCollection<BookDTO>();

        public ICommand LoadBuyBookPage { get; set; }
        public ICommand LoadBook { get; set; }
        public BuyBookViewModel()
        {
            LoadBuyBookPage = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                p.Content = new BuyBookPage();
            });

            LoadBook = new RelayCommand<ItemsControl>((p) => { return p != null; }, (p) =>
            {
                using (var context = new LMSEntities())
                {
                    
                    foreach (var item in context.BOOKs)
                    {
                        BookDTO book = new BookDTO();
                        book.TenSach = item.TENSACH;
                        book.TacGia = item.TACGIA;
                        book.NXB = item.NHAXUATBAN;
                        book.SoLuong = (int)item.SOLUONG;
                        book.Gia = (decimal)item.GIA;
                        book.ImageSource = item.IMAGESOURCE;
                        book.NamXB = (int)item.NAMXUATBAN;
                        Books.Add(book);
                    }
                }

                p.ItemsSource = Books;
            });
        }
    }
}
