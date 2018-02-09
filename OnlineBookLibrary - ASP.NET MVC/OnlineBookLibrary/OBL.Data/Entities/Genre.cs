namespace OBL.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [Display(Name = "Genre")]
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string GenreName { get; set; }

        [StringLength(500, MinimumLength = 1)]
        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
