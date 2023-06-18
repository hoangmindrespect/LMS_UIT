using DocumentFormat.OpenXml.Presentation;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MainClientWindow;
using LibraryManagementSystem.View.MainClientWindow.BuyBookPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel.ClientVM
{
    public class MainClientViewModel : BaseViewModel
    {
        #region Property
        private static ClientDTO _CurrentCustomer;
        public static ClientDTO CurrentCustomer
        {
            get { return _CurrentCustomer; }
            set
            {
                _CurrentCustomer = value;
            }
        }

        private string _AccountID;
        public string AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; OnPropertyChanged(); }
        }


        #endregion

        #region ICommand
        public ICommand LoadBuyBookFirst { get; set; }
        public ICommand Logout { get; set; }
        #endregion

        #region tempVar
        public static Frame main_frame_client;
        #endregion
        public MainClientViewModel()
        {
            LoadBuyBookFirst = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                p.Content = new BuyBookPage(AccountID);
                main_frame_client = p;
            });

            Logout = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                loginwindow w = new loginwindow();
                w.Show();
                MainClientWindow pk = System.Windows.Application.Current.Windows.OfType<MainClientWindow>().FirstOrDefault();
                pk.Close();
            });
        }
    }
}
