using LibraryManagementSystem.View.Login;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryManagementSystem.View.MessageBoxCus;
using System.Threading;

namespace LibraryManagementSystem.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Property
        private string _CurrentTime;
        public string CurrentTime
        {
            get { return _CurrentTime; }
            set { _CurrentTime = value; OnPropertyChanged(); }
        }
        #endregion

        public MainViewModel()
        {
            System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        public void Timer_Click(object sender, EventArgs e)
        {
            DateTime d;
            d = DateTime.Now;
            CurrentTime = string.Format("{0}:{1}:{2}", d.Hour.ToString("00"), d.Minute.ToString("00"), d.Second.ToString("00"));
        }
    }

}
