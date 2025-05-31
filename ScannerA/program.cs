using FileIndexingSystem.ScannerA;

namespace ScannerAApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scanner = new ScannerWorker("agent1");
            scanner.Run();
        }
    }
}