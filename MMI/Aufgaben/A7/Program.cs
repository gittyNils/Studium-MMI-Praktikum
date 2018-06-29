using GraphLibrary.Algorithm;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A7
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IVertex> setA;
            List<IVertex> setB;

            var s = File.ReadAllText(@"SampleData\Matching_100_100.txt");
            IGraph graph = GraphFactory.BipartitGraphFromAdjListString(s, "T1", true, out setA, out setB);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var matching = Matching.BipartitMatching(graph, setA, setB);
            sw.Stop();
            Console.WriteLine($"BipartitMatching {sw.ElapsedMilliseconds} ms");


            Console.ReadLine();

        }
    }
}
