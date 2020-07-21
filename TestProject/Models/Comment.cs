namespace TestProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int CommentID { get; set; }

        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [Column("Comment")]
        [Required]
        public string Comment1 { get; set; }

        public int? blogID { get; set; }

        public int? UserID { get; set; }

        [Required]
        [StringLength(250)]
        public string validation { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual User User { get; set; }
    }
}
