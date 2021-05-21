using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acs_chat_app_2.Models
{
    public class InputMessage
    {
        public string Message { get; set; }
        public string DisplayName { get; set; }
        public int ThreadIndex { get; set; }
    }
}
