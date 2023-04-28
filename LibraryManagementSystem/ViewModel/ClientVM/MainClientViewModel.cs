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
        #endregion

        #region ICommand
        public ICommand LoadBuyBookFirst { get; set; }
        #endregion
        public MainClientViewModel()
        {
            LoadBuyBookFirst = new RelayCommand<Frame>((p) => { return p != null; }, (p) =>
            {
                p.Content = new BuyBookPage();
            });

        }
    }
}
