using System.Collections.Generic;

namespace Chat.DAL.Models.Pagging
{
    public class PagingList<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}