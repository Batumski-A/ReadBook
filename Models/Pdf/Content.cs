using System.ComponentModel.DataAnnotations;

namespace ReadBook.Models.Pdf
{
    public class Content
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ContentName { get; set; } = string.Empty;
        public int PdfRequestId { get; set; }
        public int PageIndex { get; set; }
        public string TextPosition { get; set; } = string.Empty;
        public string TextWidthAndHeight { get; set; } = string.Empty;
        public bool LinkType { get; set; }
        public int GoToPage { get; set; }
        public string GoToPageTextPostion { get; set; } = string.Empty;
        public string GoToPageTextWidthAndHeight { get; set; } = string.Empty;
    }
}
