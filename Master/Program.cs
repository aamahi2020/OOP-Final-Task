using FileIndexingSystem.Master;

namespace MasterApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var master = new MasterWorker("agent1", "agent2");
            master.Run();
        }
    }
}

