Objective 

This project implements a **distributed system** in C# using **three console applications**:

- `ScannerA`: Reads and indexes `.txt` files from a directory and sends data to the Master.
- `ScannerB`: Works similarly to ScannerA but uses a different pipe.
- `Master`: Collects indexed word data from both agents via **named pipes**, merges and displays results.

Technologies Used
- .NET 8.0 Console Applications
- C#
- Named Pipes: `NamedPipeClientStream`, `NamedPipeServerStream`
- Multithreading (`Thread`)
- Processor Affinity (`Processor.ProcessorAffinity`)

Project Structure
/Master
└── MasterWorker.cs
└── Program.cs
/ScannerA
└── ScannerWorker.cs
└── Program.cs
/ScannerB
└── ScannerWorker.cs
└── Program.cs
/Models
└── WordIndexEntry.cs
/Shared
└── PipeHelper.cs

Named Pipe Names
agent1 → used by `ScannerA`
agent2 → used by `ScannerB`

How to Run
1. Open Visual Studio
- Open the solution.
- Build the entire solution **in this order**:
  1. Models
  2. Shared
  3. All others
2. Run Master
- Set Master as Startup Project
- Run it first (Ctrl + F5)
- Output:
Waiting for agent1...
Waiting for agent2...
3. Run ScannerA and ScannerB
- Run ScannerA, enter path to a folder with .txt files  
- Then run ScannerB, enter another valid folder
Master will display indexed results from both agents.

Example Output

Waiting for agent1...
Waiting for agent2...

Data from agent1:
file1.txt:hello:3
file2.txt:system:2

Data from agent2:
file3.txt:master:1
file4.txt:pipe:2
UML Diagram

UML class diagram included in `docs/uml_diagram.png` showing:

- `MasterWorker`
- `ScannerWorker`
- `WordIndexEntry`
- `PipeHelper`
Multithreading
- Master uses **two threads** to listen to both pipes.
- Scanners use a separate thread for file reading and sending data.

CPU Affinity

Each app sets its process to a dedicated CPU core:
- Master → Core 3
- ScannerA → Core 1
- ScannerB → Core 2
 
