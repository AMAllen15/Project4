using System.ComponentModel.DataAnnotations;

namespace Project_3.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Id is required")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(50)]
        public string Author { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; }
        public int PublishedYear { get; set; }

        public bool IsAvailable { get; set; }
        public ICollection<Borrowing> Borrowings { get; set; }

        public ICollection<Reader> Readers { get; set; }
    }

}
