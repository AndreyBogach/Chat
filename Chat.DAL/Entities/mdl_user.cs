namespace Chat.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class mdl_user
    {
        public int id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        public string avatar { get; set; }

        public DateTime creation_date { get; set; }

        public byte record_state { get; set; }
    }
}
