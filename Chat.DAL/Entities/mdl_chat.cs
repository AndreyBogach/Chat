namespace Chat.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class mdl_chat
    {
        public int id { get; set; }

        public string message { get; set; }

        public int sender_id { get; set; }

        public int receiver_id { get; set; }

        public DateTime creation_date { get; set; }

        public byte record_state { get; set; }
    }
}
