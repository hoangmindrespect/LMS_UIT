using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class BookDTO
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public int NamXB { get; set; }
        public string MoTa { get; set; }
        public string ImageSource { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string TheLoai { get; set; }
        public string NXB { get; set; }

        public string ColorBack { get; set; }
        
    }
}
