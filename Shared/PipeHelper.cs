using System.Text.Json;
using FileIndexingSystem.Models;

namespace FileIndexingSystem.Shared
{
    public static class PipeHelper
    {
        public static string Serialize(List<WordIndexEntry> entries) =>
            JsonSerializer.Serialize(entries);

        public static List<WordIndexEntry> Deserialize(string json) =>
            JsonSerializer.Deserialize<List<WordIndexEntry>>(json) ?? new List<WordIndexEntry>();
    }
}
