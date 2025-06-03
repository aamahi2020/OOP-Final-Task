using System.Diagnostics;
using System.IO.Pipes;
using FileIndexingSystem.Models;

namespace FileIndexingSystem.Master
{
    public class MasterWorker
    {
        private readonly string pipe1, pipe2;

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
            var thread1 = new Thread(() => Listen(pipe1));
            var thread2 = new Thread(() => Listen(pipe2));

            thread1.Start();
            thread2.Start();
        }

        private void Listen(string pipeName)
        {
            using var server = new NamedPipeServerStream(pipeName, PipeDirection.In);
            Console.WriteLine($"Waiting for {pipeName}...");
            server.WaitForConnection();
            using var reader = new StreamReader(server);
            var json = reader.ReadLine() ?? "[]";
            var entries = PipeHelper.Deserialize(json);

            Console.WriteLine($"\nData from {pipeName}:");
            foreach (var entry in entries)
            {
                Console.WriteLine($"{entry.FileName}:{entry.Word}:{entry.Count}");
            }
        }
    }
}
