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
        public ICommand Loaded { get; set; }
        public ICommand SaveChange { get; set; }
        public ICommand ChooseMale { get; set; }
        public ICommand ChooseFeMale { get; set; }
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
        }
    }
}
