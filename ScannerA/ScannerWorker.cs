using System.Diagnostics;
using System.IO.Pipes;
using FileIndexingSystem.Models;
using FileIndexingSystem.Shared;

namespace FileIndexingSystem.ScannerA
{
    public class ScannerWorker
    {
        private readonly string pipeName;

        public ScannerWorker(string pipeName)
        {
            this.pipeName = pipeName;
        }

        public void Run()
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x1;

            Console.WriteLine("Enter directory path:");
            string path = Console.ReadLine();

            var readThread = new Thread(() =>
            {
                var index = IndexFiles(path);
                var json = PipeHelper.Serialize(index);
                using var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out);
                client.Connect();
                using var writer = new StreamWriter(client) { AutoFlush = true };
                writer.WriteLine(json);
            });

            readThread.Start();
        }

        private List<WordIndexEntry> IndexFiles(string dir)
        {
            var list = new List<WordIndexEntry>();
            var files = Directory.GetFiles(dir, "*.txt");

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var words = content.Split(' ', '\n', '\r', '.', ',', ';', ':');
                var counts = words
                    .Where(w => !string.IsNullOrWhiteSpace(w))
                    .GroupBy(w => w.ToLower())
                    .ToDictionary(g => g.Key, g => g.Count());

                foreach (var pair in counts)
                {
                    list.Add(new WordIndexEntry
                    {
                        FileName = Path.GetFileName(file),
                        Word = pair.Key,
                        Count = pair.Value
                    });
                }
            }

            return list;
        }
    }
}
