namespace FileIndexingSystem.Models
{
    public class WordIndexEntry
    {
        public string FileName { get; set; } = string.Empty;
        public string Word { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
