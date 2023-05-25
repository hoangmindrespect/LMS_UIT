using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using LibraryManagementSystem.ViewModel.ClientVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel.AdminVM.BorrowBookVM
{
    public  class BorrowBookViewModel:BaseViewModel
    {
        #region Property
        public ObservableCollection<BookDTO> Books = new ObservableCollection<BookDTO>();
        public ObservableCollection<BookInBorrowDTO> ListBookBorrow = new ObservableCollection<BookInBorrowDTO>();
        private string _iD;
        public string ID
        {
            get { return _iD; }
            set { _iD = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private string _idClass;
        public string IDClass
        {
            get { return _idClass; }
            set { _idClass = value; OnPropertyChanged(); }
        }

        private string _startDay;
        public string StartDay
        {
            get { return _startDay; }
            set { _startDay = value; OnPropertyChanged();}
        }

        private string _dueDay;
        public string DueDay
        {
            get { return _dueDay; }
            set { _dueDay = value;OnPropertyChanged();}
        }
        #endregion

        #region Command
        public ICommand LoadBook { get; set; }
        public ICommand CheckID { get; set; }
        public ICommand LoadBookBorrow { get; set; }
        #endregion
        public BorrowBookViewModel()
        {
            // first load

            LoadBook = new RelayCommand<ItemsControl>((p) => { return p != null; }, (p) =>
            {
                using (var context = new LMSEntities1())
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
                        Books.Add(book);                    }
                }

                p.ItemsSource = Books;                
            });

            CheckID = new RelayCommand<Frame>((p) => { return true; }, (p) =>
             {
                 using (var context = new LMSEntities1())
                 {
                     if (!string.IsNullOrEmpty(ID))
                     {
                         Name = (from s in context.ACCOUNTs where s.MSSV == ID select s.FULLNAME).FirstOrDefault();
                         IDClass = (from s in context.ACCOUNTs where s.MSSV == ID select s.IDCLASS).FirstOrDefault();
                     }
                 }

                 StartDay = DateTime.Now.ToShortDateString();
                 DueDay = DateTime.Now.AddDays(30).ToShortDateString();
             });

            LoadBookBorrow = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                using (var context = new LMSEntities1())
                {

                    foreach (var item in context.BOOKs)
                    {
                        BookInBorrowDTO b = new BookInBorrowDTO();
                        b.TenSach = item.TENSACH;
                        b.SoLuong = 1;
                        ListBookBorrow.Add(b);
                    }
                }

                p.ItemsSource = ListBookBorrow;
            });
        }
    }
}
