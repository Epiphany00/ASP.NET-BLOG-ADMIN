namespace TestProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slider")]
    public partial class Slider
    {
        public int sliderID { get; set; }

        [StringLength(250)]
        public string title { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public string pictureURL { get; set; }
    }
}
