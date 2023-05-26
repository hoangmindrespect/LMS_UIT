using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using LibraryManagementSystem.View.MessageBoxCus;
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
        public static ObservableCollection<BookInBorrowDTO> ListBookBorrow = new ObservableCollection<BookInBorrowDTO>();
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
        public ICommand PlusOne {  get; set; }
        public ICommand MinusOne { get; set; }
        public ICommand CreateBookOrder { get; set; }
        #endregion

        #region TempVar
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
                        book.MaSach = (int)item.ID;
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
                p.ItemsSource = ListBookBorrow;
            });

            PlusOne = new RelayCommand<int>((p) => { return true; }, (p) =>
            {
                foreach (var item in ListBookBorrow)
                {
                    if (p == item.MaSach)
                    {
                        if(item.SoLuong + 1 > getMaxCount(p))
                        {
                            MessageBoxLMS msb = new MessageBoxLMS("Notice", "Exceed the max count", MessageType.Accept, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                        else
                            item.SoLuong += 1;
                    }
                        
                }
            });

            MinusOne = new RelayCommand<int>((p) => { return true; }, (p) =>
            {
                foreach (var item in ListBookBorrow)
                {
                    if (p == item.MaSach)
                    {
                        item.SoLuong -= 1;
                    }

                }
            });

            CreateBookOrder = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                //tạo phiếu mượn => tạo chi tiết phiếu mượn 
                if(!string.IsNullOrEmpty(ID) )
                {
                    if (p.Items.Count > 0)
                    {
                        using (var context = new LMSEntities1())
                        {
                            var Form = new BBFORM();
                            Form.MAKH = GetMAKH(ID);
                            Form.NGAYMUON = DateTime.Now;
                            Form.NGAYHETHAN = DateTime.Now.AddDays(30);
                            context.BBFORMs.Add(Form);
                            context.SaveChanges();

                            foreach (var book in ListBookBorrow)
                            {
                                DETAIL_BBFORM a = new DETAIL_BBFORM();
                                a.MAPHIEUMUON = Form.MAPHIEUMUON;
                                a.MASACH = book.MaSach;
                                a.SOLUONG = book.SoLuong;
                                context.DETAIL_BBFORM.Add(a);
                            }
                            context.SaveChanges();
                            MessageBoxLMS msb = new MessageBoxLMS("Notice", "Create successfully!", MessageType.Accept, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Error", "Please fill into field ID!", MessageType.Accept, MessageButtons.OK);
                        msb.ShowDialog();
                    }
                }
                else
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Error", "Choose any book you want!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
                }
            });
        }

        int getMaxCount(int id)
        {
            using (var context = new LMSEntities1())
            {
                return (int)(from s in context.BOOKs where s.ID == id select s.SOLUONG).FirstOrDefault();
            }
        }

        int GetMAKH(string MSSV)
        {
            using (var context = new LMSEntities1())
            {
                return (int)(from s in context.ACCOUNTs where MSSV == s.MSSV select s.ID).FirstOrDefault();
            }    
        }
    }
}
