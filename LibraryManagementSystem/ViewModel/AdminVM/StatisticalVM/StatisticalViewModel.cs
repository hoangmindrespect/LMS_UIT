using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryManagementSystem.Models.DataProvider;
using System.Globalization;
using System.Windows;
using DocumentFormat.OpenXml.Bibliography;
using Haley.Utils;

namespace LibraryManagementSystem.ViewModel.AdminVM.StatisticalVM
{
    public class StatisticalViewModel : BaseViewModel
    {
        #region Property
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set { _seriesCollection = value; OnPropertyChanged(); }
        }

        private string[] _labels;
        public string[] Labels
        {
            get { return _labels; }
            set { _labels = value; OnPropertyChanged(); }
        }

        private Func<double, string> _yFormatter;
        public Func<double, string> YFormatter
        {
            get { return _yFormatter; }
            set { _yFormatter = value; OnPropertyChanged(); }
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

        private string _totalIncome;
        public string TotalIncome
        {
            get { return _totalIncome; }
            set { _totalIncome = value; OnPropertyChanged(); }
        }

        private string _totalSpending;
        public string TotalSpending
        {
            get { return _totalSpending; }
            set { _totalSpending = value; OnPropertyChanged(); }
        }

        private string _totalProfit;
        public string TotalProfit
        {
            get { return _totalProfit; }
            set { _totalProfit = value; OnPropertyChanged(); }
        }

        List<decimal> statisCollectionIncomeYear; // 12 giá trị
        List<decimal> statisCollectionIncomeMonth; // 31 giá trị
        List<decimal> statisCollectionSpendingYear; // 12 giá trị
        List<decimal> statisCollectionSpendingMonth; // 31 giá trị
        List<decimal> statisCollectionProfitYear; // 12 giá trị
        List<decimal> statisCollectionProfitMonth; // 31 giá trị
        #endregion

        #region Command
        public ICommand LoadAfterChooseMonth { get; set; }
        public ICommand LoadMonth { get; set; }
        #endregion
        public StatisticalViewModel()
        {
            
            FirstLoad();
            LoadMonth = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                LoadChartYear(int.Parse(Year));
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
                    for (int i = 1; i <= now.Month; i++)
                    {
                        CollectionMonth.Add("tháng " + i.ToString());
                    }
                }

                // Khi chọn năm, hiển thị theo năm, lúc này ô tháng vẫn còn trống
                TotalIncome = (decimal.Round(getTotalIncomeYear(int.Parse(Year)), 0)).ToString("C0").Replace("$", "₫");
                TotalSpending = (decimal.Round(getTotalSpendingYear(int.Parse(Year)), 0)).ToString("C0").Replace("$", "₫");
                decimal profit = getTotalIncomeYear(int.Parse(Year)) - getTotalSpendingYear(int.Parse(Year));
                if (profit >= 0)
                {
                    TotalProfit = decimal.Round(profit, 0).ToString("C0").Replace("$", "₫");
                }
                else
                {
                    TotalProfit = "-" + decimal.Round(profit, 0).ToString("C0").Replace("(", "").Replace(")", "").Replace("$", "₫");
                }

            });

            LoadAfterChooseMonth = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                //chọn tháng sau đó load các giá trị trên 3 panel
                if (string.IsNullOrEmpty(Month))
                    return;

                int tmp = int.Parse(Month.Substring(6, 1));
                TotalIncome = (decimal.Round(getTotalIncomeMonth(int.Parse(Year), tmp), 0)).ToString("C0").Replace("$", "₫");
                TotalSpending = (decimal.Round(getTotalSpendingMonth(int.Parse(Year), tmp), 0)).ToString("C0").Replace("$", "₫");
                decimal profit = getTotalIncomeMonth(int.Parse(Year), tmp) - getTotalSpendingMonth(int.Parse(Year), tmp);
                if (profit >= 0)
                {
                    TotalProfit = decimal.Round(profit, 0).ToString("C0").Replace("$", "₫");
                }
                else
                {
                    TotalProfit = "-" + decimal.Round(profit, 0).ToString("C0").Replace("(", "").Replace(")", "").Replace("$", "₫");
                }
                int az = int.Parse(Month.Substring(6, 1));
                LoadChartMonth(int.Parse(Year), az);
            });

        }

        public void FirstLoad()
        {
            MessageBox.Show((getTotalIncomeWeek(2023, 6, 1).ToString()));
            TotalIncome = (decimal.Round(getTotalIncomeMonth(DateTime.Now.Year, DateTime.Now.Month), 0)).ToString("C0").Replace("$", "₫");
            TotalSpending = (decimal.Round(getTotalSpendingMonth(DateTime.Now.Year,DateTime.Now.Month), 0)).ToString("C0").Replace("$", "₫");
            decimal profit = getTotalIncomeMonth(DateTime.Now.Year,DateTime.Now.Month) - getTotalSpendingMonth(DateTime.Now.Year,DateTime.Now.Month);
            if(profit >= 0)
            {
                TotalProfit = decimal.Round(profit, 0).ToString("C0").Replace("$", "₫");
            }
            else
            {
                TotalProfit = "-" + decimal.Round(profit, 0).ToString("C0").Replace("(", "").Replace(")", "").Replace("$", "₫");
            }

            // Load statistical
            LoadChartMonth(DateTime.Now.Year, DateTime.Now.Month);

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

        public void LoadChartYear(int year)
        {
            statisCollectionIncomeYear = new List<decimal>();
            statisCollectionProfitYear = new List<decimal>();
            statisCollectionSpendingYear = new List<decimal>();
            for(int i = 1; i <= 12; i++)
            {
                statisCollectionIncomeYear.Add(getTotalIncomeMonth(year, i));
                statisCollectionSpendingYear.Add(getTotalSpendingMonth(year, i));
                statisCollectionProfitYear.Add(getTotalIncomeMonth(year, i) - getTotalSpendingMonth(year, i));
            }

            // Load statistical
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Income",
                    Values = new ChartValues<decimal> (statisCollectionIncomeYear)
                },
                new LineSeries
                {
                    Title = "Spending",
                    Values = new ChartValues<decimal> (statisCollectionSpendingYear),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Profit",
                    Values = new ChartValues<decimal> (statisCollectionProfitYear),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };
            Labels = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            YFormatter = value => value.ToString("C0").Replace("$", "VND");
        }   
        
        public void LoadChartMonth(int year, int month)
        {
            statisCollectionIncomeMonth = new List<decimal>();
            statisCollectionSpendingMonth = new List<decimal>();
            statisCollectionProfitMonth = new List<decimal>();
            for(int i = 1; i <= 4; i++)
            {
                statisCollectionIncomeMonth.Add(getTotalIncomeWeek(year, month, i));
                statisCollectionSpendingMonth.Add(getTotalSpendingWeek(year, month, i));
                statisCollectionProfitMonth.Add(getTotalIncomeWeek(year, month, i) - getTotalSpendingWeek(year, month, i));
            }

            // Load statistical
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Income",
                    Values = new ChartValues<decimal> (statisCollectionIncomeMonth)
                },
                new LineSeries
                {
                    Title = "Spending",
                    Values = new ChartValues<decimal> (statisCollectionSpendingMonth),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Profit",
                    Values = new ChartValues<decimal> (statisCollectionProfitMonth),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };
            Labels = new[] { "week 1", "week 2", "week 3", "week 4"};
            YFormatter = value => value.ToString("C0").Replace("$", "VND");
        }
        public decimal getTotalIncomeYear(int year)
        {
            decimal result = 0;
            using(var context = new LMSEntities1())
            {
                var billList = context.ORDER_BOOKS.Where(b => b.orderDate.Year == year);

                if (billList.ToList().Count != 0)
                {
                    result = (decimal)billList.Sum(b => b.totalValue);
                }
            }
            return result;
        }    

        public decimal getTotalIncomeMonth(int year, int month)
        {
            decimal result = 0;
            using (var context = new LMSEntities1())
            {
                var billList = context.ORDER_BOOKS.Where(b => b.orderDate.Month == month && b.orderDate.Year == year);

                if (billList.ToList().Count != 0)
                {
                    result = (decimal)billList.Sum(b => b.totalValue);
                }
            }
            return result;
        }

        public decimal getTotalIncomeWeek(int year, int month, int week)
        {
            decimal result = 0;
            using (var context = new LMSEntities1())
            {
                int start = 0; int end = 0;
                if(week == 1)
                {
                    start = 1; end = 7;
                }   
                else if(week == 2)
                {
                    start = 8; end = 14;
                }    
                else if(week == 3)
                {
                    start = 15; end = 21;
                }
                else if(week == 4)
                {
                    start = 22; end = 31;
                }

                for(int i = start; i <= end; i++)
                {
                    var billList = context.ORDER_BOOKS.Where(b => b.orderDate.Year == year && b.orderDate.Month == month && b.orderDate.Day == i);

                    if (billList.ToList().Count != 0)
                    {
                        result += (decimal)billList.Sum(b => b.totalValue);
                    }
                }    
            }
            return result;
        }
        public decimal getTotalSpendingYear(int year)
        {
            decimal result = 0;
            using (var context = new LMSEntities1())
            {
                var billList = context.IMPORTs.Where(b => b.NGNHAP.Year == year);

                if (billList.ToList().Count != 0)
                {
                    result = (decimal)billList.Sum(b => b.TRIGIA);
                }
            }
            return result;
        }

        public decimal getTotalSpendingMonth(int year,int month)
        {
            decimal result = 0;
            using (var context = new LMSEntities1())
            {
                var billList = context.IMPORTs.Where(b => b.NGNHAP.Month == month && b.NGNHAP.Year == year);

                if (billList.ToList().Count != 0)
                {
                    result = (decimal)billList.Sum(b => b.TRIGIA);
                }
            }
            return result;
        }

        public decimal getTotalSpendingWeek(int year, int month, int week)
        {
            decimal result = 0;
            using (var context = new LMSEntities1())
            {
                int count = 0;
                if (week == 4)
                    count = 10;
                else
                    count = 7;
                for (int i = 1; i <= count; i++)
                {
                    var billList = context.IMPORTs.Where(b => b.NGNHAP.Year == year && b.NGNHAP.Month == month && b.NGNHAP.Day == i);

                    if (billList.ToList().Count != 0)
                    {
                        result += (decimal)billList.Sum(b => b.TRIGIA);
                    }
                }
            }
            return result;
        }
    }
  
}
