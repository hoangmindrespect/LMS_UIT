using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MainWindow.ManageUser;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel.AdminVM.ManageUserVM
{
    public class ManageUserViewModel : BaseViewModel
    {
        #region Property
        private ObservableCollection<UserDTO> _listusermanage;
        public ObservableCollection<UserDTO> ListUserManage
        {
            get { return _listusermanage; }
            set { _listusermanage = value; OnPropertyChanged(); }
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _fullname;
        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; OnPropertyChanged(); }
        }

        private string _emailaddress;
        public string EmailAddress
        {
            get { return _emailaddress; }
            set { _emailaddress = value; OnPropertyChanged(); }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
        }

        private string _phonenumber;
        public string PhoneNumber
        {
            get { return _phonenumber; }
            set { _phonenumber = value; OnPropertyChanged(); }
        }

        //****
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


        #region ICommand
        public ICommand LoadManageUserData { get; set; }
        public ICommand UpdatingUser { get; set; }
        public ICommand DeletingUser { get; set; }
        public ICommand DeleteUserList { get; set; }
        public ICommand EditingUser { get; set; }
        public ICommand ExportToExcel { get; set; }
        public ICommand ImportFromExcel { get; set; }

        #endregion
        public ManageUserViewModel()
        {
            LoadManageUserData = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                Loaded(p);
            });

            DeletingUser = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                UserDTO item = p.Items[p.SelectedIndex] as UserDTO;
                string makhachhang = item.ID.ToString();
                using (var context = new LMSEntities1())
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@makhachhang", makhachhang);
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Delete this user?", MessageType.Waitting, MessageButtons.YesNo);
                    if (msb.ShowDialog() == true)
                    {
                        try
                        {
                            command.CommandText = "delete from ACCOUNT where ID = @makhachhang";
                            context.SaveChanges();
                            if (command.ExecuteNonQuery() != 0)
                            {
                                msb = new MessageBoxLMS("Notification", "Deleting is successful", MessageType.Accept, MessageButtons.OK);
                                msb.ShowDialog();
                                Loaded(p);
                            }
                        }
                        catch
                        {
                            msb = new MessageBoxLMS("Notification", "Cannot delete this user", MessageType.Error, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                }

            });

            DeleteUserList = new RelayCommand<DataGrid>(
            (p) =>
            {
                if (p != null)
                {
                    if (p.SelectedItems.Count > 1)
                        return true;
                    return false;
                }
                return false;
            }, (p) =>
            {
                List<string> userList = new List<string>();
                foreach (var item in p.SelectedItems)
                    userList.Add((item as UserDTO).ID.ToString());
                using (var context = new LMSEntities1())
                {
                    SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    string idList = string.Join(", ", userList);
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Delete this list of user?", MessageType.Waitting, MessageButtons.YesNo);
                    if (msb.ShowDialog() == true)
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@idList", idList);
                            command.CommandText = "delete from ACCOUNT where ID in ( " + @idList + " )";
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
                            msb = new MessageBoxLMS("Notification", "Cannot delete this list of user", MessageType.Error, MessageButtons.OK);
                            msb.ShowDialog();
                        }
                    }
                }
            });

            EditingUser = new RelayCommand<DataGrid>((p) => { return true; }, (p) =>
            {
                UserDTO item = p.Items[p.SelectedIndex] as UserDTO;
                int id = item.ID;
                EditingUserWindow window = new EditingUserWindow(id);
                if (item.FullName != null)
                    FullName = item.FullName.ToString();
                if (item.EmailAddress != null)
                    EmailAddress = item.EmailAddress.ToString();
                if (item.PhoneNumber != null)
                    PhoneNumber = item.PhoneNumber.ToString();
                window.ShowDialog();
                Loaded(p);
            });

            UpdatingUser = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                int id = EditingUserWindow.id;
                using (var context = new LMSEntities1())
                {
                    foreach (var user in context.ACCOUNTs)
                    {
                        if (id == user.ID)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(EmailAddress) && !string.IsNullOrEmpty(PhoneNumber))
                                {
                                    string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                                    Regex reg = new Regex(match);

                                    if (reg.IsMatch(EmailAddress) == false)
                                    {
                                        MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Email address is not valid!", MessageType.Error, MessageButtons.OK);
                                        ms.ShowDialog();
                                        return;
                                    }

                                    match = @"^(\+[0-9]{1,3}[- ]?)?([0-9]{10})$";
                                    reg = new Regex(match);
                                    if (!reg.IsMatch(PhoneNumber))
                                    {
                                        MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Phone number is not valid!", MessageType.Error, MessageButtons.OK);
                                        ms.ShowDialog();
                                        return;
                                    }

                                    user.FULLNAME = FullName;
                                    user.EMAILADDRESS = EmailAddress;
                                    user.PHONENUMBER = PhoneNumber;
                                    FullName = EmailAddress = PhoneNumber = "";
                                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Updating is successful!", MessageType.Accept, MessageButtons.OK);
                                    msb.ShowDialog();
                                    p.Hide();
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
                            }
                        }

                    }
                    context.SaveChanges();
                }
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
                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Please enter the path!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
                    return;
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                try
                {
                    using (ExcelPackage excel = new ExcelPackage())
                    {
                        excel.Workbook.Worksheets.Add("List of User");
                        ExcelWorksheet ws = excel.Workbook.Worksheets[0];
                        ws.Name = "List of User";
                        ws.Cells.Style.Font.Size = 11;
                        ws.Cells.Style.Font.Name = "Times New Roman";
                        string[] headerColumns = { "Full Name", "Email", "Phone Number", "StudentID", "ClassID"};

                        int numOfColumns = headerColumns.Count();
                        ws.Cells[1, 1].Value = "List of user - LMS Library";
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
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            colIndex++;

                        }

                        //Thêm dữ liệu vào sheet
                        foreach (UserDTO user in ListUserManage)
                        {
                            colIndex = 1;
                            rowIndex++;
                            ws.Cells[rowIndex, colIndex++].Value = user.FullName;
                            ws.Cells[rowIndex, colIndex++].Value = user.EmailAddress;
                            ws.Cells[rowIndex, colIndex++].Value = user.PhoneNumber;
                            ws.Cells[rowIndex, colIndex++].Value = user.StudentID;
                            ws.Cells[rowIndex, colIndex++].Value = user.ClassID;
                        }
                        ws.Column(1).Width = 22;
                        ws.Column(2).Width = 25;
                        ws.Column(3).Width = 30;
                        ws.Column(4).Width = 15;
                        ws.Column(5).Width = 15;

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
                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Please enter the path!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
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
                                string fullName = ws.Cells[i, 1].Value.ToString();
                                string studentID = ws.Cells[i, 2].Value.ToString();
                                string classID = ws.Cells[i, 3].Value.ToString();
                                string emailAddress = ws.Cells[i, 4].Value.ToString();
                                string phoneNumber = ws.Cells[i, 5].Value.ToString();
                                if (!String.IsNullOrEmpty(fullName) && !String.IsNullOrEmpty(emailAddress))
                                {
                                    ACCOUNT newAccount = new ACCOUNT();
                                    newAccount.FULLNAME = fullName;
                                    newAccount.PHONENUMBER = phoneNumber;
                                    newAccount.EMAILADDRESS = emailAddress;
                                    newAccount.MSSV = studentID;
                                    newAccount.IDCLASS = classID;
                                    newAccount.USERNAME = ws.Cells[i, 6].Value.ToString();
                                    newAccount.USERPASS = ws.Cells[i, 7].Value.ToString();
                                    newAccount.ROLE = 1;
                                    context.ACCOUNTs.Add(newAccount);
                                }
                            }
                            context.SaveChanges();
                            Loaded(p);
                        }
                    }
                    MessageBoxLMS msb = new MessageBoxLMS("Notification", "Successful import!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
                }
                catch
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Warning", "Error - Cannot import data from the selected file.", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }
            });
        }

        public void Loaded(DataGrid p)
        {
            ListUserManage = new ObservableCollection<UserDTO>();
            using (var context = new LMSEntities1())
            {
                foreach (var item in context.ACCOUNTs)
                {
                    UserDTO user = new UserDTO();
                    user.ID = item.ID;
                    user.FullName = item.FULLNAME;
                    user.EmailAddress = item.EMAILADDRESS;
                    user.PhoneNumber = item.PHONENUMBER;
                    user.StudentID = item.MSSV;
                    user.ClassID = item.IDCLASS;
                    ListUserManage.Add(user);
                }
            }
        }
    }
}
