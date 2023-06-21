using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int CusId { get; set; }
        public string OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusColor { get; set; }
        public string OrderStatusDisplay { get; set; }
        public string OrderValue { get; set; }
        public ObservableCollection<BookDTO> Details { get; set; }
    }
}
