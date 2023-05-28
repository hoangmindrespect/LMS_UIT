using LibraryManagementSystem.View.MessageBoxCus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using LibraryManagementSystem.View.MainWindow.ImportBook;
using System.Collections.ObjectModel;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.ViewModel.LoginVM;
using LibraryManagementSystem.Models.DataProvider;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;

namespace LibraryManagementSystem.ViewModel.AdminVM.ImportBookVM
{

    public partial class ImportViewModel : BaseViewModel
    {
        #region Property
        private string _tenMatHang;
        public string TenMatHang
        {
            get { return _tenMatHang; }
            set { _tenMatHang = value; OnPropertyChanged(); }
        }

        private string _tacGia;
        public string TacGia
        {
            get { return _tacGia; }
            set { _tacGia = value; OnPropertyChanged(); }
        }

        private string _nhaXuatBan;
        public string NhaXuatBan
        {
            get { return _nhaXuatBan; }
            set { _nhaXuatBan = value; OnPropertyChanged(); }
        }

        private string _soLuong;
        public string SoLuong
        {
            get { return _soLuong; }
            set { _soLuong = value; OnPropertyChanged(); }
        }

        private string _giaNhap;
        public string GiaNhap
        {
            get { return _giaNhap; }
            set { _giaNhap = value; OnPropertyChanged(); }
        }

        private string _giaBan;
        public string GiaBan
        {
            get { return _giaBan; }
            set { _giaBan = value; OnPropertyChanged(); }
        }

        private string _ngayNhap;
        public string NgayNhap
        {
            get { return _ngayNhap; }
            set { _ngayNhap = value; OnPropertyChanged(); }
        }

        private int _triGiaHoaDon;
        public int TriGiaHoaDon
        {
            get { return _triGiaHoaDon; }
            set { _triGiaHoaDon = value; OnPropertyChanged(); }
        }

        private string _triGiaChu;
        public string TriGiaChu
        {
            get { return _triGiaChu; }
            set { _triGiaChu = value; OnPropertyChanged(); }
        }
        private int _idAdmin;
        public int IdAdmin
        {
            get { return _idAdmin; }
            set { _idAdmin = value; OnPropertyChanged(); }
        }    

        private string _nameAdmin;
        public string NameAdmin
        {
            get { return _nameAdmin; }
            set { _nameAdmin = value; OnPropertyChanged(); }
        }

        private string _toDay;
        public string ToDay
        {
            get { return _toDay; }
            set { _toDay = value; OnPropertyChanged(); }
        }

        private string _intoText;
        public string IntoText
        {
            get { return _intoText; }
            set { _intoText = value; OnPropertyChanged(); }
        }
        #endregion
        public ICommand OpenAddingWindow {get; set;}
        public ICommand AddBookToImportDTG { get; set; }
        public ICommand CreateOrder { get; set; }
        public ICommand DeleteBookDTG { get; set; }
        public ICommand EditBookDTG { get; set; }
        public ICommand CancelEditBookDTG { get; set; }
        public ICommand CommitEditBookDTG { get; set; }
        public ICommand Loaded { get; set; }
        public ImportViewModel()
        {
            OpenAddingWindow = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                AddingWindow w = new AddingWindow();
                w.Show();
            });
            Loaded = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                p.ItemsSource = LoginRegisViewModel.listbook;
                LoginRegisViewModel.import_dtg = p;

