using LibraryManagementSystem.View.Login;
using LibraryManagementSystem.View.MainWindow.Statistical;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModel
{
    public class StatisticalViewModel:BaseViewModel
    {
        #region Property
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set { _seriesCollection = value;  OnPropertyChanged(); }
        }

        private string[] _labels;
        public string[] Labels
        {
            get { return _labels; }
            set { _labels = value; OnPropertyChanged();}
        }

        private Func<double, string> _yFormatter;
        public Func<double, string> YFormatter
        {
            get { return _yFormatter; }
            set { _yFormatter = value ; OnPropertyChanged(); }
        }

        private ObservableCollection<String> _collectionYear;
        public ObservableCollection<String> CollectionYear
        {
            get { return _collectionYear; }
            set { _collectionYear = value; OnPropertyChanged(); }
        }

        private ObservableCollection<String> _collectionMonth;
        public ObservableCollection<String> CollectionMonth
        {
            get { return _collectionMonth; }
            set { _collectionMonth = value; OnPropertyChanged(); }
        }

        private string _year;
        public string Year
        {
            get { return _year; }
            set { _year = value; OnPropertyChanged(); }
        }

        private string _month;
        public string Month
        {
            get { return _month; }
            set { _month = value; OnPropertyChanged(); }
        }

        #endregion

        #region Command
        public ICommand LoadYear { get; set; }
        public ICommand LoadMonth { get; set; }
        #endregion
        public StatisticalViewModel()
        {
            FirstLoad();
            LoadMonth = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                DateTime now = DateTime.Now;
                Year = p.SelectedItem.ToString();
                Month = null;
                if (now.Year > int.Parse(Year))
                {
                    CollectionMonth = new ObservableCollection<string>
                    {
                        "tháng 1", "tháng 2", "tháng 3", "tháng 4",
                        "tháng 5", "tháng 6", "tháng 7", "tháng 8",
                        "tháng 9", "tháng 10", "tháng 11", "tháng 12"
                    };
                }
                else
                {
                    CollectionMonth = new ObservableCollection<string>();
                    for(int i = 1; i <= now.Month; i++)
                    {
                        CollectionMonth.Add("tháng " + i.ToString());
                    }
                }
            });

        }

        public void FirstLoad()
        {

            // Load statistical
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Income",
                    Values = new ChartValues<double> { 400, 600, 500,500  }
                },
                new LineSeries
                {
                    Title = "Spending",
                    Values = new ChartValues<double> { 600, 700, 300, 40 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Profit",
                    Values = new ChartValues<double> { 400,200,300,600 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };
            Labels = new[] { "week 1", "week 2", "week 3", "week 4" };
            YFormatter = value => value.ToString("C");

            //Load  year first
            DateTime now = DateTime.Now;
            CollectionYear = new ObservableCollection<string>
            {
                    now.Year.ToString(),
                    (now.Year - 1).ToString(),
                    (now.Year - 2).ToString()
            };
            Year = now.Year.ToString();

            CollectionMonth = new ObservableCollection<string>();
            for (int i = 1; i <= now.Month; i++)
            {
                CollectionMonth.Add("tháng " + i.ToString());
            }
            Month = "tháng " + now.Month.ToString();
        }
    }
}
