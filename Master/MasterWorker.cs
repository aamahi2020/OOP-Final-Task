using System.Diagnostics;
using System.IO.Pipes;
using FileIndexingSystem.Models;
using FileIndexingSystem.Shared;

namespace FileIndexingSystem.Master
{
    public class MasterWorker
    {
        private readonly string pipe1, pipe2;
        private List<WordIndexEntry> entries1 = new();
        private List<WordIndexEntry> entries2 = new();

        public MasterWorker(string p1, string p2)
        {
            pipe1 = p1;
            pipe2 = p2;
        }

        public void Run()
        {
#if WINDOWS
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x4;
#endif
            var t1 = new Thread(() => entries1 = Listen(pipe1));
            var t2 = new Thread(() => entries2 = Listen(pipe2));

            t1.Start(); t2.Start();
            t1.Join(); t2.Join();

            Console.WriteLine("\nPress ENTER to merge results...");
            Console.ReadLine();

            var merged = entries1.Concat(entries2)
                .GroupBy(e => e.Word)
                .Select(g => new WordIndexEntry
                {
                    FileName = "AllFiles",
                    Word = g.Key,
                    Count = g.Sum(e => e.Count)
                });

            Console.WriteLine("\nMerged Result (Total Word Count):");
            foreach (var entry in merged.OrderBy(e => e.Word))
                Console.WriteLine($"{entry.Word}:{entry.Count}");
        }

        private List<WordIndexEntry> Listen(string pipeName)
        {
            using var server = new NamedPipeServerStream(pipeName, PipeDirection.In);
            Console.WriteLine($"Waiting for {pipeName}...");
            server.WaitForConnection();
            using var reader = new StreamReader(server);
            var json = reader.ReadLine() ?? "[]";
            var entries = PipeHelper.Deserialize(json);

            Console.WriteLine($"\nData from {pipeName}:");
            foreach (var entry in entries)
                Console.WriteLine($"{entry.FileName}:{entry.Word}:{entry.Count}");

            return entries;
        }
    }
}
