namespace Chat.DAL.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ChatContext : DbContext
    {
        public ChatContext()
            : base("name=ChatContext")
        {
        }

        public virtual DbSet<mdl_chat> mdl_chat { get; set; }
        public virtual DbSet<mdl_user> mdl_user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
