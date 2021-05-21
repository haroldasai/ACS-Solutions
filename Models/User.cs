using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acs_chat_app_2.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public List<ChatThread> Threads { get; set; }
        
        public User(string userid = "8:acs:0d4ee1ba-5ade-4f25-9804-f2f3073f286f_00000009-d854-c34a-570c-113a0d00da01", string name = "Harold Asai", string accesstoken = "")
        {
            UserID = userid;
            Name = name;
            AccessToken = accesstoken;
            Threads = new List<ChatThread>();
        }
        
    }
}
