using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MessageBoxCus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Text.RegularExpressions;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.View.MainWindow;
using System.ComponentModel;

namespace LibraryManagementSystem.ViewModel
{
    public  class LoginRegisViewModel:BaseViewModel
    {
        #region Property
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

        private Visibility _status;
        public Visibility Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(nameof(Status));}
        }

        private bool _IsSaving;
        public bool IsSaving
        {
            get { return _IsSaving; }
            set { _IsSaving = value; OnPropertyChanged(); }
        }
        #endregion

        #region ICommand
        public ICommand LoadLoginPage { get; set; }
        public ICommand LoadForgotPassPage { get; set; }
        public ICommand BackLoginPage { get; set; }
        public ICommand BackLoginPageFromRegis {  get; set; }
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
        Frame login_frame;
        ForgotPasswordPage fgpp;
        Card blur_card;
        #endregion
        public LoginRegisViewModel()
        {

            ///Status = Visibility.Hidden;

            LoadLoginPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new loginpage();
                login_frame = p;
                blur_card = loginwindow.a;
                blur_card.Visibility = Visibility.Hidden;
            });

            LoadForgotPassPage = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                fgpp = new ForgotPasswordPage();
                login_frame.Content = fgpp;
            });

            BackLoginPage = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                login_frame.Content = new loginpage();
            });

            BackLoginPageFromRegis = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
                blur_card.Visibility = Visibility.Hidden;
                
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
                    login_frame.Content = new CreateNewPass();
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
                    using (var context = new LMSEntities())
                    {
                        foreach(var acc in context.ACCOUNTs)
                        {
                            if(acc.EMAILADDRESS.Equals(Email))
                                acc.USERPASS = ConfirmNewPass;
                        }
                        context.SaveChanges();
                    }    
                    
                    MessageBoxLMS msb = new MessageBoxLMS("Thông báo", "Thay đổi mật khẩu thành công!", MessageType.Accept, MessageButtons.OK);
                    msb.ShowDialog();
                    login_frame.Content = new loginpage();
                }
                else
                {
                    MessageBoxLMS msb = new MessageBoxLMS("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    msb.ShowDialog();
                }    

            });

            PasswordChangedLogin = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                this.Password = p.Password;
            });

            LoginLMS = new RelayCommand<Label>((p) => { return true; }, (p) =>
            {
                IsSaving =  true;

                if (string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(UserName))  
                {
                    p.Visibility= Visibility.Visible;
                }
                else
                {
                    LMSEntities context = new LMSEntities();
                    string pas = (from s in context.ACCOUNTs where s.USERNAME == UserName select s.USERPASS).FirstOrDefault();
                    if (Password == pas)
                    {

                        MainWindowSystem w = new MainWindowSystem();
                        w.Show();


                        loginwindow login = Application.Current.Windows.OfType<loginwindow>().FirstOrDefault();
                        login.Close();
                        
                    }
                    else if (Password != pas)
                    { 
                        p.Visibility = Visibility.Visible; 
                    }
                    else
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Lỗi", "Mất kết nối cơ sở dữ liệu!", MessageType.Error, MessageButtons.OK);
                        msb.ShowDialog();
                    }

                    IsSaving = false;

                }
            });

            RegisterAccount = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                blur_card.Visibility = Visibility.Visible;
                RegisterWindow w = new RegisterWindow();
                w.ShowDialog();

                blur_card.Visibility = Visibility.Hidden;
            });

            RegisterLMS = new RelayCommand<Label>((p) => { return true; }, async (p) =>
            {
                IsNullNameReg = IsNullEmailReg = IsNullUserReg = IsNullPasswordReg = false;

                if (string.IsNullOrEmpty(FullNameReg)) IsNullNameReg = true;
                if (string.IsNullOrEmpty(EmailReg)) IsNullEmailReg = true;
                if (string.IsNullOrEmpty(UsernameReg)) IsNullUserReg = true;
                if (string.IsNullOrEmpty(PasswordReg)) IsNullPasswordReg = true;

                if (IsNullNameReg || IsNullEmailReg || IsNullUserReg || IsNullPasswordReg) return;

                string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex reg = new Regex(match);

                if (reg.IsMatch(EmailReg) == false)
                {
                    MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Email không hợp lệ", MessageType.Error, MessageButtons.OK);
                    ms.ShowDialog();
                    return;
                }
                else
                {
                    using (var context = new LMSEntities())
                    {
                        ACCOUNT a = new ACCOUNT();
                        a.USERNAME = UsernameReg;
                        a.USERPASS = PasswordReg;
                        a.EMAILADDRESS = EmailReg;
                        a.FULLNAME = FullNameReg;
                        context.ACCOUNTs.Add(a);
                        context.SaveChanges();

                        RegisterWindow r = Application.Current.Windows.OfType<RegisterWindow>().FirstOrDefault();
                        r.Close();

                        MessageBoxLMS ms = new MessageBoxLMS("Thông báo", "Đăng ký thành công", MessageType.Accept, MessageButtons.OK);
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
        
    }
}
