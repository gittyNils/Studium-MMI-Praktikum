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

namespace A5
{
    class Program
    {
        static void Main(string[] args)
        {
            IGraph graph = new Graph("T1", true);

            graph.AddVertex("s");
            graph.AddVertex("u");
            graph.AddVertex("v");
            graph.AddVertex("w");
            graph.AddVertex("x");
            graph.AddVertex("t");

            var s = graph.Vertices["s"];
            var u = graph.Vertices["u"];
            var v = graph.Vertices["v"];
            var w = graph.Vertices["w"];
            var x = graph.Vertices["x"];
            var t = graph.Vertices["t"];

            string kapazität = Flow.KAPAZITÄT_VALUE;
            string fluss = Flow.FLUSS_VALUE;

            graph.AddEdge(s, u, new Dictionary<string, double> { { kapazität, 1 } });
            graph.AddEdge(s, w, new Dictionary<string, double> { { kapazität, 5 } });
            graph.AddEdge(u, w, new Dictionary<string, double> { { kapazität, 2 } });
            graph.AddEdge(w, x, new Dictionary<string, double> { { kapazität, 4 } });
            graph.AddEdge(x, v, new Dictionary<string, double> { { kapazität, 2 } });
            graph.AddEdge(u, v, new Dictionary<string, double> { { kapazität, 3 } });
            graph.AddEdge(v, t, new Dictionary<string, double> { { kapazität, 3 } });
            graph.AddEdge(x, t, new Dictionary<string, double> { { kapazität, 3 } });


            Flow.EdmondsKarp(graph, s, t);



            var outKanten = s.Edges.Values.Where(z => z.FromVertex == s);
            var inKanten = s.Edges.Values.Except(outKanten);

            var maxFluss = outKanten.Sum(z => z.Values[fluss]) - inKanten.Sum(z => z.Values[fluss]);
            








            

            string str = File.ReadAllText(@"C:\Users\NilsLP\Dropbox\GitRepo\MMI\Aufgaben\A4\SampleData\G_1_2.txt");
            IGraph g = GraphFactory.GraphFromAdjListStringWithCost(str, "T2", kapazität, true);

            s = g.Vertices["0"];
            t = g.Vertices["7"];

            Flow.EdmondsKarp(g, s, t);


            maxFluss = Flow.ReadMaxFlow(s);

            Console.WriteLine(maxFluss);


            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
