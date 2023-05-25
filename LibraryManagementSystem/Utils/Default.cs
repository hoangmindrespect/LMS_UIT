using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Utils
{
    public class BookInBorrow
    {
        public static class STATUS
        {
            public static readonly string All = "ALL";
            public static readonly string Undue = "Chưa đến hạn trả";
            public static readonly string OutOfDay = "Quá hạn trả";
        }
    }
}
