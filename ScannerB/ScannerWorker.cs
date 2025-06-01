using System.Diagnostics;
using System.IO.Pipes;
using FileIndexingSystem.Models;
using FileIndexingSystem.Shared;

namespace FileIndexingSystem.ScannerB
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
#if WINDOWS
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x2;
#endif
            Console.WriteLine("Enter directory path:");
            string? path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                Console.WriteLine("Invalid path.");
                return;
            }

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