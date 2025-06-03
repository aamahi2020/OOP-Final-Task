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