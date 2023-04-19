using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTOs
{
    public class UserDTO
    {
        public int ID { set; get; }
        public string UserName { set; get; }
        public string FullName { set; get; }
        public string EmailAddress { set; get; }
        public string Password { set; get; }
    }
}
