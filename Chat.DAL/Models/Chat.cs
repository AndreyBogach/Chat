using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderAvatar { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAvatar { get; set; }
        public DateTime Date { get; set; }
    }
}