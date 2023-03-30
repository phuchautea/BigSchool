namespace BigSchool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }

        public DateTime DateTime { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public List<Category> Categories = new List<Category>();

        public string Name;
        [NotMapped]
        public string Date { get; set; }
        [NotMapped]
        public string Time { get; set; }
        [NotMapped]
        public bool isShowGoing { get; set; } = false;
        [NotMapped]
        public bool isShowFollow { get; set; } = false;
        public bool isCanceled { get; set; } = false;
    }
}
