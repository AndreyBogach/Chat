using System;

namespace Chat.DAL.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public User Sender { get; set; }
        public User Reciever { get; set; }
        public DateTime Date { get; set; }
    }
}