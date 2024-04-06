using System.ComponentModel.DataAnnotations;

namespace Project_3.Models
{
    public class Reader
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Number is required")]
        [Phone]
        public string ContactNumber { get; set; }
        public ICollection<Borrowing> Borrowings { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
