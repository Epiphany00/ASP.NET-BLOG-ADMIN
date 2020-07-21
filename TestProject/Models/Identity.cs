namespace TestProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Identity")]
    public partial class Identity
    {
        public int IdentityID { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Keywords { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string LogoURL { get; set; }

        [StringLength(50)]
        public string degree { get; set; }
    }
}
