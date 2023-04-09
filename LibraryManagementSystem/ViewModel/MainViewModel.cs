using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        #region Property

        #endregion

        #region ICommand
        public ICommand Login { get; set; }
        #endregion
        public MainViewModel()
        {
            Login = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                MessageBox.Show("Loo cc");
            });
        }
    }
}
