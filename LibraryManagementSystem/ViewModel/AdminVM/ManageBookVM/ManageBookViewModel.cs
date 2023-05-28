using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MainWindow.ManageBook;
using LibraryManagementSystem.View.MessageBoxCus;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
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
        public ICommand DeleteBookList { get; set; }
        public ICommand EditingBook { get; set; }
        public ICommand ImportImageEditWindow { get; set; }
        public ICommand ExportToExcel { get; set; }
        public ICommand ImportFromExcel { get; set; }
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
                using (var context = new LMSEntities1())
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
                TheLoai = null;
                

                if (string.IsNullOrEmpty(item.ImageSource))
                {
                    ImgSource = null;
                    TheLoai = item.TheLoai;
                }
                else
                {
                    TheLoai = item.TheLoai;
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
                using (var context = new LMSEntities1())
                {
                    foreach(var book in context.BOOKs)
                    {
                        if(book.ID == int.Parse(ID))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(TenSach) && !string.IsNullOrEmpty(TacGia) && !string.IsNullOrEmpty(NhaXuatBan) && !string.IsNullOrEmpty(NamXuatBan) && !string.IsNullOrEmpty(MoTa) && !string.IsNullOrEmpty(MoTa) && !string.IsNullOrEmpty(Gia) && !string.IsNullOrEmpty(ImgSource) && !string.IsNullOrEmpty(SoLuong))
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
                                else
                                {
                                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Some fields still empty", MessageType.Accept, MessageButtons.OK);
                                    msb.ShowDialog();
                                }    
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
                p.Close();
            });
            ExportToExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                string filePath = "";
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Excel|*.xlsx;*.xls";

                if (dialog.ShowDialog() == true)
                    filePath = dialog.FileName;

                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Duong dan sai");
                    return;
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                try
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("List of Book");
                        ExcelWorksheet ws = excel.Workbook.Worksheets[0];
                        ws.Name = "List of Book";
                        ws.Cells.Style.Font.Size = 11;
                        ws.Cells.Style.Font.Name = "Times New Roman";
                        string[] headerColumns = { "ID", "Book title", "Author's name", "Publisher", "Publication Year", "Genre", "Price", "Count" };

                        int numOfColumns = headerColumns.Count();
                        ws.Cells[1, 1].Value = "List of book - LMS Library";
                        ws.Cells[1, 1, 1, numOfColumns].Merge = true;
                        ws.Cells[1, 1, 1, numOfColumns].Style.Font.Bold = true;
                        int colIndex = 1;
                        int rowIndex = 2;

                        //Tao các header trong excel
                        foreach (string item in headerColumns)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];
                            //Set màu dòng header thành LightBlue
                            var fill = cell.Style.Fill;
                            fill.PatternType = ExcelFillStyle.Solid;
                            fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                            //Chỉnh border
                            var border = cell.Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell.Value = item;
                            colIndex++;

                        }

                        //Thêm dữ liệu vào sheet
                        foreach (BookDTO user in Listbookmanage)
                        {
                            colIndex = 1;
                            rowIndex++;
                            ws.Cells[rowIndex, colIndex++].Value = user.MaSach;
                            ws.Cells[rowIndex, colIndex++].Value = user.TenSach;
                            ws.Cells[rowIndex, colIndex++].Value = user.TacGia;
                            ws.Cells[rowIndex, colIndex++].Value = user.NXB;
                            ws.Cells[rowIndex, colIndex++].Value = user.NamXB;
                            ws.Cells[rowIndex, colIndex++].Value = user.TheLoai;
                            ws.Cells[rowIndex, colIndex++].Value = user.Gia;
                            ws.Cells[rowIndex, colIndex++].Value = user.SoLuong;
                        }
                        for (int i = 1; i < headerColumns.Length; i++)
                            ws.Column(i).Width = ws.Cells[3, i].Value.ToString().Length * 1.2 + 5;

                        ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        ws.Cells[1, 1, 2, headerColumns.Length].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //Save
                        Byte[] bin = excel.GetAsByteArray();
                        File.WriteAllBytes(filePath, bin);
                    }
                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Export to Excel is successful!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();

                }
                catch (Exception e)
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Error - The file you selected maybe open.", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }

            });

            ImportFromExcel = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                string filePath = "";
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Excel|*.xlsx;*.xls";
                if (dialog.ShowDialog() == true)
                    filePath = dialog.FileName;

                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Duong dan sai");
                    return;
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                try
                {
                    using (ExcelPackage excel = new ExcelPackage(new FileInfo(dialog.FileName)))
                    {
                        ExcelWorksheet ws = excel.Workbook.Worksheets[0];
                        int numOfRow = ws.Dimension.Rows;
                        using (var context = new LMSEntities1())
                        {
                            for (int i = 2; i <= numOfRow; i++)
                            {
                                //int id = Int32.Parse(ws.Cells[i, 1].Value.ToString());
                                string bookTitle = ws.Cells[i, 1].Value.ToString();
                                string authorName = ws.Cells[i, 2].Value.ToString();
                                string publisher = ws.Cells[i, 3].Value.ToString();
                                int publicationYear = Int32.Parse(ws.Cells[i, 4].Value.ToString());
                                string genre = ws.Cells[i, 5].Value.ToString();
                                decimal price = decimal.Parse(ws.Cells[i, 6].Value.ToString());
                                int count = Int32.Parse(ws.Cells[i, 7].Value.ToString());

                                BOOK newBook = new BOOK();
                                newBook.TENSACH = bookTitle;
                                newBook.TACGIA = authorName;
                                newBook.NHAXUATBAN = publisher;
                                newBook.NAMXUATBAN = publicationYear;
                                newBook.THELOAI = genre;
                                newBook.GIA = price;
                                newBook.SOLUONG = count;
                                context.BOOKs.Add(newBook);
                            }
                            context.SaveChanges();
                            Loaded(p);
                        }
                    }
                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Successful import!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Error - Cannot import data from the selected file.", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }
            });

            DeleteBookList = new RelayCommand<DataGrid>(
            (p) =>
            {
                if(p != null)
                {
                    if (p.SelectedItems.Count > 1)
                        return true;
                    return false;
                }
                return false;
            }, (p) =>
            {
                List<string> bookList = new List<string>();
                foreach (var item in p.SelectedItems)
                    bookList.Add((item as BookDTO).MaSach.ToString());
                using (var context = new LMSEntities1())
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    string idList = string.Join(", ", bookList);
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Delete this list of book?", MessageType.Waitting, MessageButtons.YesNo);
                    if (msb.ShowDialog() == true)
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@idList", idList);
                            command.CommandText = "delete from BOOK where ID in ( " + @idList + " )";
                            context.SaveChanges();
                            if (command.ExecuteNonQuery() != 0)
                            {
                                msb = new MessageBoxLMS("Notification", "Deleting is successful", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                                Loaded(p);
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                            msb = new MessageBoxLMS("Notification", "Cannot delete this list of book", MessageType.Error, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                }
            });
        }
        public void Loaded(DataGrid p)
        {
            Listbookmanage = new ObservableCollection<BookDTO>();
            using (var context = new LMSEntities1())
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
