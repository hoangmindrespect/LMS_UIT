﻿using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MainWindow;
using LibraryManagementSystem.View.MessageBoxCus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.View.MainClientWindow;

namespace LibraryManagementSystem.ViewModel.LoginVM
{
    public class LoginRegisViewModel : BaseViewModel
    {
        #region Property
        public static DataGrid import_dtg;
        public static ObservableCollection<ImportBook> listbook = new ObservableCollection<ImportBook>();
        //public static string UserNameThisAccount;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private bool _IsNullNameReg;
        public bool IsNullNameReg
        {
            get { return _IsNullNameReg; }
            set { _IsNullNameReg = value; OnPropertyChanged(); }
        }

        private bool _IsNullEmailReg;
        public bool IsNullEmailReg
        {
            get { return _IsNullEmailReg; }
            set { _IsNullEmailReg = value; OnPropertyChanged(); }
        }

        private bool _IsNullUserReg;
        public bool IsNullUserReg
        {
            get { return _IsNullUserReg; }
            set { _IsNullUserReg = value; OnPropertyChanged(); }
        }

        private bool _IsNullPasswordReg;
        public bool IsNullPasswordReg
        {
            get { return _IsNullPasswordReg; }
            set { _IsNullPasswordReg = value; OnPropertyChanged(); }
        }
        private int _authcode;
        public int Authcode
        {
            get { return _authcode; }
            set { _authcode = value; OnPropertyChanged(); }
        }

        private string _vertificationcode;
        public string VertificationCode
        {
            get { return _vertificationcode; }
            set { _vertificationcode = value; OnPropertyChanged(); }
        }

        private string _newpass;
        public string NewPass
        {
            get { return _newpass; }
            set { _newpass = value; OnPropertyChanged(); }
        }

        private string _confirmnewpass;
        public string ConfirmNewPass
        {
            get { return _confirmnewpass; }
            set { _confirmnewpass = value; OnPropertyChanged(); }
        }

        private string _emailreg;
        public string EmailReg
        {
            get { return _emailreg; }
            set { _emailreg = value; OnPropertyChanged(); }
        }

        private string _fullnamereg;
        public string FullNameReg
        {
            get { return _fullnamereg; }
            set { _fullnamereg = value; OnPropertyChanged(); }
        }

        private string _passwordreg;
        public string PasswordReg
        {
            get { return _passwordreg; }
            set { _passwordreg = value; OnPropertyChanged(); }
        }

        private string _usernamereg;
        public string UsernameReg
        {
            get
            {
                return _usernamereg;
            }
            set { _usernamereg = value; OnPropertyChanged(); }
        }
        public static string username;


        private bool _IsInvalidPasswordReg;
        public bool IsInvalidPasswordReg
        {
            get { return _IsInvalidPasswordReg; }
            set { _IsInvalidPasswordReg = value; OnPropertyChanged(); }
        }

        private bool _IsTooShortUsernameReg;
        public bool IsTooShortUsernameReg
        {
            get { return _IsTooShortUsernameReg; }
            set { _IsTooShortUsernameReg = value; OnPropertyChanged(); }
        }

        private bool _IsDuplicateUsernameReg;
        public bool IsDuplicateUsernameReg
        {
            get { return _IsDuplicateUsernameReg; }
            set { _IsDuplicateUsernameReg = value; OnPropertyChanged(); }
        }
        #endregion

        #region ICommand
        public ICommand LoadLoginPage { get; set; }
        public ICommand LoadForgotPassPage { get; set; }
        public ICommand BackLoginPage { get; set; }
        public ICommand BackLoginPageFromRegis { get; set; }
        public ICommand SendCode { get; set; }
        public ICommand ConfirmVerCode { get; set; }
        public ICommand CreatePassWord { get; set; }
        public ICommand NewPasswordChanged { get; set; }
        public ICommand ConfirmNewPasswordChanged { get; set; }
        public ICommand PasswordChangedLogin { get; set; }
        public ICommand LoginLMS { get; set; }
        public ICommand RegisterAccount { get; set; }
        public ICommand RegisterLMS { get; set; }
        public ICommand PasswordRegChanged { get; set; }
        #endregion

        #region temVar
        public ForgotPasswordPage fgpp;
        #endregion
      
        public LoginRegisViewModel()
        {
            LoadLoginPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new loginpage();
            });

