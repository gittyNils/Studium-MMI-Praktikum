using GraphLibrary.Algorithm;
using GraphLibrary.DataModel;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A4
{
    class Program
    {
        static void Main(string[] args)
        {
            var costName = "Kosten";

            // Testgraph
            IGraph gTest = new Graph("Test", true);

            foreach (string id in new string[] { "a", "b", "c", "d", "e", "f", "s" })
            {
                gTest.AddVertex(id);
            }

            AddEdgeForCost(gTest, "s", "a", costName, 1);
            AddEdgeForCost(gTest, "a", "d", costName, 2);
            AddEdgeForCost(gTest, "d", "f", costName, 6);
            AddEdgeForCost(gTest, "e", "f", costName, 1);
            AddEdgeForCost(gTest, "b", "e", costName, 1);
            AddEdgeForCost(gTest, "s", "b", costName, 4);

            AddEdgeForCost(gTest, "s", "c", costName, 3);
            AddEdgeForCost(gTest, "a", "c", costName, 1);
            AddEdgeForCost(gTest, "b", "c", costName, 6);
            AddEdgeForCost(gTest, "c", "e", costName, 4);
            AddEdgeForCost(gTest, "c", "f", costName, 6);

            // Assembly-Ladezeiten nicht in Zeitmessung
            ShortestPath.Dijkstra(gTest, "s", costName);
            ShortestPath.Dijkstra(gTest, "s", costName);
            ShortestPath.Dijkstra(gTest, "s", costName);
            ShortestPath.MooreBellmanFord(gTest, "s", costName);
            ShortestPath.MooreBellmanFord(gTest, "s", costName);
            ShortestPath.MooreBellmanFord(gTest, "s", costName);
            // Assembly-Ladezeiten nicht in Zeitmessung


            var sw = System.Diagnostics.Stopwatch.StartNew();
            var a = ShortestPath.Dijkstra(gTest, "s", costName);
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");

            sw.Restart();
            var b = ShortestPath.MooreBellmanFord(gTest, "s", costName);
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");

            Console.ReadLine();
        }


        static void AddEdgeForCost(IGraph g, string from, string to, string costName, double cost)
        {
            var fromVert = g.Vertices[from];
            var toVert = g.Vertices[to];

            g.AddEdge(fromVert, toVert, new Dictionary<string, double> { { costName, cost } });
        }
    }
}
