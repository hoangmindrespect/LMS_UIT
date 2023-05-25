using DocumentFormat.OpenXml.ExtendedProperties;
using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MainWindow;
using LibraryManagementSystem.View.MainWindow.BorrowBook;
using LibraryManagementSystem.View.MainWindow.ImportBook;
using LibraryManagementSystem.View.MainWindow.ManageBook;
using LibraryManagementSystem.View.MainWindow.Statistical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel.AdminVM
{
    public class MainAdminViewModel : BaseViewModel
    {
        #region Property
        private string _CurrentTime;
        public string CurrentTime
        {
            get { return _CurrentTime; }
            set { _CurrentTime = value; OnPropertyChanged(); }
        }
        #endregion

        public ICommand LoadStatisticalFirst { get; set; }
        public ICommand LoadManageBook { get; set; }
        public ICommand LoadImportPage { get; set; }
        public ICommand LoadBorrowBookPage { get; set; }
        public ICommand LoadManageBorrowBookPage { get; set; }
        public ICommand Logout { get; set; }
        public MainAdminViewModel()
        {
            System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            LoadStatisticalFirst = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new StatisticalPage();
            });

            LoadManageBook = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ManageBookPage();
            });

            LoadImportPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ImportBookPage();
            });

            LoadBorrowBookPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new BorrowBookPage();
            });

            LoadManageBorrowBookPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ManageBorrowBookPage();
            });
            Logout = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                loginwindow w = new loginwindow();
                w.Show();


                MainWindowSystem pk = System.Windows.Application.Current.Windows.OfType<MainWindowSystem>().FirstOrDefault();
                pk.Close();
                

            });
        }

        public void Timer_Click(object sender, EventArgs e)
        {
            DateTime d;
            d = DateTime.Now;
            CurrentTime = string.Format("{0}:{1}:{2}", d.Hour.ToString("00"), d.Minute.ToString("00"), d.Second.ToString("00"));
        }
    }
}
