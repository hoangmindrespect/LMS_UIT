using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using LibraryManagementSystem.View.MessageBoxCus;
using LibraryManagementSystem.ViewModel.LoginVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel.ClientVM.SettingClient
{
    public class SettingClientViewModel : BaseViewModel
    {
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged();}
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        private bool _male;
        public bool Male
        {
            get { return _male; }
            set
            {
                _male = value;
                OnPropertyChanged(); 
            }
        }

        private bool _feMale;
        public bool FeMale
        {
            get { return _feMale; }
            set 
            { 
                _feMale = value;
                OnPropertyChanged(); 
            }
        }

        private string _pas;
        public string Pas
        {
            get { return _pas; }
            set { _pas = value; OnPropertyChanged(); }
        }

        private string _newPas;
        public string NewPas
        {
            get { return _newPas; }
            set { _newPas = value; OnPropertyChanged(); }
        }

        private string _conNewPas;
        public string ConNewPas
        {
            get { return _conNewPas; }
            set { _conNewPas = value; OnPropertyChanged(); }
        }
        public ICommand Loaded { get; set; }
        public ICommand SaveChange { get; set; }
        public ICommand ChooseMale { get; set; }
        public ICommand ChooseFeMale { get; set; }
        public ICommand CurrentPassword { get; set; }
        public ICommand NewPassword { get; set; }
        public ICommand ConfirmPassword { get; set; }
    public SettingClientViewModel() 
        { 
            Loaded = new RelayCommand<Page>((p) => { return p != null; }, (p) =>
            {
                using(var context = new LMSEntities1())
                {
                    FullName = (from s in context.ACCOUNTs where s.USERNAME == LoginRegisViewModel.username select s.FULLNAME).FirstOrDefault();
                    Gender = (from s in context.ACCOUNTs where s.USERNAME == LoginRegisViewModel.username select s.GENDER).FirstOrDefault();
                    if(!string.IsNullOrWhiteSpace(Gender))
                    {
                        if(Gender == "Male")
                        {
                            Male = true;
                            FeMale = false;
                        }    
                        else if(Gender == "Female")
                        {
                            FeMale = true;
                            Male = false;
                        }    
                    }    
                    PhoneNumber = (from s in context.ACCOUNTs where s.USERNAME == LoginRegisViewModel.username select s.PHONENUMBER).FirstOrDefault();
                    Email = (from s in context.ACCOUNTs where s.USERNAME == LoginRegisViewModel.username select s.EMAILADDRESS).FirstOrDefault();
                    Address = (from s in context.ACCOUNTs where s.USERNAME == LoginRegisViewModel.username select s.ADDRESSS).FirstOrDefault();
                }    
            });

            SaveChange = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //có thay đổi mật khẩu
                if(!string.IsNullOrEmpty(Pas))
                {
                    if (string.IsNullOrWhiteSpace(NewPas) || string.IsNullOrEmpty(ConNewPas))
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Error", "This field cannot be empty!", MessageType.Accept, MessageButtons.OK);
                        msb.ShowDialog();
                        return;

                    }
                    else if (NewPas != ConNewPas)
                    {
                        MessageBoxLMS msb = new MessageBoxLMS("Error", "Password not matching!", MessageType.Accept, MessageButtons.OK);
                        msb.ShowDialog();
                        return;
                    }
                    else
                    {
                        using (var context = new LMSEntities1())
                        {
                            foreach (var item in context.ACCOUNTs)
                            {
                                if (item.USERNAME == LoginRegisViewModel.username)
                                {
                                    item.USERPASS = NewPas;
                                    break;
                                }
                            }
                            context.SaveChanges();
                        }
                    }
                }
                using (var context = new LMSEntities1())
                {
                    foreach (var item in context.ACCOUNTs)
                    {
                        if (item.USERNAME == LoginRegisViewModel.username)
                        {
                            item.FULLNAME = FullName;
                            if (Male == true)
                                item.GENDER = "Male";
                            else if (FeMale == true)
                                item.GENDER = "Female";
                            item.PHONENUMBER = PhoneNumber;
                            item.EMAILADDRESS = Email;
                            item.ADDRESSS = Address;
                            MessageBoxLMS msb = new MessageBoxLMS("Notification", "Saved all changes", MessageType.Accept, MessageButtons.OK);
                            msb.ShowDialog();
                            break;
                        }
                    }
                    context.SaveChanges();
                }
            });

            ChooseMale = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Male = true;
                FeMale = false;
            });

            ChooseFeMale = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Male = false;
                FeMale = true;
            });

            CurrentPassword = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                Pas = p.Password;
            });

            NewPassword = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                NewPas = p.Password;
            });

            ConfirmPassword = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                ConNewPas = p.Password;
            });
        }
    }
}
