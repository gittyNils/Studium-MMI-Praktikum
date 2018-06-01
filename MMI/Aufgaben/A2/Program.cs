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
        /// <summary>
        /// Informationen für einen Testlauf in dieser Aufgabe
        /// </summary>
        public class TestData
        {
            public string Name { get; set; }
            public string File { get; set; }
        }

        
        static void Main(string[] args)
        {
            /*
            Graph gSimple = new Graph("Test1", false);

            gSimple.AddVertex("1");
            gSimple.AddVertex("2");
            gSimple.AddVertex("3");
            gSimple.AddVertex("4");
            gSimple.AddVertex("5");


            gSimple.AddEdge(gSimple.Vertices["1"], gSimple.Vertices["2"], new Dictionary<string, double> { { "C", 13 } });
            gSimple.AddEdge(gSimple.Vertices["1"], gSimple.Vertices["3"], new Dictionary<string, double> { { "C", 4 } });
            gSimple.AddEdge(gSimple.Vertices["1"], gSimple.Vertices["5"], new Dictionary<string, double> { { "C", 9 } });
            gSimple.AddEdge(gSimple.Vertices["3"], gSimple.Vertices["2"], new Dictionary<string, double> { { "C", 18 } });
            gSimple.AddEdge(gSimple.Vertices["3"], gSimple.Vertices["4"], new Dictionary<string, double> { { "C", 7 } });
            gSimple.AddEdge(gSimple.Vertices["3"], gSimple.Vertices["5"], new Dictionary<string, double> { { "C", 10 } });
            gSimple.AddEdge(gSimple.Vertices["2"], gSimple.Vertices["4"], new Dictionary<string, double> { { "C", 12 } });
            gSimple.AddEdge(gSimple.Vertices["4"], gSimple.Vertices["5"], new Dictionary<string, double> { { "C", 5 } });


            var a = MST.PrimV2(gSimple, "C");


            var b = MST.Kruskal(gSimple, "C");
            

            var costName = "Kosten";

            //var s = File.ReadAllText(@"SampleData\G_1_2.txt");
            var s = File.ReadAllText(@"SampleData\G_100_200.txt");

            IGraph gTest = GraphFactory.GraphFromAdjListStringWithCost(s, "T1", costName, false);


            var sw = System.Diagnostics.Stopwatch.StartNew();


            //var c = MST.Prim(gTest, costName);

            //sw.Stop();
            //Console.WriteLine($"Prim {sw.ElapsedMilliseconds} ms with ResultCost={c.Edges.Values.Sum(x => x.Costs[costName])}");


            sw.Restart();
            var d1 = MST.KruskalV1(gTest, costName);
            sw.Stop();

            Console.WriteLine($"KruskalV1 {sw.ElapsedMilliseconds} ms with ResultCost={d1.Edges.Values.Sum(x => x.Costs[costName])}");


            sw.Restart();
            var d = MST.Kruskal(gTest, costName);
            sw.Stop();

            Console.WriteLine($"Kruskal {sw.ElapsedMilliseconds} ms with ResultCost={d.Edges.Values.Sum(x => x.Costs[costName])}");

            Console.ReadLine();

            */





            var costName = "Kosten";
            List<TestData> testFiles = new List<TestData>();

            testFiles.Add(new TestData { Name = "G_1_2", File = @"SampleData\G_1_2.txt" });
            testFiles.Add(new TestData { Name = "G_1_20", File = @"SampleData\G_1_20.txt" });
            testFiles.Add(new TestData { Name = "G_1_200", File = @"SampleData\G_1_200.txt" });
            testFiles.Add(new TestData { Name = "G_10_20", File = @"SampleData\G_10_20.txt" });
            testFiles.Add(new TestData { Name = "G_10_200", File = @"SampleData\G_10_200.txt" });
            testFiles.Add(new TestData { Name = "G_100_200", File = @"SampleData\G_100_200.txt" });



            foreach (var data in testFiles)
            {
                Console.WriteLine($"------------------ Start Name={data.Name} ------------------");

                // Initialisieren

                // Einlesen in String 
                string s = File.ReadAllText(data.File);

                var swInit = System.Diagnostics.Stopwatch.StartNew();
                IGraph g = GraphFactory.GraphFromAdjListStringWithCost(s, data.Name, costName, false);
                swInit.Stop();
                Console.WriteLine($"Init took {swInit.ElapsedMilliseconds} ms");

                // Rechnen


                var swCalc = System.Diagnostics.Stopwatch.StartNew();
                var k = MST.Kruskal(g, costName);
                swCalc.Stop();
                Console.WriteLine($"Kruskal {swCalc.ElapsedMilliseconds} ms with ResultCost={k.Edges.Values.Sum(x => x.Values[costName])}");

                swCalc.Restart();
                k = MST.KruskalV1(g, costName);
                swCalc.Stop();
                Console.WriteLine($"KruskalV1 (Dict) {swCalc.ElapsedMilliseconds} ms with ResultCost={k.Edges.Values.Sum(x => x.Values[costName])}");
                


                swCalc.Restart();
                var p = MST.Prim(g, costName);
                swCalc.Stop();
                Console.WriteLine($"Prim {swCalc.ElapsedMilliseconds} ms with ResultCost={p.Edges.Values.Sum(x => x.Values[costName])}");
            }




            Console.ReadLine();
        }
    }
}
