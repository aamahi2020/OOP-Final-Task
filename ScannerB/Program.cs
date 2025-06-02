using FileIndexingSystem.ScannerB;

namespace ScannerBApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scanner = new ScannerWorker("agent2");
            scanner.Run();
        }
    }
}