                ToDay = DateTime.Now.ToShortDateString();
                using(var context = new  LMSEntities1())
                {
                    NameAdmin = (from s in context.ACCOUNTs where LoginRegisViewModel.username == s.USERNAME select s.FULLNAME).FirstOrDefault();
                    IdAdmin = (from s in context.ACCOUNTs where LoginRegisViewModel.username == s.USERNAME select s.ID).FirstOrDefault();
                }    
            });

            //Thêm mặt hàng vào ds nhập
            AddBookToImportDTG = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (TenMatHang != null && NhaXuatBan != null && TacGia != null && GiaNhap != null && GiaBan != null && SoLuong != null)
                {
                    if ((TriGiaHoaDon + int.Parse(SoLuong) * int.Parse(GiaNhap)) > 1000000000)
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Error", "Your order cost must be under 1 billion", MessageType.Error, MessageButtons.OK);
                        msb.ShowDialog();
                    }
                    else
                    {
                        LoginRegisViewModel.listbook.Add(new ImportBook{ TenSach = TenMatHang, SoLuong = int.Parse(SoLuong), GiaNhap = int.Parse(GiaNhap), GiaBan = int.Parse(GiaBan), NhaXuatBan = NhaXuatBan, TacGia = TacGia });
                        TriGiaHoaDon += int.Parse(SoLuong) * int.Parse(GiaNhap);
                        IntoText = So_chu(TriGiaHoaDon);
                        TenMatHang = SoLuong = GiaNhap = GiaBan = TacGia = NhaXuatBan = null;
                        p.Close();
                    }
                }
                else
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Error", "Some fields are empty", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }
            });

            //Xóa mặt hàng trong danh sách nhập
            DeleteBookDTG = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                ImportBook book = LoginRegisViewModel.import_dtg.SelectedItems[0] as ImportBook;
                LoginRegisViewModel.listbook.Remove(book);
                CapNhatTriGia();
                IntoText = So_chu(TriGiaHoaDon);
            });

            //Sửa mặt hàng trong danh sách nhập
            EditBookDTG = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                DataGridRow row = (DataGridRow)LoginRegisViewModel.import_dtg.ItemContainerGenerator.ContainerFromItem(LoginRegisViewModel.import_dtg.CurrentItem);
                ShowCellsEditingTemplate(row);
            });

            // Đồng ý thay đổi mặt hàng
            CommitEditBookDTG = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                DataGridRow row = (DataGridRow)LoginRegisViewModel.import_dtg.ItemContainerGenerator.ContainerFromItem(LoginRegisViewModel.import_dtg.CurrentItem);
                ShowCellsNormalTemplate(row, true);
                CapNhatTriGia();
            });

            //Hủy thay đổi mặt hàng
            CancelEditBookDTG = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                DataGridRow row = (DataGridRow)LoginRegisViewModel.import_dtg.ItemContainerGenerator.ContainerFromItem(LoginRegisViewModel.import_dtg.CurrentItem);
                ShowCellsNormalTemplate(row);
            });

            //Lập phiếu nhập hàng
            CreateOrder = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                
                using(var context = new LMSEntities1())
                {
                    // đầu tiên là tạo phiếu nhập hàng trước
                    IMPORT a = new IMPORT();
                    a.MANV = IdAdmin;
                    a.TRIGIA = TriGiaHoaDon;
                    a.NGNHAP = DateTime.Now;
                    context.IMPORTs.Add(a);
                    context.SaveChanges();

                    // Tiếp theo là tạo chi tiết từng phiếu nhập ha
                    foreach(var item in LoginRegisViewModel.listbook)
                    {
                        //thêm thong tin vào chi tiết phiếu
                        DETAIIMPORT b = new DETAIIMPORT();
                        b.SOHD = a.SOHD;
                        b.TENSACH = item.TenSach;
                        b.NXB = item.NhaXuatBan;
                        b.TACGIA = item.TacGia;
                        b.SL = item.SoLuong;
                        b.GIANHAP = item.GiaNhap;
                        b.GIABAN = item.GiaBan;
                        context.DETAIIMPORTs.Add(b);

                        // đồng thời tạo sách với thông tin không đầy đủ trong tràng quản lý
                        // kiểm tra xem nếu tên sách, nhà xuất bản, năm xuất bản đã có => cộng só lượng vào, ngược lại thì tạo sách mới
                        BOOK c = new BOOK();
                        c.TENSACH = item.TenSach;
                        c.NHAXUATBAN = item.NhaXuatBan;
                        c.TACGIA = item.TacGia;
                        c.SOLUONG = item.SoLuong;
                        c.GIA = item.GiaBan;
                        context.BOOKs.Add(c);

                    }    
                    context.SaveChanges();
                    NameAdmin = ToDay = IntoText = null; TriGiaHoaDon = 0;
                    LoginRegisViewModel.listbook = null;
                }
                MessageBoxLMS msb = new MessageBoxLMS("Notification", "Import successfully", MessageType.Accept, MessageButtons.OK);
                msb.ShowDialog();

            });
        }

        #region Custom Datagrid
        private void ShowCellsEditingTemplate(DataGridRow row)
        {
            foreach (DataGridColumn col in LoginRegisViewModel.import_dtg.Columns)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(col.GetCellContent(row));
                while (parent.GetType().Name != "DataGridCell")
                    parent = VisualTreeHelper.GetParent(parent);

                DataGridCell cell = ((DataGridCell)parent);
                DataGridTemplateColumn c = (DataGridTemplateColumn)col;
                if (c.CellEditingTemplate != null)
                    cell.Content = ((DataGridTemplateColumn)col).CellEditingTemplate.LoadContent();
            }
        }

        private void ShowCellsNormalTemplate(DataGridRow row, bool canCommit = false)
        {
            foreach (DataGridColumn col in LoginRegisViewModel.import_dtg.Columns)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(col.GetCellContent(row));
                while (parent.GetType().Name != "DataGridCell")
                    parent = VisualTreeHelper.GetParent(parent);

                DataGridCell cell = ((DataGridCell)parent);
                DataGridTemplateColumn c = (DataGridTemplateColumn)col;
                if (col.DisplayIndex != 0)
                {
                    if (canCommit == true)
                    {
                        TextBox textBox = cell.Content as TextBox;
                        if (textBox != null)
                        {
                            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                        }
                    }
                    else
                    {
                        TextBox textBox = cell.Content as TextBox;
                        if (textBox != null)
                        {
                            textBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                        }
                    }
                }
                cell.Content = c.CellTemplate.LoadContent();
            }
        }
        #endregion

        #region Cập nhật các thông số trong datagrid
        private void CapNhatTriGia()
        {
            TriGiaHoaDon = 0;
            foreach (var item in LoginRegisViewModel.listbook)
            {
                if ((TriGiaHoaDon) > 1000000000)
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Error", "Your order cost must be under 1  billion", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }
                else
                {
                    TriGiaHoaDon += item.GiaNhap * item.SoLuong;
                    IntoText = So_chu(TriGiaHoaDon);
                }
            }
        }
        #endregion

        #region Đổi từ số sang chữ
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }
        public static string So_chu(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng.";

            return lso_chu.ToString().Trim();

        }
        #endregion
    }
}

