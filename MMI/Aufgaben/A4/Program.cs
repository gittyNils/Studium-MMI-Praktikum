using GraphLibrary.Algorithm;
using GraphLibrary.DataModel;
using GraphLibrary.Factory;
using GraphLibrary.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A4
{
    /// <summary>
    /// Informationen für einen Testlauf in dieser Aufgabe
    /// </summary>
    public class TestData
    {
        public string Name { get; set; }
        public string File { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var costName = CONST.KOSTEN_VALUE;

            /*
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
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");*/



            List<TestData> testFiles = new List<TestData>();

            testFiles.Add(new TestData { Name = "G_1_2", File = @"SampleData\G_1_2.txt" });
            testFiles.Add(new TestData { Name = "G_1_20", File = @"SampleData\G_1_20.txt" });
            testFiles.Add(new TestData { Name = "G_1_200", File = @"SampleData\G_1_200.txt" });
            testFiles.Add(new TestData { Name = "G_10_20", File = @"SampleData\G_10_20.txt" });
            testFiles.Add(new TestData { Name = "G_10_200", File = @"SampleData\G_10_200.txt" });
            testFiles.Add(new TestData { Name = "G_100_200", File = @"SampleData\G_100_200.txt" });

            foreach (var test in testFiles)
            {
                Console.WriteLine("---------------------------------------------------------------");
                var sw = new System.Diagnostics.Stopwatch();


                string s = File.ReadAllText(test.File);
                IGraph graph = GraphFactory.GraphFromAdjListStringWithCost(s, test.Name, costName, true);

                sw.Restart();
                var big = ShortestPath.Dijkstra(graph, graph.Vertices.First().Key, costName);
                sw.Stop();
                Console.WriteLine($"Dijkstra         {sw.ElapsedMilliseconds} ms");


                sw.Restart();
                IEdge cycleEdge;
                big = ShortestPath.MooreBellmanFord(graph, graph.Vertices.First().Key, costName, out cycleEdge);
                sw.Stop();
                Console.WriteLine($"MooreBellmanFord {sw.ElapsedMilliseconds} ms");
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }


        //static void AddEdgeForCost(IGraph g, string from, string to, string costName, double cost)
        //{
        //    var fromVert = g.Vertices[from];
        //    var toVert = g.Vertices[to];

        //    g.AddEdge(fromVert, toVert, new Dictionary<string, double> { { costName, cost } });
        //}
    }
}
