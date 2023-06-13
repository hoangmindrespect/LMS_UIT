using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using LibraryManagementSystem.View.MainWindow.BorrowBook;
using LibraryManagementSystem.View.MessageBoxCus;
using LibraryManagementSystem.ViewModel.ClientVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<BookDTO> Books;
        public static ObservableCollection<BookInBorrowDTO> ListBookBorrow;
        public static ObservableCollection<BookBorrowForm> ListReturnBook;

        private string _colorBack;
        public string ColorBack
        {
            get { return _colorBack; }
            set { _colorBack = value; OnPropertyChanged(); }
        }
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
        public ICommand LoadBorrowBookPage { get; set; }
        public ICommand LoadReturnBookPage { get; set; }
        public ICommand LoadReturnBook { get; set; }
        public ICommand ReturnBook { get; set; }
        #endregion

        #region TempVar
        #endregion
        public BorrowBookViewModel()
        {
            // first load

            LoadBook = new RelayCommand<ItemsControl>((p) => { return p != null; }, (p) =>
            {
                Books = new ObservableCollection<BookDTO>();
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
                        if(item.NAMXUATBAN != null)
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
                     try
                     {
                         if (!string.IsNullOrEmpty(ID))
                         {
                             Name = (from s in context.ACCOUNTs where s.MSSV == ID select s.FULLNAME).FirstOrDefault();
                             IDClass = (from s in context.ACCOUNTs where s.MSSV == ID select s.IDCLASS).FirstOrDefault();

                             StartDay = DateTime.Now.ToShortDateString();
                             DueDay = DateTime.Now.AddDays(30).ToShortDateString();

                         }
                     }
                     catch
                     {
                         MessageBoxLMS msb = new MessageBoxLMS("Notice", "Can not find any studen with this ID", MessageType.Accept, MessageButtons.OK);
                         msb.ShowDialog();
                     }
                 }
             });

            LoadBookBorrow = new RelayCommand<ListView>((p) => { return true; }, (p) =>
            {
                ListBookBorrow = new ObservableCollection<BookInBorrowDTO>();
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
                        if(item.SoLuong == 1)
                        {
                            ListBookBorrow.Remove(item);
                            return;
                        }    
                        else
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
                            Form.MSSV = ID;
                            Form.TENKH = Name;
                            Form.NGAYMUON = DateTime.Now;
                            Form.NGAYHETHAN = DateTime.Now.AddDays(30);
                            context.BBFORMs.Add(Form);
                            context.SaveChanges();

                            foreach (var book in ListBookBorrow)
                            {
                                DETAIL_BBFORM a = new DETAIL_BBFORM();
                                a.MAPHIEUMUON = Form.MAPHIEUMUON;
                                a.MASACH = book.MaSach;
                                a.TENSACH = book.TenSach;
                                a.SOLUONG = book.SoLuong;
                                MinusBook(book.MaSach, book.SoLuong);
                                context.DETAIL_BBFORM.Add(a);
                            }
                            context.SaveChanges();
                            ListBookBorrow.Clear();
                            ID = IDClass = Name = StartDay = DueDay = null;
                            MessageBoxLMS msb = new MessageBoxLMS("Notice", "Create successfully!", MessageType.Accept, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Error", "Please choose at least 1 book to borrow", MessageType.Accept, MessageButtons.OK);
                        msb.ShowDialog();
                    }
                }
                else
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Error", " Please fill into field ID!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
                }
            });

            LoadBorrowBookPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new BorrowBookPage();
            });

            LoadReturnBookPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ReturnBookPage();
            });

            LoadReturnBook = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                ListReturnBook = new ObservableCollection<BookBorrowForm>();
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.BBFORMs)
                    {
                        BookBorrowForm a = new BookBorrowForm();
                        a.ID = item.MAPHIEUMUON.ToString();
                        a.IDCus = (int)item.MAKH;
                        a.Name = item.TENKH;
                        a.MSSV = item.MSSV;
                        a.DayStart = item.NGAYMUON.ToShortDateString();
                        a.DayEnd = ((DateTime)item.NGAYHETHAN).ToShortDateString();
                        a.list = new ObservableCollection<DetailBookBorrowForm>();
                        if (item.NGAYHETHAN > item.NGAYMUON)
                        {
                            a.Status = "Being borrowed";
                            a.ColorBack = "#E1E1E2";
                        }
                        else
                        {
                            a.Status = "Out of date";
                            a.ColorBack = "#E89895";
                        }
                        foreach (var item2 in context.DETAIL_BBFORM)
                        {
                            if(item2.MAPHIEUMUON.ToString() == a.ID)
                            {
                                DetailBookBorrowForm b = new DetailBookBorrowForm();
                                b.IDBook = item2.MASACH;
                                b.ID = item2.MAPHIEUMUON;
                                b.Title = item2.TENSACH;
                                b.Count = (int)item2.SOLUONG;
                                a.list.Add(b);
                            }    
                        }

                        ListReturnBook.Add(a);
                    }
                }

                p.ItemsSource = ListReturnBook;
            });

            ReturnBook = new RelayCommand<string>((p) => { return true; }, (p) =>
            {
                //Return book into library and delete form in database
                using(var context = new LMSEntities1())
                {
                    foreach(var item in context.DETAIL_BBFORM)
                    {
                        if(p == item.MAPHIEUMUON.ToString())
                        {
                            PlusBook(item.MASACH, (int)item.SOLUONG);
                            context.DETAIL_BBFORM.Remove(item);
                        }    
                    }

                    foreach (var item in context.BBFORMs)
                    {
                        if (p == item.MAPHIEUMUON.ToString())
                        {
                           context.BBFORMs.Remove(item);
                        }
                    }
                    context.SaveChanges();
                }    

                //Remove this form from ListReturnBook
                foreach (var item in ListReturnBook)
                {
                    if(item.ID == p)
                    {
                        ListReturnBook.Remove(item);
                        break;
                    }    
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
        
        // trừ đi số lượng sách đã mượn
        void MinusBook(int id, int count)
        {
            using (var context = new LMSEntities1())
            {
                foreach(var item in context.BOOKs)
                {
                    if(item.ID == id)
                    {
                        item.SOLUONG -= count;
                    }
                }
                context.SaveChanges();
            }
        }
        void PlusBook(int id, int count)
        {
            using (var context = new LMSEntities1())
            {
                foreach (var item in context.BOOKs)
                {
                    if (item.ID == id)
                    {
                        item.SOLUONG += count;
                    }
                }
                context.SaveChanges();
            }
        }
        string GetNameBook(int id)
        {
            using(var context = new LMSEntities1())
            {
                foreach(var  item in context.BOOKs)
                {
                    if (id == item.ID)
                        return item.TENSACH;
                }    
            }
            return null;
        }


    }
}
