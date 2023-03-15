namespace ReadBook.Models
{
    public class FileClass
    {
        public int FileId { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public bool IsModifide { get; set; } = false;
        public List<FileClass> Files { get; set; } = new List<FileClass>();
        public List<FileClass> LinkedFiles { get; set; } = new List<FileClass>();
    }
}
