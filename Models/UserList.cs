using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acs_chat_app_2.Models
{
    public class UserList
    {
        public List<User> Users { get; set; }
        public UserList()
        {
            Users = new List<User>();
        }
    }
}
