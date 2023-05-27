using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class DetailBookBorrowForm
    {
        public int ID { get; set; }// id book borrow form
        public int IDBook { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }

    }
}
