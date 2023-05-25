using DocumentFormat.OpenXml.Presentation;
using LibraryManagementSystem.DTOs;
using LibraryManagementSystem.Models.DataProvider;
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
    public class MainClientViewModel:BaseViewModel
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
        #endregion

        #region ICommand
        public ICommand LoadBuyBookFirst { get; set; }
        #endregion

        #region tempVar
        public static Frame main_frame_client;
        #endregion
        public MainClientViewModel()
        {
            LoadBuyBookFirst = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                p.Content = new BuyBookPage();
                main_frame_client = p;
            });

        }
    }
}
