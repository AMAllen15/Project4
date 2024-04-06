using System.ComponentModel.DataAnnotations;

namespace Project_3.Models
{
    public class Borrowing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [Required]
        public int ReaderId { get; set; }
        public Reader? Reader { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }

}