            LoadForgotPassPage = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                fgpp = new ForgotPasswordPage();
                loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();
                Frame a = login.FindName("login_frame") as Frame;
                a.Content = fgpp;
            });

            BackLoginPage = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();
                Frame a = login.FindName("login_frame") as Frame;

                a.Content = new loginpage();
            });

            BackLoginPageFromRegis = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
                loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();
                login.blur_card.Visibility = Visibility.Collapsed;
            });

            SendCode = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                if (IsValidEmail(Email))
                {
                    fgpp.lable.Visibility = Visibility.Hidden;

                    p.Content = new VertificationPage();

                    Random random = new Random();
                    Authcode = random.Next(1, 999999);
                    SendEmail(Authcode.ToString());
                }
                else
                {
                    fgpp.lable.Visibility = Visibility.Visible;
                    return;
                }
            });

            ConfirmVerCode = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (VertificationCode == null || VertificationCode.Length == 0 || VertificationCode != Authcode.ToString())
                    p.Visibility = Visibility.Visible;
                else if (VertificationCode == Authcode.ToString())
                {
                    p.Visibility = Visibility.Hidden;

                    loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();
                    Frame a = login.FindName("login_frame") as Frame;

                    a.Content = new CreateNewPass();
                }
            });

            NewPasswordChanged = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                NewPass = p.Password;
            });

            ConfirmNewPasswordChanged = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                ConfirmNewPass = p.Password;
            });

            CreatePassWord = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(NewPass) || string.IsNullOrEmpty(ConfirmNewPass) || NewPass != ConfirmNewPass)
                {
                    p.Visibility = Visibility.Visible;
                    return;
                }
                else if (NewPass == ConfirmNewPass)
                {
                    p.Visibility = Visibility.Hidden;

                    //write new password into database
                    using (var context = new LMSEntities1())
                    {
                        foreach (var acc in context.ACCOUNTs)
                        {
                            if (acc.EMAILADDRESS.Equals(Email))
                                acc.USERPASS = ConfirmNewPass;
                        }
                        context.SaveChanges();
                    }

                    MessageBoxLMS msb = new MessageBoxLMS("Thông báo", "Change password successfully!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();

                    loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();
                    Frame a = login.FindName("login_frame") as Frame;

                    a.Content = new loginpage();
                }
                else
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Lỗi", "Lost database connection!", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }

            });

            PasswordChangedLogin = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                this.Password = p.Password;
            });

            LoginLMS = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {

                if (string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(UserName))
                {
                    p.Visibility = Visibility.Visible;
                }
                else
                {
                    LMSEntities1 context = new LMSEntities1();
                    string pas = (from s in context.ACCOUNTs where s.USERNAME == UserName select s.USERPASS).FirstOrDefault();
                    if (Password == pas)
                    {
                        loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();
                        int role = (int)(from s in context.ACCOUNTs where s.USERNAME == UserName select s.ROLE).FirstOrDefault();
                        if(role == 0)
                        {
                            MainWindowSystem w = new MainWindowSystem();
                            w.Show();

                            login.Close();
                        }  
                        else if(role == 1)
                        {
                            string AccountID = (from s in context.ACCOUNTs where s.USERNAME == UserName && s.USERPASS == Password select s.ID).FirstOrDefault().ToString();
                            MainClientWindow w = new MainClientWindow(AccountID);
                            w.Show();

                            login.Close();
                        }
                        username = UserName;
                    }
                    else if (Password != pas)
                    {
                        p.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Lỗi", "Lost database connection!", MessageType.Error, MessageButtons.OK);
                        msb.ShowDialog();
                    }

                }
            });

            RegisterAccount = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();

                login.blur_card.Visibility = Visibility.Visible;
                RegisterWindow w = new RegisterWindow();
                w.ShowDialog();

                login.blur_card.Visibility = Visibility.Hidden;
            });

            RegisterLMS = new RelayCommand<Label>((p) => { return true; }, async (p) =>
            {
                IsNullNameReg = IsNullEmailReg = IsNullUserReg = IsNullPasswordReg = IsInvalidPasswordReg = IsTooShortUsernameReg = IsDuplicateUsernameReg = false;

                if (string.IsNullOrEmpty(FullNameReg)) IsNullNameReg = true;
                if (string.IsNullOrEmpty(EmailReg)) IsNullEmailReg = true;
                if (string.IsNullOrEmpty(UsernameReg)) IsNullUserReg = true;
                if (string.IsNullOrEmpty(PasswordReg)) IsNullPasswordReg = true;

                if (IsNullNameReg || IsNullEmailReg || IsNullUserReg || IsNullPasswordReg) return;

                if (PasswordReg.Length < 8 || !HasNumber(PasswordReg) || !HasSpecialCharacter(PasswordReg) || !HasUppercaseCharacter(PasswordReg)) IsInvalidPasswordReg = true;
                if (IsInvalidPasswordReg) return;

                if (UsernameReg.Length < 8) IsTooShortUsernameReg = true;
                if (IsTooShortUsernameReg) return;

                string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex reg = new Regex(match);

                if (reg.IsMatch(EmailReg) == false)
                {
                    MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Email address is not valid!", MessageType.Error, MessageButtons.OK);
                    ms.ShowDialog();
                    return;
                }
                else
                {
                    using (var context = new LMSEntities1())
                    {
                        ACCOUNT a = new ACCOUNT();
                        a.USERNAME = UsernameReg;
                        a.USERPASS = PasswordReg;
                        a.EMAILADDRESS = EmailReg;
                        a.FULLNAME = FullNameReg;
                        a.ROLE = 1;
                        context.ACCOUNTs.Add(a);
                        context.SaveChanges();

                        RegisterWindow r = Application.Current.Windows.OfType<RegisterWindow>().FirstOrDefault();
                        r.Close();

                        MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Successful registration!", MessageType.Accept, MessageButtons.OK);
                        ms.ShowDialog();
                    }
                }

            });

            PasswordRegChanged = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                PasswordReg = p.Password;
            });
        }
        public bool IsValidEmail(string email)
        {
            if (email == null)
                return false;
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public void SendEmail(string content)
        {
            MailMessage mailMessage = new MailMessage("hlibrarymanagementsystemhelps@gmail.com\r\n", Email, "Khôi phục mật khẩu LMS", "Mã xác nhận của bạn là: " + content);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("librarymanagementsystemhelps@gmail.com\r\n", "fjnimjkbsnvdyyps\r\n");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
        }

        private bool HasNumber(string input)
        {
            return input.Any(c => char.IsDigit(c));
        }

        private bool HasSpecialCharacter(string input)
        {
            string specialChars = @"!@#$%^&*()-_=+[{]}\|;:'"",<.>/?";
            return input.Any(c => specialChars.Contains(c));
        }

        private bool HasUppercaseCharacter(string input)
        {
            return input.Any(c => char.IsUpper(c));
        }

    }
}
