using System.ComponentModel.DataAnnotations;

namespace ReadBook.Models.Pdf
{
    public class AllBook
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; } = string.Empty;
        public int BookPages { get; set; }
        public int PageIndex { get; set; }
        public string WithHeight { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;
    }

}
