using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class BookInBorrowDTO:INotifyPropertyChanged
    {
        //public int MaPhieuMuon { get; set; }
        //public string TenKH { get; set; }
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        //public string img { get; set; }
        //public int SoLuong { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        //public int SoLuongMax { get; set; }
        //public DateTime NgayHetHan { get; set; }
        //public DateTime NgayMuon { get; set; }
        private int soLuong;

        public int SoLuong
        {
            get { return soLuong; }
            set
            {
                if (soLuong != value)
                {
                    soLuong = value;
                    OnPropertyChanged(nameof(SoLuong));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
