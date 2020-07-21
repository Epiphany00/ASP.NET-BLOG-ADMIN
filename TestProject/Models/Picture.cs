namespace TestProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Picture")]
    public partial class Picture
    {
        [Key]
        [StringLength(250)]
        public string PicID { get; set; }

        [StringLength(250)]
        public string bigpic { get; set; }

        [StringLength(250)]
        public string midpic { get; set; }

        [StringLength(250)]
        public string smallpic { get; set; }

        public bool? used { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userID { get; set; }
    }
}
