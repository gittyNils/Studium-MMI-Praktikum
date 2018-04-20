using GraphLibrary.Algorithm;
using GraphLibrary.Interface;
using GraphLibrary.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary.Factory;
using System.IO;

namespace A2
{
    class Program
    {
        static void Main(string[] args)
        {

            Graph g = new Graph("Test1", false);

            g.AddVertex("1");
            g.AddVertex("2");
            g.AddVertex("3");
            g.AddVertex("4");
            g.AddVertex("5");


            g.AddEdge(g.Vertices["1"], g.Vertices["2"], new Dictionary<string, double> { { "C", 13 } });
            g.AddEdge(g.Vertices["1"], g.Vertices["3"], new Dictionary<string, double> { { "C", 4 } });
            g.AddEdge(g.Vertices["1"], g.Vertices["5"], new Dictionary<string, double> { { "C", 9 } });
            g.AddEdge(g.Vertices["3"], g.Vertices["2"], new Dictionary<string, double> { { "C", 18 } });
            g.AddEdge(g.Vertices["3"], g.Vertices["4"], new Dictionary<string, double> { { "C", 7 } });
            g.AddEdge(g.Vertices["3"], g.Vertices["5"], new Dictionary<string, double> { { "C", 10 } });
            g.AddEdge(g.Vertices["2"], g.Vertices["4"], new Dictionary<string, double> { { "C", 12 } });
            g.AddEdge(g.Vertices["4"], g.Vertices["5"], new Dictionary<string, double> { { "C", 5 } });


            var a = MST.Prim(g, "C");


            var b = MST.Kruskal(g, "C");


            var costName = "Kosten";

            var s = File.ReadAllText(@"SampleData\G_1_2.txt");
            IGraph gTest = GraphFactory.GraphFromAdjListStringWithCost(s, "T1", costName, false);


            var sw = System.Diagnostics.Stopwatch.StartNew();


            var c = MST.Prim(gTest, costName);

            sw.Stop();
            Console.WriteLine($"Prim {sw.ElapsedMilliseconds} ms with ResultCost={c.Edges.Values.Sum(x => x.Costs[costName])}");


            sw.Restart();
            var d = MST.Kruskal(gTest, costName);


            Console.WriteLine($"Kruskal {sw.ElapsedMilliseconds} ms with ResultCost={d.Edges.Values.Sum(x => x.Costs[costName])}");

            Console.ReadLine();
        }
    }
}
