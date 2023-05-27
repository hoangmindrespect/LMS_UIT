using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class BookBorrowForm
    {
        public int ID { get; set; }
        public int IDCus { get; set; }
        public string Name { get; set; }
        public string MSSV { get; set; }
        public string Status { get; set; }
        public string DayStart { get; set; }
        public string DayEnd { get; set; }
        public bool Collected { get; set; }
        public string ColorBack { get; set; }
        public ObservableCollection<DetailBookBorrowForm> list { get; set; }
    }
}
