namespace TestProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [StringLength(50)]
        public string roleID { get; set; }

        [Required]
        [StringLength(50)]
        public string rolename { get; set; }

        public virtual Role Role1 { get; set; }

        public virtual Role Role2 { get; set; }
    }
}
