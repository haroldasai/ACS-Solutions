using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Communication.Chat;

namespace acs_chat_app_2.Models
{
    public class ChatThread
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public List<ChatMessage> Messages { get; set; }

        public ChatThread(string id, string topic, List<ChatMessage> messages)
        {
            Id = id;
            Topic = topic;
            Messages = messages;
        }

    }
}
