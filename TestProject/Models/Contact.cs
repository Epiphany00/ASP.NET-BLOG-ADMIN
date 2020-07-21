namespace TestProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public int contactID { get; set; }

        [StringLength(500)]
        public string Adress { get; set; }

        [StringLength(50)]
        public string phone { get; set; }
    }
}
