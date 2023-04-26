using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MainWindow.ManageBook;
using LibraryManagementSystem.View.MessageBoxCus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LibraryManagementSystem.ViewModel.AdminVM.ManageBookVM
{
    public class ManageBookViewModel:BaseViewModel
    {

        #region Property
        private ObservableCollection<BookDTO> _listbookmanage;
        public ObservableCollection<BookDTO> Listbookmanage
        {
            get { return _listbookmanage; }
            set { _listbookmanage = value; OnPropertyChanged(); }
        }

        private string _masach;
        public string MaSach
        {
            get { return _masach; }
            set { _masach = value; OnPropertyChanged(); }
        }

        private string _tensach;
        public string TenSach
        {
            get { return _tensach; }
            set { _tensach = value; OnPropertyChanged(); }
        }

        private string _tacgia;
        public string TacGia
        {
            get { return _tacgia; }
            set { _tacgia = value; OnPropertyChanged(); }
        }

        private string _nhaxuatban;
        public string NhaXuatBan
        {
            get { return _nhaxuatban; }
            set { _nhaxuatban = value; OnPropertyChanged(); }
        }

        private string _namxuatban;
        public string NamXuatBan
        {
            get { return _namxuatban; }
            set { _namxuatban = value; OnPropertyChanged(); }
        }

        private string _soluong;
        public string SoLuong
        {
            get { return _soluong; }
            set { _soluong = value; OnPropertyChanged(); }
        }

        private string _gia;
        public string Gia
        {
            get { return _gia; }
            set { _gia = value; OnPropertyChanged(); }
        }

        private string _theloai;
        public string TheLoai
        {
            get { return _theloai; }
            set { _theloai = value; OnPropertyChanged(); }
        }

        private string _mota;
        public string MoTa
        {
            get { return _mota; }
            set { _mota = value; OnPropertyChanged(); }
        }

        private string _imgsource;
        public string ImgSource
        {
            get { return _imgsource; }
            set { _imgsource = value; OnPropertyChanged(); }
        }

        private bool _isIncomplete;
        public bool IsIncomplete
        {
            get { return _isIncomplete; }
            set { _isIncomplete = value; OnPropertyChanged(); }
        }

        private bool _isNull;
        public bool IsNull
        {
            get { return _isNull; }
            set { _isNull = value; OnPropertyChanged(); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }
        #endregion

        #region Icommand
        public ICommand LoadManageBookData { get; set; }
        public ICommand Updating { get; set; }
        public ICommand DeletingBook { get; set; }
        public ICommand EditingBook { get; set; }
        public ICommand ImportImageEditWindow { get; set; }
        #endregion

        EdittingBookWindow window;
        public ManageBookViewModel()
        {
            LoadManageBookData = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                Loaded(p);
            });

            //Button delete in manage book page
            DeletingBook = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                BookDTO item = p.Items[p.SelectedIndex] as BookDTO;
                string masach = item.MaSach.ToString();
                using (var context = new LMSEntities())
                {
                    string connectionStr = context.Database.Connection.ConnectionString;
                    SqlConnection connect = new SqlConnection(connectionStr);
                    connect.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connect;
                    command.Parameters.AddWithValue("@masach", masach);
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Delete this book?", MessageType.Waitting, MessageButtons.YesNo);

                    if (msb.ShowDialog() == true)
                    {
                        try
                        {
                            command.CommandText = "delete from BOOK where ID = @masach";
                            context.SaveChanges();
                            if (command.ExecuteNonQuery() != 0)
                            {
                                msb = new MessageBoxLMS("Notification", "Delete successful", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                                Loaded(p);
                            }
                        }
                        catch
                        {
                            msb = new MessageBoxLMS("Notification", "Cannot delete this book", MessageType.Error, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                }
            });


            // Button edit in book manage page
            EditingBook = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                BookDTO item = p.Items[p.SelectedIndex] as BookDTO;
                var masach = item.MaSach.ToString();
                EdittingBookWindow window = new EdittingBookWindow(masach);

                SoLuong = item.SoLuong.ToString();
                TenSach = item.TenSach.ToString();
                TacGia = item.TacGia.ToString();
                NhaXuatBan = item.NXB;
                NamXuatBan = item.NamXB.ToString();
                Gia = item.Gia.ToString();
                MoTa = item.MoTa.ToString();
                TheLoai = item.TheLoai.ToString();

                if (string.IsNullOrEmpty(item.ImageSource)) { }
                else
                {
                    ImgSource = item.ImageSource.ToString();
                    //Load ảnh hiện tai lên trang chỉnh sửa
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(item.ImageSource);
                    img.EndInit();
                    window.image_img.Source = img;
                }
                window.ShowDialog();

                //load lại trang quản lý sách
                Loaded(p);
            });

            ImportImageEditWindow = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Filter = "JPG File (.jpg)|*.jpg";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(dlg.FileName);
                    img.EndInit();
                    EdittingBookWindow.Image.Source = img;

                    Account account = new Account(
                    "djwef41is",
                    "839823714638816",
                    "06sLWSKM71X4YJFIrSeWG-1TuMk");

                    Cloudinary cloudinary = new Cloudinary(account);
                    cloudinary.Api.Secure = true;
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(dlg.FileName),
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    ImgSource = uploadResult.Url.ToString();
                    IsLoading = false;
                }
            });

            Updating = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                string ID = EdittingBookWindow.masach;
                using (var context = new LMSEntities())
                {
                    foreach(var book in context.BOOKs)
                    {
                        if(book.ID == int.Parse(ID))
                        {
                            try
                            {
                                book.TENSACH = TenSach;
                                book.TACGIA = TacGia;
                                book.NHAXUATBAN = NhaXuatBan;
                                book.NAMXUATBAN = int.Parse(NamXuatBan);
                                book.MOTA = MoTa;
                                book.THELOAI = TheLoai;
                                book.GIA = decimal.Parse(Gia);
                                book.IMAGESOURCE = ImgSource;
                                book.SOLUONG = int.Parse(SoLuong);

                                MessageBoxLMS msb = new MessageBoxLMS("Notification", "Edit book successfull", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                                break;
                            }
                            catch
                            {
                                MessageBoxLMS msb = new MessageBoxLMS("Warning", "Cannot connect to server", MessageType.Error, MessageButtons.OK);
                                msb.ShowDialog();
                                break;
                            }
                        }    
                    }
                    context.SaveChanges();

                }
            });
        }
        public void Loaded(DataGrid p)
        {
            Listbookmanage = new ObservableCollection<BookDTO>();
            using (var context = new LMSEntities())
            {
                foreach (var item in context.BOOKs)
                {

                    BookDTO book = new BookDTO();
                    book.MaSach = item.ID;
                    book.TenSach = item.TENSACH;
                    book.TacGia = item.TACGIA;
                    book.SoLuong = (int)item.SOLUONG;
                    book.Gia = (int)item.GIA;
                    book.NXB = item.NHAXUATBAN;
                    book.NamXB = (int)item.NAMXUATBAN;
                    book.TheLoai = item.THELOAI;
                    book.MoTa = item.MOTA;
                    book.ImageSource = item.IMAGESOURCE;
                    Listbookmanage.Add(book);
                }
            }            
        }
    }
}
